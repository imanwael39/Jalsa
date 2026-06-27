using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.DTOs.Intake;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Patient;
using Moq;

namespace Jalsa.Tests;

public class IntakeServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<IntakeForm>> _intakeRepoMock;
    private readonly Mock<IGenericRepository<Patient>> _patientRepoMock;
    private readonly Mock<IGenericRepository<Therapist>> _therapistRepoMock;
    private readonly IntakeService _sut;
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _therapistId = Guid.NewGuid();
    private readonly Guid _patientId = Guid.NewGuid();

    public IntakeServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _intakeRepoMock = new Mock<IGenericRepository<IntakeForm>>();
        _patientRepoMock = new Mock<IGenericRepository<Patient>>();
        _therapistRepoMock = new Mock<IGenericRepository<Therapist>>();

        _unitOfWorkMock.Setup(u => u.Repository<IntakeForm>()).Returns(_intakeRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Patient>()).Returns(_patientRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Therapist>()).Returns(_therapistRepoMock.Object);

        _therapistRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Therapist { Id = _therapistId, UserId = _userId, FullName = "Test Therapist", LicenseNumber = "LIC-001" });

        _sut = new IntakeService(_unitOfWorkMock.Object);
    }

    private Patient CreatePatient() => new()
    {
        Id = _patientId,
        TherapistId = _therapistId,
        FullName = "Test Patient",
    };

    private IntakeForm CreateIntakeForm() => new()
    {
        Id = Guid.NewGuid(),
        PatientId = _patientId,
        PresentingProblem = "Anxiety",
        PsychiatricHistory = "None",
        FamilyHistory = "None",
        Medications = "None",
        SocialHistory = "Stable",
        Status = "Draft",
        CreatedAt = DateTime.UtcNow,
    };

    [Fact]
    public async Task GetByPatientIdAsync_ExistingForm_ReturnsViewDto()
    {
        var patient = CreatePatient();
        var form = CreateIntakeForm();

        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);
        _intakeRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<IntakeForm, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(form);

        var result = await _sut.GetByPatientIdAsync(_patientId, _userId);

        result.Should().NotBeNull();
        result.PatientId.Should().Be(_patientId);
        result.PresentingProblem.Should().Be("Anxiety");
        result.Status.Should().Be("Draft");
    }

    [Fact]
    public async Task GetByPatientIdAsync_NoForm_ThrowsKeyNotFound()
    {
        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient());
        _intakeRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<IntakeForm, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IntakeForm?)null);

        var act = () => _sut.GetByPatientIdAsync(_patientId, _userId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetByPatientIdAsync_WrongTherapist_ThrowsUnauthorized()
    {
        var patient = CreatePatient();
        var wrongUserId = Guid.NewGuid();
        var wrongTherapistId = Guid.NewGuid();

        _therapistRepoMock.Setup(r => r.FindSingleAsync(It.Is<Expression<Func<Therapist, bool>>>(e => true), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Therapist { Id = wrongTherapistId, UserId = wrongUserId, FullName = "Wrong", LicenseNumber = "LIC-999" });

        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        var act = () => _sut.GetByPatientIdAsync(_patientId, wrongUserId);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task SaveAsync_NewForm_CreatesAndReturnsDto()
    {
        var dto = new IntakeFormSaveDto
        {
            PresentingProblem = "Depression",
            PsychiatricHistory = "Prior treatment",
            FamilyHistory = "Father had depression",
            Medications = "SSRIs",
            SocialHistory = "Married",
        };

        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient());
        _intakeRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<IntakeForm, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IntakeForm?)null);
        _intakeRepoMock.Setup(r => r.AddAsync(It.IsAny<IntakeForm>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.SaveAsync(_patientId, dto, _therapistId);

        result.Should().NotBeNull();
        result.PresentingProblem.Should().Be("Depression");
        result.Status.Should().Be("Draft");
        _intakeRepoMock.Verify(r => r.AddAsync(It.IsAny<IntakeForm>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SaveAsync_ExistingForm_UpdatesAndReturnsDto()
    {
        var existingForm = CreateIntakeForm();
        var dto = new IntakeFormSaveDto
        {
            PresentingProblem = "Updated problem",
            Medications = "New medication",
        };

        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient());
        _intakeRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<IntakeForm, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingForm);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.SaveAsync(_patientId, dto, _therapistId);

        result.PresentingProblem.Should().Be("Updated problem");
        result.Medications.Should().Be("New medication");
        _intakeRepoMock.Verify(r => r.Update(existingForm), Times.Once);
        _intakeRepoMock.Verify(r => r.AddAsync(It.IsAny<IntakeForm>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SubmitAsync_DraftForm_SetsStatusToSubmitted()
    {
        var form = CreateIntakeForm();

        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient());
        _intakeRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<IntakeForm, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(form);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.SubmitAsync(_patientId, _userId);

        result.Status.Should().Be("Submitted");
        form.SubmittedAt.Should().NotBeNull();
        _intakeRepoMock.Verify(r => r.Update(form), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_NoForm_ThrowsKeyNotFound()
    {
        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient());
        _intakeRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<IntakeForm, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IntakeForm?)null);

        var act = () => _sut.SubmitAsync(_patientId, _userId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task SaveAsync_PatientNotFound_ThrowsKeyNotFound()
    {
        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        var act = () => _sut.SaveAsync(_patientId, new IntakeFormSaveDto(), _therapistId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
