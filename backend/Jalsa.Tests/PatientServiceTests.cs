using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.DTOs.Patient;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Identity;
using Jalsa.Domain.Models.Patient;
using Moq;

namespace Jalsa.Tests;

public class PatientServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Patient>> _patientRepoMock;
    private readonly Mock<IGenericRepository<Therapist>> _therapistRepoMock;
    private readonly Mock<IGenericRepository<User>> _userRepoMock;
    private readonly Mock<IGenericRepository<Role>> _roleRepoMock;
    private readonly PatientService _sut;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _therapistId = Guid.NewGuid();
    private readonly Guid _otherTherapistId = Guid.NewGuid();

    public PatientServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _patientRepoMock = new Mock<IGenericRepository<Patient>>();
        _therapistRepoMock = new Mock<IGenericRepository<Therapist>>();
        _userRepoMock = new Mock<IGenericRepository<User>>();
        _roleRepoMock = new Mock<IGenericRepository<Role>>();

        _unitOfWorkMock.Setup(x => x.Repository<Patient>()).Returns(_patientRepoMock.Object);
        _unitOfWorkMock.Setup(x => x.Repository<Therapist>()).Returns(_therapistRepoMock.Object);
        _unitOfWorkMock.Setup(x => x.Repository<User>()).Returns(_userRepoMock.Object);
        _unitOfWorkMock.Setup(x => x.Repository<Role>()).Returns(_roleRepoMock.Object);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _therapistRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Therapist { Id = _therapistId, UserId = _userId, FullName = "Dr. Test", LicenseNumber = "LIC-001" });

        _userRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        _roleRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Role, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Role { Id = Guid.NewGuid(), Name = "Patient" });

        _sut = new PatientService(_unitOfWorkMock.Object);
    }

    private static PatientCreateDto MakeCreateDto(string name = "أحمد محمد") => new()
    {
        FullName = name,
        DateOfBirth = new DateOnly(1990, 5, 15),
        Gender = "ذكر",
        Phone = "01012345678",
        Email = "patient@test.com",
        Password = "Passw0rd123",
        ChiefComplaint = "قلق عام"
    };

    private Patient MakePatient(Guid? id = null, Guid? therapistId = null, string status = "Active") => new()
    {
        Id = id ?? Guid.NewGuid(),
        TherapistId = therapistId ?? _therapistId,
        FullName = "أحمد محمد",
        DateOfBirth = new DateOnly(1990, 5, 15),
        Gender = "ذكر",
        Phone = "01012345678",
        Status = status,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    // --- Create ---

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsPatientViewDto()
    {
        var dto = MakeCreateDto();

        var result = await _sut.CreateAsync(dto, _userId);

        result.FullName.Should().Be(dto.FullName);
        result.TherapistId.Should().Be(_therapistId);
        result.Status.Should().Be("Active");
        _patientRepoMock.Verify(x => x.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithEmergencyContactAndTreatmentStartDate_PersistsAllFields()
    {
        var dto = MakeCreateDto();
        dto.EmergencyContactName = "سارة عبدالله";
        dto.EmergencyContactRelationship = "الأخت";
        dto.EmergencyContactPhone = "+201190980339";
        dto.TreatmentStartDate = new DateOnly(2026, 1, 15);

        var result = await _sut.CreateAsync(dto, _userId);

        result.EmergencyContactName.Should().Be("سارة عبدالله");
        result.EmergencyContactRelationship.Should().Be("الأخت");
        result.EmergencyContactPhone.Should().Be("+201190980339");
        result.TreatmentStartDate.Should().Be(new DateOnly(2026, 1, 15));
    }

    [Fact]
    public async Task CreateAsync_WithEmailAndPassword_CreatesLinkedUserAccount()
    {
        var dto = MakeCreateDto();

        var result = await _sut.CreateAsync(dto, _userId);

        result.UserId.Should().NotBeNull();
        _userRepoMock.Verify(x => x.AddAsync(
            It.Is<User>(u => u.Email == dto.Email && u.UserRoles.Any(ur => ur.RoleId != Guid.Empty)),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_NoEmail_DoesNotCreateUserAccount()
    {
        var dto = MakeCreateDto();
        dto.Email = null;
        dto.Password = null;

        var result = await _sut.CreateAsync(dto, _userId);

        result.UserId.Should().BeNull();
        _userRepoMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_EmailWithoutPassword_ThrowsInvalidOperation()
    {
        var dto = MakeCreateDto();
        dto.Password = null;

        var act = () => _sut.CreateAsync(dto, _userId);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task CreateAsync_WeakPassword_ThrowsInvalidOperation()
    {
        var dto = MakeCreateDto();
        dto.Password = "weak";

        var act = () => _sut.CreateAsync(dto, _userId);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task CreateAsync_EmailAlreadyRegistered_ThrowsInvalidOperation()
    {
        var dto = MakeCreateDto();
        _userRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Id = Guid.NewGuid(), Email = dto.Email!, PasswordHash = "hash", IsActive = true });

        var act = () => _sut.CreateAsync(dto, _userId);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task CreateAsync_NoTherapistProfile_ThrowsUnauthorized()
    {
        var unknownUserId = Guid.NewGuid();
        _therapistRepoMock
            .Setup(x => x.FindSingleAsync(It.Is<Expression<Func<Therapist, bool>>>(e => true), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Therapist?)null);

        var sut = new PatientService(_unitOfWorkMock.Object);

        var act = () => sut.CreateAsync(MakeCreateDto(), unknownUserId);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    // --- GetById ---

    [Fact]
    public async Task GetByIdAsync_OwnPatient_ReturnsDto()
    {
        var patientId = Guid.NewGuid();
        var patient = MakePatient(id: patientId);

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        var result = await _sut.GetByIdAsync(patientId, _userId);

        result.Id.Should().Be(patientId);
        result.FullName.Should().Be(patient.FullName);
    }

    [Fact]
    public async Task GetByIdAsync_OtherTherapistPatient_ThrowsNotFound()
    {
        var patientId = Guid.NewGuid();
        var patient = MakePatient(id: patientId, therapistId: _otherTherapistId);

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        var act = () => _sut.GetByIdAsync(patientId, _userId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetByIdAsync_NonExistent_ThrowsNotFound()
    {
        _patientRepoMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        var act = () => _sut.GetByIdAsync(Guid.NewGuid(), _userId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    // --- GetAll ---

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyOwnPatients()
    {
        var patients = new List<Patient>
        {
            MakePatient(therapistId: _therapistId),
            MakePatient(therapistId: _therapistId),
            MakePatient(therapistId: _otherTherapistId)
        };

        _patientRepoMock.Setup(x => x.Query()).Returns(patients.AsQueryable());

        var result = await _sut.GetAllAsync(_userId);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllAsync_WithStatusFilter_FiltersCorrectly()
    {
        var patients = new List<Patient>
        {
            MakePatient(therapistId: _therapistId, status: "Active"),
            MakePatient(therapistId: _therapistId, status: "Archived")
        };

        _patientRepoMock.Setup(x => x.Query()).Returns(patients.AsQueryable());

        var result = await _sut.GetAllAsync(_userId, new PatientFilterDto { Status = "Active" });

        result.Should().HaveCount(1);
        result.First().Status.Should().Be("Active");
    }

    [Fact]
    public async Task GetAllAsync_WithSearchTerm_MatchesByName()
    {
        var patients = new List<Patient>
        {
            MakePatient(therapistId: _therapistId),
            MakePatient(therapistId: _therapistId)
        };
        patients[1].FullName = "سارة علي";

        _patientRepoMock.Setup(x => x.Query()).Returns(patients.AsQueryable());

        var result = await _sut.GetAllAsync(_userId, new PatientFilterDto { SearchTerm = "سارة" });

        result.Should().HaveCount(1);
        result.First().FullName.Should().Be("سارة علي");
    }

    // --- Update ---

    [Fact]
    public async Task UpdateAsync_OwnPatient_UpdatesAndReturns()
    {
        var patientId = Guid.NewGuid();
        var patient = MakePatient(id: patientId);

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        var updateDto = new PatientUpdateDto
        {
            FullName = "اسم جديد",
            Gender = "ذكر"
        };

        var result = await _sut.UpdateAsync(patientId, updateDto, _userId);

        result.FullName.Should().Be("اسم جديد");
        _patientRepoMock.Verify(x => x.Update(It.IsAny<Patient>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithEmergencyContactAndTreatmentStartDate_UpdatesAllFields()
    {
        var patientId = Guid.NewGuid();
        var patient = MakePatient(id: patientId);

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        var updateDto = new PatientUpdateDto
        {
            FullName = patient.FullName,
            EmergencyContactName = "منى صبحي",
            EmergencyContactRelationship = "الأخت",
            EmergencyContactPhone = "+201025779044",
            TreatmentStartDate = new DateOnly(2026, 2, 1)
        };

        var result = await _sut.UpdateAsync(patientId, updateDto, _userId);

        result.EmergencyContactName.Should().Be("منى صبحي");
        result.EmergencyContactRelationship.Should().Be("الأخت");
        result.EmergencyContactPhone.Should().Be("+201025779044");
        result.TreatmentStartDate.Should().Be(new DateOnly(2026, 2, 1));
    }

    [Fact]
    public async Task UpdateAsync_ClearingEmergencyContact_SetsFieldsToNull()
    {
        var patientId = Guid.NewGuid();
        var patient = MakePatient(id: patientId);
        patient.EmergencyContactName = "اسم قديم";
        patient.EmergencyContactRelationship = "علاقة قديمة";
        patient.EmergencyContactPhone = "0100000000";

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        var updateDto = new PatientUpdateDto { FullName = patient.FullName };

        var result = await _sut.UpdateAsync(patientId, updateDto, _userId);

        result.EmergencyContactName.Should().BeNull();
        result.EmergencyContactRelationship.Should().BeNull();
        result.EmergencyContactPhone.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_OtherTherapistPatient_ThrowsNotFound()
    {
        var patientId = Guid.NewGuid();
        var patient = MakePatient(id: patientId, therapistId: _otherTherapistId);

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        var act = () => _sut.UpdateAsync(patientId, new PatientUpdateDto { FullName = "x" }, _userId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    // --- Archive / Restore ---

    [Fact]
    public async Task ArchiveAsync_OwnPatient_SetsStatusArchived()
    {
        var patientId = Guid.NewGuid();
        var patient = MakePatient(id: patientId);

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        await _sut.ArchiveAsync(patientId, _userId);

        patient.Status.Should().Be("Archived");
        _patientRepoMock.Verify(x => x.Update(patient), Times.Once);
    }

    [Fact]
    public async Task RestoreAsync_ArchivedPatient_SetsStatusActive()
    {
        var patientId = Guid.NewGuid();
        var patient = MakePatient(id: patientId, status: "Archived");

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        await _sut.RestoreAsync(patientId, _userId);

        patient.Status.Should().Be("Active");
    }

    [Fact]
    public async Task ArchiveAsync_OtherTherapistPatient_ThrowsNotFound()
    {
        var patientId = Guid.NewGuid();
        var patient = MakePatient(id: patientId, therapistId: _otherTherapistId);

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        var act = () => _sut.ArchiveAsync(patientId, _userId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    // --- Delete ---

    [Fact]
    public async Task DeleteAsync_OwnPatient_RemovesPatient()
    {
        var patientId = Guid.NewGuid();
        var patient = MakePatient(id: patientId);

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        await _sut.DeleteAsync(patientId, _userId);

        _patientRepoMock.Verify(x => x.Remove(patient), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_OtherTherapistPatient_ThrowsNotFound()
    {
        var patientId = Guid.NewGuid();
        var patient = MakePatient(id: patientId, therapistId: _otherTherapistId);

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        var act = () => _sut.DeleteAsync(patientId, _userId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
