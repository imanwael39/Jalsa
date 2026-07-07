using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Identity;
using Jalsa.Domain.Models.Patient;
using Moq;

namespace Jalsa.Tests;

public class TherapistAdminServiceTests
{
    private readonly List<Therapist> _therapists = [];
    private readonly List<Patient> _patients = [];
    private readonly List<RefreshToken> _refreshTokens = [];

    private readonly Mock<IGenericRepository<Therapist>> _therapistRepoMock;
    private readonly Mock<IGenericRepository<Patient>> _patientRepoMock;
    private readonly Mock<IGenericRepository<RefreshToken>> _refreshTokenRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAuditLogService> _auditLogServiceMock;
    private readonly TherapistAdminService _sut;

    public TherapistAdminServiceTests()
    {
        _therapistRepoMock = new Mock<IGenericRepository<Therapist>>();
        _therapistRepoMock.Setup(r => r.Query()).Returns(() => _therapists.AsQueryable());
        _therapistRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<Therapist, bool>> predicate, CancellationToken _) =>
                _therapists.AsQueryable().FirstOrDefault(predicate));

        _patientRepoMock = new Mock<IGenericRepository<Patient>>();
        _patientRepoMock.Setup(r => r.Query()).Returns(() => _patients.AsQueryable());

        _refreshTokenRepoMock = new Mock<IGenericRepository<RefreshToken>>();
        _refreshTokenRepoMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<RefreshToken, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<RefreshToken, bool>> predicate, CancellationToken _) =>
                _refreshTokens.AsQueryable().Where(predicate).ToList());

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(u => u.Repository<Therapist>()).Returns(_therapistRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Patient>()).Returns(_patientRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<RefreshToken>()).Returns(_refreshTokenRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _auditLogServiceMock = new Mock<IAuditLogService>();
        _auditLogServiceMock
            .Setup(a => a.GetLogsAsync(It.IsAny<AuditLogFilterDto>()))
            .ReturnsAsync(new PagedResultDto<AuditLogViewDto> { Items = [], TotalCount = 0, Page = 1, PageSize = 10 });

        _sut = new TherapistAdminService(_unitOfWorkMock.Object, _auditLogServiceMock.Object);
    }

    private (Therapist therapist, User user) SeedTherapist(string approvalStatus = "Pending")
    {
        var user = new User { Id = Guid.NewGuid(), Email = "dr@test.com", PasswordHash = "x", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var therapist = new Therapist
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            User = user,
            FullName = "Dr. Test",
            LicenseNumber = "LIC-1",
            ApprovalStatus = approvalStatus,
            CreatedAt = DateTime.UtcNow
        };
        _therapists.Add(therapist);
        return (therapist, user);
    }

    [Fact]
    public async Task GetTherapistsAsync_FiltersByApprovalStatus()
    {
        SeedTherapist("Pending");
        SeedTherapist("Approved");

        var result = await _sut.GetTherapistsAsync(new TherapistFilterDto { ApprovalStatus = "Approved" });

        result.TotalCount.Should().Be(1);
        result.Items.Single().ApprovalStatus.Should().Be("Approved");
    }

    [Fact]
    public async Task GetTherapistsAsync_ComputesPatientCount()
    {
        var (therapist, _) = SeedTherapist();
        _patients.Add(new Patient { Id = Guid.NewGuid(), TherapistId = therapist.Id, FullName = "P1" });
        _patients.Add(new Patient { Id = Guid.NewGuid(), TherapistId = therapist.Id, FullName = "P2" });
        _patients.Add(new Patient { Id = Guid.NewGuid(), TherapistId = Guid.NewGuid(), FullName = "OtherTherapistPatient" });

        var result = await _sut.GetTherapistsAsync(new TherapistFilterDto());

        result.Items.Single().PatientCount.Should().Be(2);
    }

    [Fact]
    public async Task GetTherapistDetailAsync_NotFound_ReturnsNull()
    {
        var result = await _sut.GetTherapistDetailAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTherapistDetailAsync_Found_IncludesRecentAuditLogs()
    {
        var (therapist, _) = SeedTherapist();
        _auditLogServiceMock
            .Setup(a => a.GetLogsAsync(It.Is<AuditLogFilterDto>(f => f.EntityName == "Therapist" && f.EntityId == therapist.Id.ToString())))
            .ReturnsAsync(new PagedResultDto<AuditLogViewDto>
            {
                Items = [new AuditLogViewDto { Action = "Therapist.ApprovalStatus" }],
                TotalCount = 1
            });

        var result = await _sut.GetTherapistDetailAsync(therapist.Id);

        result.Should().NotBeNull();
        result!.RecentAuditLogs.Should().ContainSingle(l => l.Action == "Therapist.ApprovalStatus");
    }

    [Fact]
    public async Task UpdateApprovalStatusAsync_InvalidStatus_Throws()
    {
        var (therapist, _) = SeedTherapist();

        var act = () => _sut.UpdateApprovalStatusAsync(therapist.Id, "NotARealStatus");

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task UpdateApprovalStatusAsync_NotFound_ReturnsNull()
    {
        var result = await _sut.UpdateApprovalStatusAsync(Guid.NewGuid(), TherapistApprovalStatus.Approved);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateApprovalStatusAsync_ToSuspended_RevokesActiveRefreshTokens()
    {
        var (therapist, user) = SeedTherapist("Approved");
        var activeToken = new RefreshToken { Id = Guid.NewGuid(), UserId = user.Id, Token = "t1", ExpiresAt = DateTime.UtcNow.AddDays(1), CreatedAt = DateTime.UtcNow };
        var alreadyRevokedToken = new RefreshToken { Id = Guid.NewGuid(), UserId = user.Id, Token = "t2", ExpiresAt = DateTime.UtcNow.AddDays(1), CreatedAt = DateTime.UtcNow, RevokedAt = DateTime.UtcNow.AddHours(-1) };
        _refreshTokens.Add(activeToken);
        _refreshTokens.Add(alreadyRevokedToken);

        var result = await _sut.UpdateApprovalStatusAsync(therapist.Id, TherapistApprovalStatus.Suspended);

        result.Should().NotBeNull();
        result!.ApprovalStatus.Should().Be(TherapistApprovalStatus.Suspended);
        activeToken.RevokedAt.Should().NotBeNull();
        alreadyRevokedToken.RevokedAt.Should().BeCloseTo(DateTime.UtcNow.AddHours(-1), TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task UpdateApprovalStatusAsync_ToApproved_DoesNotRevokeTokens()
    {
        var (therapist, user) = SeedTherapist("Pending");
        var activeToken = new RefreshToken { Id = Guid.NewGuid(), UserId = user.Id, Token = "t1", ExpiresAt = DateTime.UtcNow.AddDays(1), CreatedAt = DateTime.UtcNow };
        _refreshTokens.Add(activeToken);

        await _sut.UpdateApprovalStatusAsync(therapist.Id, TherapistApprovalStatus.Approved);

        activeToken.RevokedAt.Should().BeNull();
    }

    [Fact]
    public async Task UpdateApprovalStatusAsync_WritesAuditLogWithOldAndNewStatus()
    {
        var (therapist, _) = SeedTherapist("Pending");

        await _sut.UpdateApprovalStatusAsync(therapist.Id, TherapistApprovalStatus.Approved, Guid.NewGuid(), "1.2.3.4", "TestAgent");

        _auditLogServiceMock.Verify(a => a.LogAsync(
            It.IsAny<Guid?>(), "Therapist", therapist.Id.ToString(), "Therapist.ApprovalStatus",
            It.IsAny<object>(), It.IsAny<object>(), "1.2.3.4", "TestAgent"), Times.Once);
    }
}
