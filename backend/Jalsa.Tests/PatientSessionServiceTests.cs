using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.DTOs.PatientSession;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Patient;
using Jalsa.Domain.Models.Session;
using Jalsa.Infrastructure.Services;
using Moq;

namespace Jalsa.Tests;

public class PatientSessionServiceTests
{
    private readonly Mock<IPatientRepository> _patientRepoMock;
    private readonly Mock<ISessionRepository> _sessionRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Therapist>> _therapistRepoMock;
    private readonly PatientSessionService _sut;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _patientId = Guid.NewGuid();
    private readonly Guid _therapistId = Guid.NewGuid();

    public PatientSessionServiceTests()
    {
        _patientRepoMock = new Mock<IPatientRepository>();
        _sessionRepoMock = new Mock<ISessionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _therapistRepoMock = new Mock<IGenericRepository<Therapist>>();

        _unitOfWorkMock.Setup(u => u.Repository<Therapist>()).Returns(_therapistRepoMock.Object);

        _patientRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patient { Id = _patientId, UserId = _userId, TherapistId = _therapistId, FullName = "سارة أحمد" });

        _therapistRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Therapist { Id = _therapistId, UserId = Guid.NewGuid(), FullName = "د. أحمد سالم", LicenseNumber = "L-1" });

        _sut = new PatientSessionService(_patientRepoMock.Object, _sessionRepoMock.Object, _unitOfWorkMock.Object);
    }

    private Session CreateScheduledFutureSession(Guid sessionId) => new()
    {
        Id = sessionId,
        PatientId = _patientId,
        SessionNumber = 1,
        SessionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)),
        DurationMinutes = 50,
        SessionType = "Individual",
        Status = "Scheduled",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task GetListAsync_UserWithNoPatientProfile_ThrowsUnauthorizedAccessException()
    {
        _patientRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        var act = () => _sut.GetListAsync(_userId);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetListAsync_ReturnsOnlyPatientsOwnSessionsOrderedByDateDescending()
    {
        var otherPatientId = Guid.NewGuid();
        var sessions = new List<Session>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, SessionNumber = 1, SessionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10)), Status = "Completed" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, SessionNumber = 2, SessionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)), Status = "Scheduled" },
            new() { Id = Guid.NewGuid(), PatientId = otherPatientId, SessionNumber = 1, SessionDate = DateOnly.FromDateTime(DateTime.UtcNow), Status = "Scheduled" }
        };
        _sessionRepoMock.Setup(x => x.Query()).Returns(new AsyncQueryProvider<Session>(sessions.AsQueryable()));

        var result = await _sut.GetListAsync(_userId);

        result.Should().HaveCount(2);
        result[0].SessionDate.Should().Be(sessions[1].SessionDate);
        result[1].SessionDate.Should().Be(sessions[0].SessionDate);
    }

    [Fact]
    public async Task GetDetailAsync_SessionBelongsToAnotherPatient_ThrowsKeyNotFoundException()
    {
        var sessionId = Guid.NewGuid();
        _sessionRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Session?)null);

        var act = () => _sut.GetDetailAsync(_userId, sessionId);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetDetailAsync_IncludesTherapistNameAndNoClinicalNoteContent()
    {
        var sessionId = Guid.NewGuid();
        var session = CreateScheduledFutureSession(sessionId);
        _sessionRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        var result = await _sut.GetDetailAsync(_userId, sessionId);

        result.TherapistName.Should().Be("د. أحمد سالم");
        result.CanRequestChange.Should().BeTrue();
        // PatientSessionDetailDto has no field carrying SessionNote content by design.
        typeof(PatientSessionDetailDto).GetProperty("Note").Should().BeNull();
        typeof(PatientSessionDetailDto).GetProperty("Observations").Should().BeNull();
    }

    [Fact]
    public async Task GetDetailAsync_CompletedSession_CanRequestChangeIsFalse()
    {
        var sessionId = Guid.NewGuid();
        var session = CreateScheduledFutureSession(sessionId);
        session.Status = "Completed";
        _sessionRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        var result = await _sut.GetDetailAsync(_userId, sessionId);

        result.CanRequestChange.Should().BeFalse();
    }

    [Fact]
    public async Task GetDetailAsync_PastSession_CanRequestChangeIsFalse()
    {
        var sessionId = Guid.NewGuid();
        var session = CreateScheduledFutureSession(sessionId);
        session.SessionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2));
        _sessionRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        var result = await _sut.GetDetailAsync(_userId, sessionId);

        result.CanRequestChange.Should().BeFalse();
    }

    [Fact]
    public async Task GetDetailAsync_AlreadyHasPendingRequest_CanRequestChangeIsFalse()
    {
        var sessionId = Guid.NewGuid();
        var session = CreateScheduledFutureSession(sessionId);
        session.PatientRequestStatus = "Pending";
        _sessionRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        var result = await _sut.GetDetailAsync(_userId, sessionId);

        result.CanRequestChange.Should().BeFalse();
    }

    [Fact]
    public async Task RequestRescheduleAsync_ValidSession_SetsRequestFields()
    {
        var sessionId = Guid.NewGuid();
        var session = CreateScheduledFutureSession(sessionId);
        _sessionRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        await _sut.RequestRescheduleAsync(_userId, sessionId, new SessionChangeRequestDto { Note = "أحتاج موعداً آخر" });

        session.PatientRequestType.Should().Be("Reschedule");
        session.PatientRequestStatus.Should().Be("Pending");
        session.PatientRequestNote.Should().Be("أحتاج موعداً آخر");
        _sessionRepoMock.Verify(x => x.Update(session), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RequestCancelAsync_ValidSession_SetsRequestFields()
    {
        var sessionId = Guid.NewGuid();
        var session = CreateScheduledFutureSession(sessionId);
        _sessionRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        await _sut.RequestCancelAsync(_userId, sessionId, new SessionChangeRequestDto { Note = "لن أتمكن من الحضور" });

        session.PatientRequestType.Should().Be("Cancel");
        session.PatientRequestStatus.Should().Be("Pending");
    }

    [Fact]
    public async Task RequestRescheduleAsync_SessionNotScheduled_ThrowsInvalidOperationException()
    {
        var sessionId = Guid.NewGuid();
        var session = CreateScheduledFutureSession(sessionId);
        session.Status = "Draft";
        _sessionRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        var act = () => _sut.RequestRescheduleAsync(_userId, sessionId, new SessionChangeRequestDto { Note = "test" });

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task RequestCancelAsync_AlreadyHasPendingRequest_ThrowsInvalidOperationException()
    {
        var sessionId = Guid.NewGuid();
        var session = CreateScheduledFutureSession(sessionId);
        session.PatientRequestStatus = "Pending";
        _sessionRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        var act = () => _sut.RequestCancelAsync(_userId, sessionId, new SessionChangeRequestDto { Note = "test" });

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
