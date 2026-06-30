using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.DTOs.Assessment;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Assessment;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Patient;
using Moq;

namespace Jalsa.Tests;

public class AssessmentServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Assessment>> _assessmentRepoMock;
    private readonly Mock<IGenericRepository<Patient>> _patientRepoMock;
    private readonly Mock<IGenericRepository<AssessmentTemplate>> _templateRepoMock;
    private readonly Mock<IGenericRepository<Therapist>> _therapistRepoMock;
    private readonly AssessmentService _sut;
    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _therapistId = Guid.NewGuid();
    private readonly Guid _patientId = Guid.NewGuid();

    public AssessmentServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _assessmentRepoMock = new Mock<IGenericRepository<Assessment>>();
        _patientRepoMock = new Mock<IGenericRepository<Patient>>();
        _templateRepoMock = new Mock<IGenericRepository<AssessmentTemplate>>();
        _therapistRepoMock = new Mock<IGenericRepository<Therapist>>();

        _unitOfWorkMock.Setup(u => u.Repository<Assessment>()).Returns(_assessmentRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Patient>()).Returns(_patientRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<AssessmentTemplate>()).Returns(_templateRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Therapist>()).Returns(_therapistRepoMock.Object);

        _therapistRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Therapist { Id = _therapistId, UserId = _userId, FullName = "Test Therapist", LicenseNumber = "LIC-001" });

        _sut = new AssessmentService(_unitOfWorkMock.Object);
    }

    private Patient CreatePatient() => new()
    {
        Id = _patientId,
        TherapistId = _therapistId,
        FullName = "Test Patient",
    };

    [Fact]
    public async Task GetByPatientIdAsync_ReturnsAssessmentsOrderedByDate()
    {
        var now = DateTime.UtcNow;
        var assessments = new List<Assessment>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, TemplateId = Guid.NewGuid(), Status = "Completed", CreatedAt = now.AddDays(-2), UpdatedAt = now },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, TemplateId = Guid.NewGuid(), Status = "Completed", CreatedAt = now, UpdatedAt = now },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, TemplateId = Guid.NewGuid(), Status = "Completed", CreatedAt = now.AddDays(-1), UpdatedAt = now },
        };

        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient());
        _assessmentRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<Assessment, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assessments);

        var result = (await _sut.GetByPatientIdAsync(_patientId, _userId)).ToList();

        result.Should().HaveCount(3);
        result[0].CreatedAt.Should().Be(now);
        result[1].CreatedAt.Should().Be(now.AddDays(-1));
        result[2].CreatedAt.Should().Be(now.AddDays(-2));
    }

    [Fact]
    public async Task GetByPatientIdAsync_WrongTherapist_ThrowsUnauthorized()
    {
        var wrongUserId = Guid.NewGuid();

        _therapistRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Therapist { Id = Guid.NewGuid(), UserId = wrongUserId, FullName = "Wrong", LicenseNumber = "LIC-999" });

        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient());

        var act = () => _sut.GetByPatientIdAsync(_patientId, wrongUserId);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetByPatientIdAsync_PatientNotFound_ThrowsKeyNotFound()
    {
        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        var act = () => _sut.GetByPatientIdAsync(_patientId, _userId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task CreateAsync_ExistingTemplate_UsesExistingTemplateId()
    {
        var templateId = Guid.NewGuid();
        var dto = new AssessmentCreateDto
        {
            TemplateId = "phq-9",
            Title = "PHQ-9 Assessment",
            TotalScore = 15,
            AssessmentDate = DateOnly.FromDateTime(DateTime.UtcNow),
        };

        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient());
        _templateRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<AssessmentTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssessmentTemplate { Id = templateId, Name = "phq-9", Version = 1, IsActive = true, CreatedAt = DateTime.UtcNow });
        _assessmentRepoMock.Setup(r => r.AddAsync(It.IsAny<Assessment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.CreateAsync(_patientId, dto, _therapistId);

        result.Should().NotBeNull();
        result.TemplateId.Should().Be(templateId);
        result.Title.Should().Be("PHQ-9 Assessment");
        result.TotalScore.Should().Be(15);
        result.Status.Should().Be("Completed");
        _templateRepoMock.Verify(r => r.AddAsync(It.IsAny<AssessmentTemplate>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_NewTemplate_CreatesTemplateOnTheFly()
    {
        var dto = new AssessmentCreateDto
        {
            TemplateId = "new-scale",
            Title = "New Scale",
            TotalScore = 20,
        };

        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient());
        _templateRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<AssessmentTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AssessmentTemplate?)null);
        _templateRepoMock.Setup(r => r.AddAsync(It.IsAny<AssessmentTemplate>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _assessmentRepoMock.Setup(r => r.AddAsync(It.IsAny<Assessment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.CreateAsync(_patientId, dto, _therapistId);

        result.Should().NotBeNull();
        result.TotalScore.Should().Be(20);
        _templateRepoMock.Verify(r => r.AddAsync(It.IsAny<AssessmentTemplate>(), It.IsAny<CancellationToken>()), Times.Once);
        _assessmentRepoMock.Verify(r => r.AddAsync(It.IsAny<Assessment>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_NoDateProvided_DefaultsToToday()
    {
        var dto = new AssessmentCreateDto
        {
            TemplateId = "phq-9",
            TotalScore = 10,
        };

        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient());
        _templateRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<AssessmentTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssessmentTemplate { Id = Guid.NewGuid(), Name = "phq-9", Version = 1, IsActive = true, CreatedAt = DateTime.UtcNow });
        _assessmentRepoMock.Setup(r => r.AddAsync(It.IsAny<Assessment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.CreateAsync(_patientId, dto, _therapistId);

        result.AssessmentDate.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
    }

    [Fact]
    public async Task CreateAsync_WrongTherapist_ThrowsUnauthorized()
    {
        var dto = new AssessmentCreateDto { TemplateId = "phq-9", TotalScore = 5 };
        var wrongUserId = Guid.NewGuid();

        _therapistRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Therapist { Id = Guid.NewGuid(), UserId = wrongUserId, FullName = "Wrong", LicenseNumber = "LIC-999" });

        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient());

        var act = () => _sut.CreateAsync(_patientId, dto, wrongUserId);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task CreateAsync_WithSessionId_SetsSessionId()
    {
        var sessionId = Guid.NewGuid();
        var dto = new AssessmentCreateDto
        {
            TemplateId = "gad-7",
            SessionId = sessionId,
            TotalScore = 12,
        };

        _patientRepoMock.Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreatePatient());
        _templateRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<AssessmentTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssessmentTemplate { Id = Guid.NewGuid(), Name = "gad-7", Version = 1, IsActive = true, CreatedAt = DateTime.UtcNow });
        _assessmentRepoMock.Setup(r => r.AddAsync(It.IsAny<Assessment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.CreateAsync(_patientId, dto, _therapistId);

        result.SessionId.Should().Be(sessionId);
    }
}
