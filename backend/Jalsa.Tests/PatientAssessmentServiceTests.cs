using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.DTOs.PatientAssessment;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Assessment;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Crisis;
using Jalsa.Domain.Models.Notification;
using Jalsa.Domain.Models.Patient;
using Jalsa.Infrastructure.Services;
using Moq;

namespace Jalsa.Tests;

public class PatientAssessmentServiceTests
{
    private readonly Mock<IPatientRepository> _patientRepoMock;
    private readonly Mock<IAssessmentRepository> _assessmentRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<AssessmentTemplate>> _templateRepoMock;
    private readonly Mock<IGenericRepository<AssessmentQuestion>> _questionRepoMock;
    private readonly Mock<IGenericRepository<AssessmentResponse>> _responseRepoMock;
    private readonly Mock<IGenericRepository<Patient>> _patientGenericRepoMock;
    private readonly Mock<IGenericRepository<Therapist>> _therapistRepoMock;
    private readonly Mock<IGenericRepository<CrisisAlert>> _crisisAlertRepoMock;
    private readonly Mock<IGenericRepository<Notification>> _notificationRepoMock;
    private readonly PatientAssessmentService _sut;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _patientId = Guid.NewGuid();
    private readonly Guid _therapistId = Guid.NewGuid();
    private readonly Guid _templateId = Guid.NewGuid();

    public PatientAssessmentServiceTests()
    {
        _patientRepoMock = new Mock<IPatientRepository>();
        _assessmentRepoMock = new Mock<IAssessmentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _templateRepoMock = new Mock<IGenericRepository<AssessmentTemplate>>();
        _questionRepoMock = new Mock<IGenericRepository<AssessmentQuestion>>();
        _responseRepoMock = new Mock<IGenericRepository<AssessmentResponse>>();
        _patientGenericRepoMock = new Mock<IGenericRepository<Patient>>();
        _therapistRepoMock = new Mock<IGenericRepository<Therapist>>();
        _crisisAlertRepoMock = new Mock<IGenericRepository<CrisisAlert>>();
        _notificationRepoMock = new Mock<IGenericRepository<Notification>>();

        _unitOfWorkMock.Setup(u => u.Repository<AssessmentTemplate>()).Returns(_templateRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<AssessmentQuestion>()).Returns(_questionRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<AssessmentResponse>()).Returns(_responseRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Patient>()).Returns(_patientGenericRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Therapist>()).Returns(_therapistRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<CrisisAlert>()).Returns(_crisisAlertRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Notification>()).Returns(_notificationRepoMock.Object);

        _patientRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patient { Id = _patientId, UserId = _userId, TherapistId = _therapistId, FullName = "سارة أحمد" });

