using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Assessment;
using Jalsa.Domain.Models.Exercise;
using Jalsa.Domain.Models.Patient;
using Jalsa.Domain.Models.Session;
using Jalsa.Infrastructure.Services;
using Moq;

namespace Jalsa.Tests;

public class PatientProgressServiceTests
{
    private readonly Mock<IPatientRepository> _patientRepoMock;
    private readonly Mock<ISessionRepository> _sessionRepoMock;
    private readonly Mock<IExerciseRepository> _exerciseRepoMock;
    private readonly Mock<IExerciseLogRepository> _exerciseLogRepoMock;
    private readonly Mock<IAssessmentRepository> _assessmentRepoMock;
    private readonly PatientProgressService _sut;
    private readonly Guid _userId;
    private readonly Guid _patientId;
    private readonly DateOnly _from;
    private readonly DateOnly _to;

    public PatientProgressServiceTests()
    {
        _patientRepoMock = new Mock<IPatientRepository>();
        _sessionRepoMock = new Mock<ISessionRepository>();
        _exerciseRepoMock = new Mock<IExerciseRepository>();
        _exerciseLogRepoMock = new Mock<IExerciseLogRepository>();
        _assessmentRepoMock = new Mock<IAssessmentRepository>();

        _userId = Guid.NewGuid();
        _patientId = Guid.NewGuid();
        _to = new DateOnly(2026, 7, 8);
        _from = _to.AddMonths(-2);

        _patientRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patient { Id = _patientId, UserId = _userId, TherapistId = Guid.NewGuid(), FullName = "سارة أحمد" });

        SetupEmptyQueryables();

