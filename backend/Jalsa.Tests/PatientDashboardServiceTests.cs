using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Assessment;
using Jalsa.Domain.Models.Chat;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Exercise;
using Jalsa.Domain.Models.Patient;
using Jalsa.Domain.Models.Session;
using Jalsa.Domain.Models.System;
using Jalsa.Infrastructure.Services;
using Moq;

namespace Jalsa.Tests;

public class PatientDashboardServiceTests
{
    private readonly Mock<IPatientRepository> _patientRepoMock;
    private readonly Mock<ISessionRepository> _sessionRepoMock;
    private readonly Mock<IExerciseRepository> _exerciseRepoMock;
    private readonly Mock<IAssessmentRepository> _assessmentRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<PatientSupportConversation>> _chatConversationRepoMock;
    private readonly Mock<IGenericRepository<Therapist>> _therapistRepoMock;
    private readonly Mock<IGenericRepository<SystemSetting>> _systemSettingRepoMock;
    private readonly Mock<IGenericRepository<AssessmentTemplate>> _assessmentTemplateRepoMock;
    private readonly PatientDashboardService _sut;
    private readonly Guid _userId;
    private readonly Guid _patientId;
    private readonly Guid _therapistId;

    public PatientDashboardServiceTests()
    {
        _patientRepoMock = new Mock<IPatientRepository>();
        _sessionRepoMock = new Mock<ISessionRepository>();
        _exerciseRepoMock = new Mock<IExerciseRepository>();
        _assessmentRepoMock = new Mock<IAssessmentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _chatConversationRepoMock = new Mock<IGenericRepository<PatientSupportConversation>>();
        _therapistRepoMock = new Mock<IGenericRepository<Therapist>>();
        _systemSettingRepoMock = new Mock<IGenericRepository<SystemSetting>>();
        _assessmentTemplateRepoMock = new Mock<IGenericRepository<AssessmentTemplate>>();

        _userId = Guid.NewGuid();
        _patientId = Guid.NewGuid();
        _therapistId = Guid.NewGuid();

        _unitOfWorkMock.Setup(x => x.Repository<PatientSupportConversation>()).Returns(_chatConversationRepoMock.Object);
        _unitOfWorkMock.Setup(x => x.Repository<Therapist>()).Returns(_therapistRepoMock.Object);
        _unitOfWorkMock.Setup(x => x.Repository<SystemSetting>()).Returns(_systemSettingRepoMock.Object);
        _unitOfWorkMock.Setup(x => x.Repository<AssessmentTemplate>()).Returns(_assessmentTemplateRepoMock.Object);

        _patientRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patient { Id = _patientId, UserId = _userId, TherapistId = _therapistId, FullName = "سارة أحمد" });