        _patientGenericRepoMock
            .Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patient { Id = _patientId, UserId = _userId, TherapistId = _therapistId, FullName = "سارة أحمد" });

        _therapistRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Therapist { Id = _therapistId, UserId = Guid.NewGuid(), FullName = "د. أحمد سالم", LicenseNumber = "L-1" });

        _sut = new PatientAssessmentService(_patientRepoMock.Object, _assessmentRepoMock.Object, _unitOfWorkMock.Object);
    }

    private Assessment CreateAssignedAssessment() => new()
    {
        Id = Guid.NewGuid(),
        PatientId = _patientId,
        TemplateId = _templateId,
        Status = "Assigned",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    private List<AssessmentQuestion> CreatePhq9Questions(int count = 9) =>
        Enumerable.Range(1, count)
            .Select(i => new AssessmentQuestion { Id = Guid.NewGuid(), TemplateId = _templateId, QuestionText = $"Q{i}", QuestionType = "Likert0to3", SortOrder = i })
            .ToList();

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
    public async Task GetDetailAsync_AssessmentNotFoundForPatient_ThrowsKeyNotFoundException()
    {
        _assessmentRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Assessment, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Assessment?)null);

        var act = () => _sut.GetDetailAsync(_userId, Guid.NewGuid());

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetDetailAsync_ReturnsQuestionsOrderedWithExistingAnswers()
    {
        var assessment = CreateAssignedAssessment();
        var questions = CreatePhq9Questions(3);
        var existingResponse = new AssessmentResponse { Id = Guid.NewGuid(), AssessmentId = assessment.Id, QuestionId = questions[0].Id, AnswerNumber = 2 };

        _assessmentRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Assessment, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assessment);
        _templateRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<AssessmentTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssessmentTemplate { Id = _templateId, Name = "phq-9" });
        _questionRepoMock.Setup(r => r.Query()).Returns(new AsyncQueryProvider<AssessmentQuestion>(questions.AsQueryable()));
        _responseRepoMock.Setup(r => r.Query()).Returns(new AsyncQueryProvider<AssessmentResponse>(new List<AssessmentResponse> { existingResponse }.AsQueryable()));

        var result = await _sut.GetDetailAsync(_userId, assessment.Id);

        result.Questions.Should().HaveCount(3);
        result.Questions[0].AnswerNumber.Should().Be(2);
        result.Questions[1].AnswerNumber.Should().BeNull();
    }

    [Fact]
    public async Task SaveAnswerAsync_CompletedAssessment_ThrowsInvalidOperationException()
    {
        var assessment = CreateAssignedAssessment();
        assessment.Status = "Completed";

        _assessmentRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Assessment, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assessment);

        var act = () => _sut.SaveAnswerAsync(_userId, assessment.Id, Guid.NewGuid(), new SaveAnswerRequestDto { AnswerNumber = 1 });

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task SaveAnswerAsync_NewAnswer_AddsResponse()
    {
        var assessment = CreateAssignedAssessment();
        var questionId = Guid.NewGuid();

        _assessmentRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Assessment, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assessment);
        _questionRepoMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<AssessmentQuestion, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _responseRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<AssessmentResponse, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AssessmentResponse?)null);

        await _sut.SaveAnswerAsync(_userId, assessment.Id, questionId, new SaveAnswerRequestDto { AnswerNumber = 3 });

        _responseRepoMock.Verify(r => r.AddAsync(It.Is<AssessmentResponse>(x => x.AnswerNumber == 3 && x.QuestionId == questionId), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SaveAnswerAsync_ExistingAnswer_UpdatesResponse()
    {
        var assessment = CreateAssignedAssessment();
        var questionId = Guid.NewGuid();
        var existing = new AssessmentResponse { Id = Guid.NewGuid(), AssessmentId = assessment.Id, QuestionId = questionId, AnswerNumber = 1 };

        _assessmentRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Assessment, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assessment);
        _questionRepoMock
            .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<AssessmentQuestion, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _responseRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<AssessmentResponse, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existing);

        await _sut.SaveAnswerAsync(_userId, assessment.Id, questionId, new SaveAnswerRequestDto { AnswerNumber = 3 });

        existing.AnswerNumber.Should().Be(3);
        _responseRepoMock.Verify(r => r.Update(existing), Times.Once);
        _responseRepoMock.Verify(r => r.AddAsync(It.IsAny<AssessmentResponse>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SubmitAsync_NotAllQuestionsAnswered_ThrowsInvalidOperationException()
    {
        var assessment = CreateAssignedAssessment();
        var questions = CreatePhq9Questions(9);
        var responses = questions.Take(5).Select(q => new AssessmentResponse { Id = Guid.NewGuid(), AssessmentId = assessment.Id, QuestionId = q.Id, AnswerNumber = 1 }).ToList();

        _assessmentRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Assessment, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assessment);
        _templateRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<AssessmentTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssessmentTemplate { Id = _templateId, Name = "phq-9" });
        _questionRepoMock.Setup(r => r.Query()).Returns(new AsyncQueryProvider<AssessmentQuestion>(questions.AsQueryable()));
        _responseRepoMock.Setup(r => r.Query()).Returns(new AsyncQueryProvider<AssessmentResponse>(responses.AsQueryable()));

        var act = () => _sut.SubmitAsync(_userId, assessment.Id);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task SubmitAsync_AllAnswered_ComputesTotalScoreAndMildSeverity()
    {
        var assessment = CreateAssignedAssessment();
        var questions = CreatePhq9Questions(9);
        // 8 questions answered 1 each = 8, last question (item 9, suicide screen) answered 0 => total 8 => Mild (5-9)
        var responses = questions.Select((q, i) => new AssessmentResponse
        {
            Id = Guid.NewGuid(),
            AssessmentId = assessment.Id,
            QuestionId = q.Id,
            AnswerNumber = i < 8 ? 1 : 0
        }).ToList();

        _assessmentRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Assessment, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assessment);
        _templateRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<AssessmentTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssessmentTemplate { Id = _templateId, Name = "phq-9" });
        _questionRepoMock.Setup(r => r.Query()).Returns(new AsyncQueryProvider<AssessmentQuestion>(questions.AsQueryable()));
        _responseRepoMock.Setup(r => r.Query()).Returns(new AsyncQueryProvider<AssessmentResponse>(responses.AsQueryable()));

        var result = await _sut.SubmitAsync(_userId, assessment.Id);

        result.TotalScore.Should().Be(8);
        result.Severity.Should().Be("Mild");
        result.Status.Should().Be("Completed");
        assessment.CompletedAt.Should().NotBeNull();
        _crisisAlertRepoMock.Verify(r => r.AddAsync(It.IsAny<CrisisAlert>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SubmitAsync_HighScore_ComputesSevereSeverity()
    {
        var assessment = CreateAssignedAssessment();
        var questions = CreatePhq9Questions(9);
        var responses = questions.Select(q => new AssessmentResponse { Id = Guid.NewGuid(), AssessmentId = assessment.Id, QuestionId = q.Id, AnswerNumber = 3 }).ToList();

        _assessmentRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Assessment, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assessment);
        _templateRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<AssessmentTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssessmentTemplate { Id = _templateId, Name = "phq-9" });
        _questionRepoMock.Setup(r => r.Query()).Returns(new AsyncQueryProvider<AssessmentQuestion>(questions.AsQueryable()));
        _responseRepoMock.Setup(r => r.Query()).Returns(new AsyncQueryProvider<AssessmentResponse>(responses.AsQueryable()));

        var result = await _sut.SubmitAsync(_userId, assessment.Id);

        result.TotalScore.Should().Be(27);
        result.Severity.Should().Be("Severe");
    }

    [Fact]
    public async Task SubmitAsync_LastQuestionAnsweredNonZero_CreatesCrisisAlertAndNotification()
    {
        var assessment = CreateAssignedAssessment();
        var questions = CreatePhq9Questions(9);
        var responses = questions.Select((q, i) => new AssessmentResponse
        {
            Id = Guid.NewGuid(),
            AssessmentId = assessment.Id,
            QuestionId = q.Id,
            AnswerNumber = i == 8 ? 2 : 0 // item 9 (SortOrder 9) answered non-zero
        }).ToList();

        _assessmentRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Assessment, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assessment);
        _templateRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<AssessmentTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssessmentTemplate { Id = _templateId, Name = "phq-9" });
        _questionRepoMock.Setup(r => r.Query()).Returns(new AsyncQueryProvider<AssessmentQuestion>(questions.AsQueryable()));
        _responseRepoMock.Setup(r => r.Query()).Returns(new AsyncQueryProvider<AssessmentResponse>(responses.AsQueryable()));

        await _sut.SubmitAsync(_userId, assessment.Id);

        _crisisAlertRepoMock.Verify(r => r.AddAsync(
            It.Is<CrisisAlert>(a => a.PatientId == _patientId && a.TherapistId == _therapistId && a.Severity == "High" && a.Status == "Open"),
            It.IsAny<CancellationToken>()), Times.Once);
        _notificationRepoMock.Verify(r => r.AddAsync(
            It.Is<Notification>(n => n.Type == "CrisisAlert" && n.Body!.Contains("سارة أحمد")),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_NonPhq9Template_DoesNotComputeSeverityOrCheckCrisis()
    {
        var assessment = CreateAssignedAssessment();
        var questions = new List<AssessmentQuestion>
        {
            new() { Id = Guid.NewGuid(), TemplateId = _templateId, QuestionText = "Q1", SortOrder = 1 }
        };
        var responses = new List<AssessmentResponse>
        {
            new() { Id = Guid.NewGuid(), AssessmentId = assessment.Id, QuestionId = questions[0].Id, AnswerNumber = 3 }
        };

        _assessmentRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Assessment, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assessment);
        _templateRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<AssessmentTemplate, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AssessmentTemplate { Id = _templateId, Name = "custom-scale" });
        _questionRepoMock.Setup(r => r.Query()).Returns(new AsyncQueryProvider<AssessmentQuestion>(questions.AsQueryable()));
        _responseRepoMock.Setup(r => r.Query()).Returns(new AsyncQueryProvider<AssessmentResponse>(responses.AsQueryable()));

        var result = await _sut.SubmitAsync(_userId, assessment.Id);

        result.Severity.Should().BeNull();
        _crisisAlertRepoMock.Verify(r => r.AddAsync(It.IsAny<CrisisAlert>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
