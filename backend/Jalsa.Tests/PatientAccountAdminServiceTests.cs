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

public class PatientAccountAdminServiceTests
{
    private readonly List<Patient> _patients = [];
    private readonly List<User> _users = [];

    private readonly Mock<IGenericRepository<Patient>> _patientRepoMock;
    private readonly Mock<IGenericRepository<User>> _userRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAuditLogService> _auditLogServiceMock;
    private readonly PatientAccountAdminService _sut;

    public PatientAccountAdminServiceTests()
    {
        _patientRepoMock = new Mock<IGenericRepository<Patient>>();
        _patientRepoMock.Setup(r => r.Query()).Returns(() => _patients.AsQueryable());
        _patientRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<Patient, bool>> predicate, CancellationToken _) =>
                _patients.AsQueryable().FirstOrDefault(predicate));

        _userRepoMock = new Mock<IGenericRepository<User>>();
        _userRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<User, bool>> predicate, CancellationToken _) =>
                _users.AsQueryable().FirstOrDefault(predicate));

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(u => u.Repository<Patient>()).Returns(_patientRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _auditLogServiceMock = new Mock<IAuditLogService>();

        _sut = new PatientAccountAdminService(_unitOfWorkMock.Object, _auditLogServiceMock.Object);
    }

    private (Patient patient, User? user) SeedPatient(bool withLoginAccount = true)
    {
        var therapist = new Therapist { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), FullName = "Dr. Owner", LicenseNumber = "LIC-1", CreatedAt = DateTime.UtcNow };
        User? user = null;
        if (withLoginAccount)
        {
            user = new User { Id = Guid.NewGuid(), Email = "patient@test.com", PasswordHash = "x", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            _users.Add(user);
        }

        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            TherapistId = therapist.Id,
            Therapist = therapist,
            UserId = user?.Id,
            User = user,
            FullName = "Test Patient",
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _patients.Add(patient);
        return (patient, user);
    }

    [Fact]
    public async Task GetPatientAccountsAsync_ProjectsAccountFieldsOnly_NeverClinicalData()
    {
        var (patient, _) = SeedPatient();

        var result = await _sut.GetPatientAccountsAsync(new PatientAccountFilterDto());

        var dto = result.Items.Single();
        dto.Id.Should().Be(patient.Id);
        dto.TherapistName.Should().Be("Dr. Owner");
        dto.IsActive.Should().BeTrue();

        // Compile-time guarantee: PatientAccountAdminViewDto has no MedicalHistory/ChiefComplaint/
        // EmergencyContact/Sessions/Exercises/Reports properties at all — nothing to assert further,
        // the type itself proves clinical data cannot leak through this projection.
    }

    [Fact]
    public async Task GetPatientAccountsAsync_PatientWithNoLoginAccount_HasNullActiveState()
    {
        SeedPatient(withLoginAccount: false);

        var result = await _sut.GetPatientAccountsAsync(new PatientAccountFilterDto());

        var dto = result.Items.Single();
        dto.UserId.Should().BeNull();
        dto.IsActive.Should().BeNull();
    }

    [Fact]
    public async Task DisableAccountAsync_PatientNotFound_ReturnsNull()
    {
        var result = await _sut.DisableAccountAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task DisableAccountAsync_NoLinkedUser_ThrowsWithClearMessage()
    {
        var (patient, _) = SeedPatient(withLoginAccount: false);

        var act = () => _sut.DisableAccountAsync(patient.Id);

        var ex = await act.Should().ThrowAsync<InvalidOperationException>();
        ex.Which.Message.Should().Contain("لا يوجد حساب دخول");
    }

    [Fact]
    public async Task DisableAccountAsync_LinkedUser_SetsInactiveAndLogsAudit()
    {
        var (patient, user) = SeedPatient();

        var result = await _sut.DisableAccountAsync(patient.Id, Guid.NewGuid(), "1.2.3.4", "Agent");

        result.Should().NotBeNull();
        user!.IsActive.Should().BeFalse();
        _auditLogServiceMock.Verify(a => a.LogAsync(
            It.IsAny<Guid?>(), "Patient", patient.Id.ToString(), "Patient.DisableAccount",
            It.IsAny<object>(), It.IsAny<object>(), "1.2.3.4", "Agent"), Times.Once);
    }

    [Fact]
    public async Task RestoreAccountAsync_ClearsInactiveAndDeletedFlags()
    {
        var (patient, user) = SeedPatient();
        user!.IsActive = false;
        user.IsDeleted = true;

        var result = await _sut.RestoreAccountAsync(patient.Id);

        result.Should().NotBeNull();
        user.IsActive.Should().BeTrue();
        user.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAccountAsync_SetsIsDeletedTrue()
    {
        var (patient, user) = SeedPatient();

        var result = await _sut.DeleteAccountAsync(patient.Id);

        result.Should().NotBeNull();
        user!.IsDeleted.Should().BeTrue();
    }
}