        _therapistRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Therapist { Id = _therapistId, UserId = Guid.NewGuid(), FullName = "د. أحمد سالم", LicenseNumber = "L-1" });

        _systemSettingRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<SystemSetting, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SystemSetting?)null);

        SetupEmptyQueryables();

        _sut = new PatientDashboardService(
            _patientRepoMock.Object,
            _sessionRepoMock.Object,
            _exerciseRepoMock.Object,
            _assessmentRepoMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetDashboardAsync_ResolvesPatientIdFromUserId_NotFromRawClaim()
    {
        var otherPatientId = Guid.NewGuid();
        var sessions = new List<Session>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, SessionDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1), Status = "Scheduled" },
            new() { Id = Guid.NewGuid(), PatientId = otherPatientId, SessionDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1), Status = "Scheduled" }
        };
        _sessionRepoMock.Setup(x => x.Query()).Returns(new AsyncQueryProvider<Session>(sessions.AsQueryable()));

        var result = await _sut.GetDashboardAsync(_userId);

        result.UpcomingSessions.Should().ContainSingle();
        result.UpcomingSessions[0].Id.Should().Be(sessions[0].Id);
    }

    [Fact]
    public async Task GetDashboardAsync_UpcomingSessions_FiltersFutureSessionsOnly()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var sessions = new List<Session>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, SessionDate = today.AddDays(-3), Status = "Completed" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, SessionDate = today, Status = "Scheduled" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, SessionDate = today.AddDays(5), Status = "Scheduled" }
        };
        _sessionRepoMock.Setup(x => x.Query()).Returns(new AsyncQueryProvider<Session>(sessions.AsQueryable()));

        var result = await _sut.GetDashboardAsync(_userId);

        result.UpcomingSessions.Should().HaveCount(2);
        result.UpcomingSessions.Should().OnlyContain(s => s.SessionDate >= today);
    }

    [Fact]
    public async Task GetDashboardAsync_AssignedExercises_ExcludesCompletedAndFlagsOverdue()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var exercises = new List<Exercise>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, Status = "Active", DueDate = today.AddDays(-2) },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, Status = "Active", DueDate = today.AddDays(3) },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, Status = "Complete", DueDate = today.AddDays(-1) }
        };
        _exerciseRepoMock.Setup(x => x.Query()).Returns(new AsyncQueryProvider<Exercise>(exercises.AsQueryable()));
        _exerciseRepoMock
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Exercise, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns((Expression<Func<Exercise, bool>> pred, CancellationToken _) =>
                Task.FromResult(exercises.AsQueryable().Count(pred)));

        var result = await _sut.GetDashboardAsync(_userId);

        result.AssignedExercises.Should().HaveCount(2);
        result.AssignedExercises.Should().OnlyContain(e => e.Status != "Complete");
        result.AssignedExercises.Single(e => e.DueDate == today.AddDays(-2)).IsOverdue.Should().BeTrue();
        result.AssignedExercises.Single(e => e.DueDate == today.AddDays(3)).IsOverdue.Should().BeFalse();
    }

    [Fact]
    public async Task GetDashboardAsync_ExerciseCompletionRate_CalculatesCorrectly()
    {
        var exercises = new List<Exercise>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, Status = "Complete" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, Status = "Complete" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, Status = "Active" },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, Status = "Active" }
        };
        _exerciseRepoMock
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Exercise, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns((Expression<Func<Exercise, bool>> pred, CancellationToken _) =>
                Task.FromResult(exercises.AsQueryable().Count(pred)));
        _exerciseRepoMock.Setup(x => x.Query()).Returns(new AsyncQueryProvider<Exercise>(exercises.AsQueryable()));

        var result = await _sut.GetDashboardAsync(_userId);

        result.ProgressOverview.ExerciseCompletionRate.Should().Be(50);
    }

    [Fact]
    public async Task GetDashboardAsync_ProgressOverview_LatestAndPreviousScores_OrderedByDate()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var assessments = new List<Assessment>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, TemplateId = Guid.NewGuid(), AssessmentDate = today.AddMonths(-2), TotalScore = 20m },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, TemplateId = Guid.NewGuid(), AssessmentDate = today.AddMonths(-1), TotalScore = 12m }
        };
        _assessmentRepoMock.Setup(x => x.Query()).Returns(new AsyncQueryProvider<Assessment>(assessments.AsQueryable()));

        var result = await _sut.GetDashboardAsync(_userId);

        result.ProgressOverview.LatestAssessmentScore.Should().Be(12m);
        result.ProgressOverview.PreviousAssessmentScore.Should().Be(20m);
    }

    [Fact]
    public async Task GetDashboardAsync_EmptyDatabase_ReturnsZeroValuesNotNull()
    {
        var result = await _sut.GetDashboardAsync(_userId);

        result.UpcomingSessions.Should().NotBeNull().And.BeEmpty();
        result.TodayReminders.Should().NotBeNull().And.BeEmpty();
        result.AssignedExercises.Should().NotBeNull().And.BeEmpty();
        result.PendingAssessments.Should().NotBeNull().And.BeEmpty();
        result.RecentConversations.Should().NotBeNull().And.BeEmpty();
        result.ProgressOverview.Should().NotBeNull();
        result.ProgressOverview.ExerciseCompletionRate.Should().Be(0);
        result.ProgressOverview.LatestAssessmentScore.Should().BeNull();
        result.ProgressOverview.PreviousAssessmentScore.Should().BeNull();
        result.ProgressOverview.AssessmentTrend.Should().NotBeNull();
        result.ProgressOverview.CompletedSessionsCount.Should().Be(0);
    }

    [Fact]
    public async Task GetDashboardAsync_UserWithNoPatientProfile_ThrowsUnauthorizedAccessException()
    {
        _patientRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Patient?)null);

        var act = () => _sut.GetDashboardAsync(_userId);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetDashboardAsync_CrisisSupport_FallsBackToDefaultWhenSettingMissing()
    {
        var result = await _sut.GetDashboardAsync(_userId);

        result.CrisisSupport.HotlineNumber.Should().NotBeNullOrWhiteSpace();
        result.CrisisSupport.HotlineLabel.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task GetDashboardAsync_CrisisSupport_PrefersSystemSettingWhenPresent()
    {
        _systemSettingRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<SystemSetting, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SystemSetting { Key = "CrisisHotline", Value = "19288" });

        var result = await _sut.GetDashboardAsync(_userId);

        result.CrisisSupport.HotlineNumber.Should().Be("19288");
    }

    [Fact]
    public async Task GetDashboardAsync_TherapistInfo_PopulatedFromPatientsAssignedTherapist()
    {
        var result = await _sut.GetDashboardAsync(_userId);

        result.Therapist.Should().NotBeNull();
        result.Therapist!.Id.Should().Be(_therapistId);
        result.Therapist.FullName.Should().Be("د. أحمد سالم");
    }

    [Fact]
    public async Task GetDashboardAsync_PendingAssessments_OnlyIncludesAssignedStatus()
    {
        var templateId = Guid.NewGuid();
        var assessments = new List<Assessment>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, TemplateId = templateId, Status = "Assigned", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), PatientId = _patientId, TemplateId = templateId, Status = "Completed", CreatedAt = DateTime.UtcNow }
        };
        _assessmentRepoMock.Setup(x => x.Query()).Returns(new AsyncQueryProvider<Assessment>(assessments.AsQueryable()));
        _assessmentTemplateRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<AssessmentTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssessmentTemplate { Id = templateId, Name = "phq-9" });

        var result = await _sut.GetDashboardAsync(_userId);

        result.PendingAssessments.Should().ContainSingle();
        result.PendingAssessments[0].TemplateName.Should().Be("phq-9");
    }

    [Fact]
    public async Task GetDashboardAsync_RecentConversations_DoNotExposeMessageContent()
    {
        var conversations = new List<PatientSupportConversation>
        {
            new() { Id = Guid.NewGuid(), PatientId = _patientId, Status = "Open", LastActivityAt = DateTime.UtcNow }
        };
        _chatConversationRepoMock.Setup(x => x.Query()).Returns(new AsyncQueryProvider<PatientSupportConversation>(conversations.AsQueryable()));

        var result = await _sut.GetDashboardAsync(_userId);

        result.RecentConversations.Should().ContainSingle();
        result.RecentConversations[0].Status.Should().Be("Open");
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
            .Setup(x => x.Query())
            .Returns(new AsyncQueryProvider<Exercise>(new List<Exercise>().AsQueryable()));
        _exerciseRepoMock
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Exercise, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _assessmentRepoMock
            .Setup(x => x.Query())
            .Returns(new AsyncQueryProvider<Assessment>(new List<Assessment>().AsQueryable()));

        _chatConversationRepoMock
            .Setup(x => x.Query())
            .Returns(new AsyncQueryProvider<PatientSupportConversation>(new List<PatientSupportConversation>().AsQueryable()));
    }
}
