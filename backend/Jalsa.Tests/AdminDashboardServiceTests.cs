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

public class AdminDashboardServiceTests
{
    private readonly List<User> _users = [];
    private readonly List<Therapist> _therapists = [];
    private readonly List<Patient> _patients = [];

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAuditLogService> _auditLogServiceMock;
    private readonly Mock<IUserManagementService> _userManagementServiceMock;
    private readonly AdminDashboardService _sut;

    public AdminDashboardServiceTests()
    {
        var userRepoMock = new Mock<IGenericRepository<User>>();
        userRepoMock.Setup(r => r.Query()).Returns(() => _users.AsQueryable());

        var therapistRepoMock = new Mock<IGenericRepository<Therapist>>();
        therapistRepoMock.Setup(r => r.Query()).Returns(() => _therapists.AsQueryable());

        var patientRepoMock = new Mock<IGenericRepository<Patient>>();
        patientRepoMock.Setup(r => r.Query()).Returns(() => _patients.AsQueryable());

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Therapist>()).Returns(therapistRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Patient>()).Returns(patientRepoMock.Object);

        _auditLogServiceMock = new Mock<IAuditLogService>();
        _auditLogServiceMock
            .Setup(a => a.GetLogsAsync(It.IsAny<AuditLogFilterDto>()))
            .ReturnsAsync(new PagedResultDto<AuditLogViewDto> { Items = [], TotalCount = 0 });

        _userManagementServiceMock = new Mock<IUserManagementService>();
        _userManagementServiceMock.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(() =>
            _users.OrderByDescending(u => u.CreatedAt).Select(u => new UserAdminViewDto { Id = u.Id, Email = u.Email, CreatedAt = u.CreatedAt }));

        _sut = new AdminDashboardService(_unitOfWorkMock.Object, _auditLogServiceMock.Object, _userManagementServiceMock.Object);
    }

    private void AddUser(bool isActive = true, bool isDeleted = false, string? role = null, DateTime? createdAt = null)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = $"{Guid.NewGuid()}@test.com",
            PasswordHash = "x",
            IsActive = isActive,
            IsDeleted = isDeleted,
            CreatedAt = createdAt ?? DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (role != null)
        {
            var r = new Role { Id = Guid.NewGuid(), Name = role };
            user.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = r.Id, Role = r });
        }

        _users.Add(user);
    }

    [Fact]
    public async Task GetDashboardAsync_ComputesUserCounts()
    {
        AddUser(isActive: true, role: "Admin");
        AddUser(isActive: true);
        AddUser(isActive: false);

        var result = await _sut.GetDashboardAsync();

        result.TotalUsers.Should().Be(3);
        result.TotalAdmins.Should().Be(1);
        result.ActiveUsers.Should().Be(2);
        result.DisabledUsers.Should().Be(1);
    }

    [Fact]
    public async Task GetDashboardAsync_ComputesNewRegistrationWindows()
    {
        AddUser(createdAt: DateTime.UtcNow.AddDays(-2));
        AddUser(createdAt: DateTime.UtcNow.AddDays(-10));
        AddUser(createdAt: DateTime.UtcNow.AddDays(-40));

        var result = await _sut.GetDashboardAsync();

        result.NewRegistrations7Days.Should().Be(1);
        result.NewRegistrations30Days.Should().Be(2);
    }

    [Fact]
    public async Task GetDashboardAsync_CountsPendingDoctorApprovals()
    {
        _therapists.Add(new Therapist { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), FullName = "A", LicenseNumber = "L1", ApprovalStatus = "Pending", CreatedAt = DateTime.UtcNow });
        _therapists.Add(new Therapist { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), FullName = "B", LicenseNumber = "L2", ApprovalStatus = "Approved", CreatedAt = DateTime.UtcNow });

        var result = await _sut.GetDashboardAsync();

        result.TotalTherapists.Should().Be(2);
        result.PendingDoctorApprovals.Should().Be(1);
    }

    [Fact]
    public async Task GetDashboardAsync_LimitsLatestRegisteredUsersToTen()
    {
        for (var i = 0; i < 15; i++)
            AddUser(createdAt: DateTime.UtcNow.AddMinutes(-i));

        var result = await _sut.GetDashboardAsync();

        result.LatestRegisteredUsers.Should().HaveCount(10);
    }
}
