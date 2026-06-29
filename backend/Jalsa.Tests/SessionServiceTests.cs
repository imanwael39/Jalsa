using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.DTOs.Session;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Patient;
using Jalsa.Domain.Models.Session;
using Moq;

namespace Jalsa.Tests;

public class SessionServiceTests
{
    private readonly Mock<ISessionRepository> _sessionRepoMock;
    private readonly Mock<IPatientRepository> _patientRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Therapist>> _therapistRepoMock;
    private readonly SessionService _sut;

    private readonly Guid _therapistUserId = Guid.NewGuid();
    private readonly Guid _therapistProfileId = Guid.NewGuid();
    private readonly Guid _patientId = Guid.NewGuid();

    public SessionServiceTests()
    {
        _sessionRepoMock = new Mock<ISessionRepository>();
        _patientRepoMock = new Mock<IPatientRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _therapistRepoMock = new Mock<IGenericRepository<Therapist>>();

        _unitOfWorkMock
            .Setup(x => x.Repository<Therapist>())
            .Returns(_therapistRepoMock.Object);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _sut = new SessionService(
            _sessionRepoMock.Object,
            _patientRepoMock.Object,
            _unitOfWorkMock.Object);
    }

    private void SetupOwnershipCheck()
    {
        var therapist = new Therapist { Id = _therapistProfileId, UserId = _therapistUserId, FullName = "د. أحمد", LicenseNumber = "LIC001", CreatedAt = DateTime.UtcNow };
        var patient = new Patient { Id = _patientId, TherapistId = _therapistProfileId, FullName = "مريض تجربة", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

        _therapistRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(therapist);

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsSessionViewDto()
    {
        // Arrange
        SetupOwnershipCheck();
        _sessionRepoMock
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(2);
        _sessionRepoMock
            .Setup(x => x.AddAsync(It.IsAny<Session>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var dto = new SessionCreateDto
        {
            PatientId = _patientId,
            SessionDate = DateOnly.FromDateTime(DateTime.UtcNow),
            DurationMinutes = 50,
            SessionType = "Individual"
        };

        // Act
        var result = await _sut.CreateAsync(dto, _therapistUserId);

        // Assert
        result.Should().NotBeNull();
        result.PatientId.Should().Be(_patientId);
        result.SessionNumber.Should().Be(3);
        result.DurationMinutes.Should().Be(50);
        result.Status.Should().Be("Draft");
        _sessionRepoMock.Verify(x => x.AddAsync(It.IsAny<Session>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_PatientNotOwnedByTherapist_ThrowsUnauthorized()
    {
        // Arrange
        var therapist = new Therapist { Id = _therapistProfileId, UserId = _therapistUserId, FullName = "د. أحمد", LicenseNumber = "LIC001", CreatedAt = DateTime.UtcNow };
        var otherTherapistId = Guid.NewGuid();
        var patient = new Patient { Id = _patientId, TherapistId = otherTherapistId, FullName = "مريض", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

        _therapistRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(therapist);
        _patientRepoMock
            .Setup(x => x.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        var dto = new SessionCreateDto { PatientId = _patientId, SessionDate = DateOnly.FromDateTime(DateTime.UtcNow) };

        // Act & Assert
        var act = () => _sut.CreateAsync(dto, _therapistUserId);
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("You do not have access to this patient's data.");
    }

    [Fact]
    public async Task UpdateAsync_ExistingSession_ReturnsUpdatedDto()
    {
        // Arrange
        SetupOwnershipCheck();
        var sessionId = Guid.NewGuid();
        var existingSession = new Session
        {
            Id = sessionId,
            PatientId = _patientId,
            SessionNumber = 1,
            SessionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
            DurationMinutes = 45,
            SessionType = "Individual",
            Status = "Draft",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        _sessionRepoMock
            .Setup(x => x.GetByIdAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingSession);

        var dto = new SessionUpdateDto { Id = sessionId, DurationMinutes = 60, Status = "Completed" };

        // Act
        var result = await _sut.UpdateAsync(dto, _therapistUserId);

        // Assert
        result.Should().NotBeNull();
        result.DurationMinutes.Should().Be(60);
        result.Status.Should().Be("Completed");
        _sessionRepoMock.Verify(x => x.Update(existingSession), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_SessionNotFound_ThrowsKeyNotFound()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        _sessionRepoMock
            .Setup(x => x.GetByIdAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Session?)null);

        var dto = new SessionUpdateDto { Id = sessionId };

        // Act & Assert
        var act = () => _sut.UpdateAsync(dto, _therapistUserId);
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task DeleteAsync_ExistingSession_CallsRemoveAndSave()
    {
        // Arrange
        SetupOwnershipCheck();
        var sessionId = Guid.NewGuid();
        var session = new Session
        {
            Id = sessionId,
            PatientId = _patientId,
            SessionNumber = 1,
            SessionDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = "Draft",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _sessionRepoMock
            .Setup(x => x.GetByIdAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        // Act
        await _sut.DeleteAsync(sessionId, _therapistUserId);

        // Assert
        _sessionRepoMock.Verify(x => x.Remove(session), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByPatientIdAsync_ReturnsSessionList()
    {
        // Arrange
        SetupOwnershipCheck();
        var sessions = new List<Session>
        {
            new Session { Id = Guid.NewGuid(), PatientId = _patientId, SessionNumber = 1, SessionDate = DateOnly.FromDateTime(DateTime.UtcNow), Status = "Draft", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Session { Id = Guid.NewGuid(), PatientId = _patientId, SessionNumber = 2, SessionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7)), Status = "Completed", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        _sessionRepoMock
            .Setup(x => x.GetByPatientIdWithNotesAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(sessions);

        // Act
        var result = await _sut.GetByPatientIdAsync(_patientId, _therapistUserId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(s => s.PatientId == _patientId);
    }

    [Fact]
    public async Task SaveNoteAsync_NewNote_CreatesAndReturnsNote()
    {
        // Arrange
        SetupOwnershipCheck();
        var sessionId = Guid.NewGuid();
        var session = new Session
        {
            Id = sessionId,
            PatientId = _patientId,
            SessionNumber = 1,
            SessionDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = "Draft",
            SessionNote = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _sessionRepoMock
            .Setup(x => x.GetByIdWithNoteAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        var dto = new SessionNoteDto
        {
            Observations = "لاحظت تحسناً في المزاج",
            Interventions = "تقنيات التنفس",
            PatientResponse = "تجاوب إيجابي",
            HomeworkAssigned = "يومية المشاعر",
            NextGoals = "مواجهة الأفكار السلبية"
        };

        // Act
        var result = await _sut.SaveNoteAsync(sessionId, dto, _therapistUserId);

        // Assert
        result.Should().NotBeNull();
        result.Observations.Should().Be(dto.Observations);
        result.Interventions.Should().Be(dto.Interventions);
        result.SessionId.Should().Be(sessionId);
        _sessionRepoMock.Verify(x => x.Update(session), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SaveNoteAsync_ExistingNote_UpdatesInPlace()
    {
        // Arrange
        SetupOwnershipCheck();
        var sessionId = Guid.NewGuid();
        var noteId = Guid.NewGuid();
        var session = new Session
        {
            Id = sessionId,
            PatientId = _patientId,
            SessionNumber = 1,
            SessionDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = "Draft",
            SessionNote = new SessionNote
            {
                Id = noteId,
                SessionId = sessionId,
                Observations = "ملاحظة قديمة",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _sessionRepoMock
            .Setup(x => x.GetByIdWithNoteAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        var dto = new SessionNoteDto { Observations = "ملاحظة محدثة" };

        // Act
        var result = await _sut.SaveNoteAsync(sessionId, dto, _therapistUserId);

        // Assert
        result.Id.Should().Be(noteId);
        result.Observations.Should().Be("ملاحظة محدثة");
    }

    [Fact]
    public async Task GetNoteAsync_NoNote_ReturnsNull()
    {
        // Arrange
        SetupOwnershipCheck();
        var sessionId = Guid.NewGuid();
        var session = new Session
        {
            Id = sessionId,
            PatientId = _patientId,
            SessionNumber = 1,
            SessionDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = "Draft",
            SessionNote = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _sessionRepoMock
            .Setup(x => x.GetByIdWithNoteAsync(sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        // Act
        var result = await _sut.GetNoteAsync(sessionId, _therapistUserId);

        // Assert
        result.Should().BeNull();
    }
}