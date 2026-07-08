using Jalsa.Application.DTOs.PatientAssessment;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Assessment;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Crisis;
using Jalsa.Domain.Models.Notification;
using Microsoft.EntityFrameworkCore;
using PatientEntity = Jalsa.Domain.Models.Patient.Patient;

namespace Jalsa.Infrastructure.Services;

public class PatientAssessmentService : IPatientAssessmentService
{
    private const string Phq9TemplateName = "phq-9";

    private readonly IPatientRepository _patientRepository;
    private readonly IAssessmentRepository _assessmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PatientAssessmentService(
        IPatientRepository patientRepository,
        IAssessmentRepository assessmentRepository,
        IUnitOfWork unitOfWork)
    {
        _patientRepository = patientRepository;
        _assessmentRepository = assessmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PatientAssessmentSummaryDto>> GetListAsync(Guid userId)
    {
        var patientId = await ResolvePatientIdAsync(userId);

        var assessments = await _assessmentRepository.Query()
            .AsNoTracking()
            .Where(a => a.PatientId == patientId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        var result = new List<PatientAssessmentSummaryDto>();
        foreach (var a in assessments)
        {
            var template = await _unitOfWork.Repository<AssessmentTemplate>().FindSingleAsync(t => t.Id == a.TemplateId);
            var questionCount = await _unitOfWork.Repository<AssessmentQuestion>().CountAsync(q => q.TemplateId == a.TemplateId);
            var answeredCount = await _unitOfWork.Repository<AssessmentResponse>().CountAsync(r => r.AssessmentId == a.Id);

            result.Add(new PatientAssessmentSummaryDto
            {
                Id = a.Id,
                TemplateName = template?.Name ?? "",
                Title = a.Title,
                Status = a.Status,
                AssessmentDate = a.AssessmentDate,
                CompletedAt = a.CompletedAt,
                TotalScore = a.TotalScore,
                Severity = a.Severity,
                QuestionCount = questionCount,
                AnsweredCount = answeredCount
            });
        }

        return result;
    }

    public async Task<PatientAssessmentDetailDto> GetDetailAsync(Guid userId, Guid assessmentId)
    {
        var patientId = await ResolvePatientIdAsync(userId);
        var assessment = await _assessmentRepository.FindSingleAsync(a => a.Id == assessmentId && a.PatientId == patientId)
            ?? throw new KeyNotFoundException("Assessment not found.");

        var template = await _unitOfWork.Repository<AssessmentTemplate>().FindSingleAsync(t => t.Id == assessment.TemplateId);

        var questions = await _unitOfWork.Repository<AssessmentQuestion>().Query()
            .AsNoTracking()
            .Where(q => q.TemplateId == assessment.TemplateId)
            .OrderBy(q => q.SortOrder)
            .ToListAsync();

        var responses = await _unitOfWork.Repository<AssessmentResponse>().Query()
            .AsNoTracking()
            .Where(r => r.AssessmentId == assessmentId)
            .ToListAsync();

        return BuildDetailDto(assessment, template, questions, responses);
    }

    public async Task SaveAnswerAsync(Guid userId, Guid assessmentId, Guid questionId, SaveAnswerRequestDto dto)
    {
        var patientId = await ResolvePatientIdAsync(userId);
        var assessment = await _assessmentRepository.FindSingleAsync(a => a.Id == assessmentId && a.PatientId == patientId)
            ?? throw new KeyNotFoundException("Assessment not found.");

        if (assessment.Status != "Assigned")
            throw new InvalidOperationException("لا يمكن تعديل إجابات تقييم مكتمل بالفعل.");

        var questionExists = await _unitOfWork.Repository<AssessmentQuestion>()
            .AnyAsync(q => q.Id == questionId && q.TemplateId == assessment.TemplateId);
        if (!questionExists)
            throw new KeyNotFoundException("Question not found for this assessment's template.");

        var responseRepo = _unitOfWork.Repository<AssessmentResponse>();
        var existing = await responseRepo.FindSingleAsync(r => r.AssessmentId == assessmentId && r.QuestionId == questionId);

        if (existing != null)
        {
            existing.AnswerNumber = dto.AnswerNumber;
            responseRepo.Update(existing);
        }
        else
        {
            await responseRepo.AddAsync(new AssessmentResponse
            {
                Id = Guid.NewGuid(),
                AssessmentId = assessmentId,
                QuestionId = questionId,
                AnswerNumber = dto.AnswerNumber,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PatientAssessmentDetailDto> SubmitAsync(Guid userId, Guid assessmentId)
    {
        var patientId = await ResolvePatientIdAsync(userId);
        var assessment = await _assessmentRepository.FindSingleAsync(a => a.Id == assessmentId && a.PatientId == patientId)
            ?? throw new KeyNotFoundException("Assessment not found.");

        if (assessment.Status != "Assigned")
            throw new InvalidOperationException("هذا التقييم مكتمل بالفعل.");

        var template = await _unitOfWork.Repository<AssessmentTemplate>().FindSingleAsync(t => t.Id == assessment.TemplateId)
            ?? throw new KeyNotFoundException("Assessment template not found.");

        var questions = await _unitOfWork.Repository<AssessmentQuestion>().Query()
            .AsNoTracking()
            .Where(q => q.TemplateId == assessment.TemplateId)
            .ToListAsync();

        var responses = await _unitOfWork.Repository<AssessmentResponse>().Query()
            .AsNoTracking()
            .Where(r => r.AssessmentId == assessmentId)
            .ToListAsync();

        if (responses.Count < questions.Count)
            throw new InvalidOperationException("يرجى الإجابة على جميع الأسئلة قبل الإرسال.");

        var totalScore = responses.Sum(r => r.AnswerNumber ?? 0);
        var severity = ComputeSeverity(template.Name, totalScore);

        assessment.TotalScore = totalScore;
        assessment.Severity = severity;
        assessment.Status = "Completed";
        assessment.AssessmentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        assessment.CompletedAt = DateTime.UtcNow;
        assessment.UpdatedAt = DateTime.UtcNow;
        _assessmentRepository.Update(assessment);

        await CheckForCrisisIndicatorsAsync(patientId, template, questions, responses);

        await _unitOfWork.SaveChangesAsync();

        return BuildDetailDto(assessment, template, questions.OrderBy(q => q.SortOrder).ToList(), responses);
    }

    private async Task CheckForCrisisIndicatorsAsync(
        Guid patientId,
        AssessmentTemplate template,
        List<AssessmentQuestion> questions,
        List<AssessmentResponse> responses)
    {
        if (template.Name != Phq9TemplateName) return;

        var lastQuestion = questions.OrderByDescending(q => q.SortOrder).FirstOrDefault();
        if (lastQuestion is null) return;

        var lastResponse = responses.FirstOrDefault(r => r.QuestionId == lastQuestion.Id);
        if (lastResponse?.AnswerNumber is null || lastResponse.AnswerNumber <= 0) return;

        var patient = await _unitOfWork.Repository<PatientEntity>().GetByIdAsync(patientId);
        if (patient is null) return;

        var therapist = await _unitOfWork.Repository<Therapist>().FindSingleAsync(t => t.Id == patient.TherapistId);

        await _unitOfWork.Repository<CrisisAlert>().AddAsync(new CrisisAlert
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            TherapistId = therapist?.Id,
            Severity = "High",
            Status = "Open",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        if (therapist != null)
        {
            await _unitOfWork.Repository<Notification>().AddAsync(new Notification
            {
                Id = Guid.NewGuid(),
                RecipientUserId = therapist.UserId,
                Type = "CrisisAlert",
                Title = "تنبيه أزمة",
                Body = $"أشارت إجابات المريض {patient.FullName} على تقييم PHQ-9 إلى احتمال وجود أفكار إيذاء النفس. يرجى المتابعة فوراً.",
                CreatedAt = DateTime.UtcNow
            });
        }
    }

    private static string? ComputeSeverity(string templateName, decimal totalScore)
    {
        if (templateName != Phq9TemplateName) return null;

        return totalScore switch
        {
            <= 4 => "Minimal",
            <= 9 => "Mild",
            <= 14 => "Moderate",
            <= 19 => "ModeratelySevere",
            _ => "Severe"
        };
    }

    private static PatientAssessmentDetailDto BuildDetailDto(
        Assessment assessment,
        AssessmentTemplate? template,
        List<AssessmentQuestion> questions,
        List<AssessmentResponse> responses) => new()
    {
        Id = assessment.Id,
        TemplateName = template?.Name ?? "",
        Title = assessment.Title,
        Status = assessment.Status,
        AssessmentDate = assessment.AssessmentDate,
        CompletedAt = assessment.CompletedAt,
        TotalScore = assessment.TotalScore,
        Severity = assessment.Severity,
        Questions = questions.Select(q => new AssessmentQuestionDto
        {
            Id = q.Id,
            QuestionText = q.QuestionText,
            QuestionType = q.QuestionType,
            SortOrder = q.SortOrder,
            AnswerNumber = responses.FirstOrDefault(r => r.QuestionId == q.Id)?.AnswerNumber
        }).ToList()
    };

    private async Task<Guid> ResolvePatientIdAsync(Guid userId)
    {
        var patient = await _patientRepository.FindSingleAsync(p => p.UserId == userId)
            ?? throw new UnauthorizedAccessException("Patient profile not found.");

        return patient.Id;
    }
}