        _sut = new PatientProgressService(
            _patientRepoMock.Object,
            _sessionRepoMock.Object,
            _exerciseRepoMock.Object,
            _exerciseLogRepoMock.Object,
            _assessmentRepoMock.Object);
    }

    [Fact]
    public async Task GetProgressAsync_ResolvesPatientIdFromUserId_NotFromRawClaim()
    {
        var otherPatientId = Guid.NewGuid();
        var assessments = new List<Assessment>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, TemplateId = Guid.NewGuid(), AssessmentDate = _from, TotalScore = 10m },
            new() { Id = Guid.NewGuid(), PatientId = otherPatientId, TemplateId = Guid.NewGuid(), AssessmentDate = _from, TotalScore = 99m }
        };
        _assessmentRepoMock.Setup(x => x.Query()).Returns(new AsyncQueryProvider<Assessment>(assessments.AsQueryable()));

        var result = await _sut.GetProgressAsync(_userId, _from, _to);

        result.Statistics.LatestAssessmentScore.Should().Be(10m);
    }

    [Fact]
    public async Task GetProgressAsync_UserWithNoPatientProfile_ThrowsUnauthorizedAccessException()
    {
        _patientRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        var act = () => _sut.GetProgressAsync(_userId, _from, _to);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetProgressAsync_AttendanceRate_ExcludesScheduledAndDraftSessions()
    {
        var sessions = new List<Session>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, SessionDate = _from, Status = "Completed" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, SessionDate = _from, Status = "Completed" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, SessionDate = _from, Status = "Completed" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, SessionDate = _from, Status = "Cancelled" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, SessionDate = _to, Status = "Scheduled" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, SessionDate = _to, Status = "Draft" }
        };
        _sessionRepoMock.Setup(x => x.Query()).Returns(new AsyncQueryProvider<Session>(sessions.AsQueryable()));
        _sessionRepoMock
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns((Expression<Func<Session, bool>> pred, CancellationToken _) =>
                Task.FromResult(sessions.AsQueryable().Count(pred)));

        var result = await _sut.GetProgressAsync(_userId, _from, _to);

        result.Statistics.TotalSessions.Should().Be(6);
        result.Statistics.CompletedSessions.Should().Be(3);
        result.Statistics.AttendanceRate.Should().Be(75); // 3 completed / (3 completed + 1 cancelled) — Scheduled/Draft excluded
        result.AttendanceBreakdown.Should().HaveCount(4); // breakdown itself lists every status present, unlike the rate
        result.AttendanceBreakdown.Single(b => b.Status == "Completed").Count.Should().Be(3);
    }

    [Fact]
    public async Task GetProgressAsync_ExerciseCompletionRate_CalculatesCorrectly()
    {
        var exercises = new List<Exercise>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, Status = "Complete" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, Status = "Active" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, Status = "Active" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, Status = "Active" }
        };
        _exerciseRepoMock
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Exercise, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns((Expression<Func<Exercise, bool>> pred, CancellationToken _) =>
                Task.FromResult(exercises.AsQueryable().Count(pred)));

        var result = await _sut.GetProgressAsync(_userId, _from, _to);

        result.Statistics.TotalExercises.Should().Be(4);
        result.Statistics.CompletedExercises.Should().Be(1);
        result.Statistics.ExerciseCompletionRate.Should().Be(25);
    }

    [Fact]
    public async Task GetProgressAsync_ImprovementPercentage_CalculatesFromFirstAndLatestScoreByDate()
    {
        var assessments = new List<Assessment>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, TemplateId = Guid.NewGuid(), AssessmentDate = _from, TotalScore = 20m },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, TemplateId = Guid.NewGuid(), AssessmentDate = _to, TotalScore = 10m }
        };
        _assessmentRepoMock.Setup(x => x.Query()).Returns(new AsyncQueryProvider<Assessment>(assessments.AsQueryable()));

        var result = await _sut.GetProgressAsync(_userId, _from, _to);

        result.Statistics.FirstAssessmentScore.Should().Be(20m);
        result.Statistics.LatestAssessmentScore.Should().Be(10m);
        result.Statistics.ImprovementPercentage.Should().Be(50); // (20-10)/20 * 100
    }

    [Fact]
    public async Task GetProgressAsync_MoodTrend_AveragesMoodAfterPerMonth()
    {
        var logs = new List<ExerciseLog>
        {
            new() { Id = Guid.NewGuid(), ExerciseId = Guid.NewGuid(), PatientId = _patientId, CompletionStatus = "Completed", MoodAfter = 6, LoggedAt = _from.ToDateTime(TimeOnly.MinValue) },
            new() { Id = Guid.NewGuid(), ExerciseId = Guid.NewGuid(), PatientId = _patientId, CompletionStatus = "Completed", MoodAfter = 8, LoggedAt = _from.ToDateTime(TimeOnly.MinValue) }
        };
        _exerciseLogRepoMock.Setup(x => x.Query()).Returns(new AsyncQueryProvider<ExerciseLog>(logs.AsQueryable()));

        var result = await _sut.GetProgressAsync(_userId, _from, _to);

        var bucket = result.MoodTrend.Single(t => t.Date.Year == _from.Year && t.Date.Month == _from.Month);
        bucket.Value.Should().Be(7); // average of 6 and 8
    }

    [Fact]
    public async Task GetProgressAsync_EmptyDatabase_ReturnsZeroValuesNotNull()
    {
        var result = await _sut.GetProgressAsync(_userId, _from, _to);

        result.AssessmentScoreTrend.Should().NotBeNull().And.OnlyContain(t => t.Value == 0);
        result.MoodTrend.Should().NotBeNull().And.OnlyContain(t => t.Value == 0);
        result.ExerciseCompletionTrend.Should().NotBeNull().And.OnlyContain(t => t.Value == 0);
        result.AttendanceBreakdown.Should().NotBeNull().And.BeEmpty();
        result.Statistics.TotalSessions.Should().Be(0);
        result.Statistics.AttendanceRate.Should().Be(0);
        result.Statistics.ExerciseCompletionRate.Should().Be(0);
        result.Statistics.FirstAssessmentScore.Should().BeNull();
        result.Statistics.LatestAssessmentScore.Should().BeNull();
        result.Statistics.ImprovementPercentage.Should().BeNull();
    }

    private void SetupEmptyQueryables()
    {
        _sessionRepoMock
            .Setup(x => x.Query())
            .Returns(new AsyncQueryProvider<Session>(new List<Session>().AsQueryable()));
        _sessionRepoMock
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _exerciseRepoMock
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Exercise, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _exerciseLogRepoMock
            .Setup(x => x.Query())
            .Returns(new AsyncQueryProvider<ExerciseLog>(new List<ExerciseLog>().AsQueryable()));

        _assessmentRepoMock
            .Setup(x => x.Query())
            .Returns(new AsyncQueryProvider<Assessment>(new List<Assessment>().AsQueryable()));
    }
}
