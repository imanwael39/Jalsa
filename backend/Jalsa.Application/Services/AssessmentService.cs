using Jalsa.Application.DTOs.Assessment;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Assessment;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Patient;

namespace Jalsa.Application.Services;

public class AssessmentService : IAssessmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public AssessmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AssessmentViewDto>> GetByPatientIdAsync(Guid patientId, Guid therapistId, CancellationToken ct = default)
    {
        await EnsurePatientBelongsToTherapist(patientId, therapistId, ct);

        var repo = _unitOfWork.Repository<Assessment>();
        var assessments = await repo.FindAsync(a => a.PatientId == patientId, ct);

        return assessments
            .OrderByDescending(a => a.CreatedAt)
            .Select(MapToViewDto);
    }

    public async Task<AssessmentViewDto> CreateAsync(Guid patientId, AssessmentCreateDto dto, Guid therapistId, CancellationToken ct = default)
    {
        await EnsurePatientBelongsToTherapist(patientId, therapistId, ct);

        var templateId = await ResolveTemplateIdAsync(dto.TemplateId, ct);

        var assessment = new Assessment
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            SessionId = dto.SessionId,
            TemplateId = templateId,
            Title = dto.Title,
            AssessmentDate = dto.AssessmentDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            TotalScore = dto.TotalScore,
            Status = "Completed",
            CompletedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        var repo = _unitOfWork.Repository<Assessment>();
        await repo.AddAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToViewDto(assessment);
    }

    public async Task<AssessmentViewDto> AssignAsync(Guid patientId, AssessmentAssignDto dto, Guid therapistId, CancellationToken ct = default)
    {
        await EnsurePatientBelongsToTherapist(patientId, therapistId, ct);

        var templateId = await ResolveTemplateIdAsync(dto.TemplateId, ct);

        var assessment = new Assessment
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            SessionId = dto.SessionId,
            TemplateId = templateId,
            Title = dto.Title,
            Status = "Assigned",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        var repo = _unitOfWork.Repository<Assessment>();
        await repo.AddAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return MapToViewDto(assessment);
    }

    private async Task<Guid> ResolveTemplateIdAsync(string templateKey, CancellationToken ct)
    {
        var templateRepo = _unitOfWork.Repository<AssessmentTemplate>();
        var existing = await templateRepo.FindSingleAsync(t => t.Name == templateKey, ct);

        if (existing != null)
            return existing.Id;

        var template = new AssessmentTemplate
        {
            Id = Guid.NewGuid(),
            Name = templateKey,
            Version = 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
        };

        await templateRepo.AddAsync(template, ct);
        return template.Id;
    }

    private async Task EnsurePatientBelongsToTherapist(Guid patientId, Guid userId, CancellationToken ct)
    {
        var therapist = await _unitOfWork.Repository<Therapist>()
            .FindSingleAsync(t => t.UserId == userId)
            ?? throw new UnauthorizedAccessException("Therapist profile not found.");

        var patientRepo = _unitOfWork.Repository<Patient>();
        var patient = await patientRepo.GetByIdAsync(patientId, ct)
            ?? throw new KeyNotFoundException($"Patient with ID {patientId} not found.");

        if (patient.TherapistId != therapist.Id)
            throw new UnauthorizedAccessException("You do not have access to this patient's data.");
    }

    private static AssessmentViewDto MapToViewDto(Assessment a) => new()
    {
        Id = a.Id,
        PatientId = a.PatientId,
        SessionId = a.SessionId,
        TemplateId = a.TemplateId,
        Title = a.Title,
        AssessmentDate = a.AssessmentDate,
        TotalScore = a.TotalScore,
        Severity = a.Severity,
        Status = a.Status,
        CompletedAt = a.CompletedAt,
        CreatedAt = a.CreatedAt,
        UpdatedAt = a.UpdatedAt,
    };
}
