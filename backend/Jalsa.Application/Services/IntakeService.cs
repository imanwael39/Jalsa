using Jalsa.Application.DTOs.Intake;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Patient;

namespace Jalsa.Application.Services;

public class IntakeService : IIntakeService
{
    private readonly IUnitOfWork _unitOfWork;

    public IntakeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IntakeFormViewDto> GetByPatientIdAsync(Guid patientId, Guid therapistId, CancellationToken ct = default)
    {
        await EnsurePatientBelongsToTherapist(patientId, therapistId, ct);

        var repo = _unitOfWork.Repository<IntakeForm>();
        var form = await repo.FindSingleAsync(f => f.PatientId == patientId, ct);

        if (form == null)
            throw new KeyNotFoundException($"No intake form found for patient {patientId}.");

        return MapToViewDto(form);
    }

    public async Task<IntakeFormViewDto> SaveAsync(Guid patientId, IntakeFormSaveDto dto, Guid therapistId, CancellationToken ct = default)
    {
        await EnsurePatientBelongsToTherapist(patientId, therapistId, ct);

        var repo = _unitOfWork.Repository<IntakeForm>();
        var existing = await repo.FindSingleAsync(f => f.PatientId == patientId, ct);

        if (existing != null)
        {
            existing.PresentingProblem = dto.PresentingProblem;
            existing.PsychiatricHistory = dto.PsychiatricHistory;
            existing.FamilyHistory = dto.FamilyHistory;
            existing.Medications = dto.Medications;
            existing.SocialHistory = dto.SocialHistory;
            repo.Update(existing);
        }
        else
        {
            existing = new IntakeForm
            {
                Id = Guid.NewGuid(),
                PatientId = patientId,
                PresentingProblem = dto.PresentingProblem,
                PsychiatricHistory = dto.PsychiatricHistory,
                FamilyHistory = dto.FamilyHistory,
                Medications = dto.Medications,
                SocialHistory = dto.SocialHistory,
                Status = "Draft",
                CreatedAt = DateTime.UtcNow,
            };
            await repo.AddAsync(existing, ct);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        return MapToViewDto(existing);
    }

    public async Task<IntakeFormViewDto> SubmitAsync(Guid patientId, Guid therapistId, CancellationToken ct = default)
    {
        await EnsurePatientBelongsToTherapist(patientId, therapistId, ct);

        var repo = _unitOfWork.Repository<IntakeForm>();
        var form = await repo.FindSingleAsync(f => f.PatientId == patientId, ct)
            ?? throw new KeyNotFoundException($"No intake form found for patient {patientId}.");

        form.Status = "Submitted";
        form.SubmittedAt = DateTime.UtcNow;
        repo.Update(form);

        await _unitOfWork.SaveChangesAsync(ct);
        return MapToViewDto(form);
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

    private static IntakeFormViewDto MapToViewDto(IntakeForm form) => new()
    {
        Id = form.Id,
        PatientId = form.PatientId,
        PresentingProblem = form.PresentingProblem,
        PsychiatricHistory = form.PsychiatricHistory,
        FamilyHistory = form.FamilyHistory,
        Medications = form.Medications,
        SocialHistory = form.SocialHistory,
        Status = form.Status,
        CreatedAt = form.CreatedAt,
        SubmittedAt = form.SubmittedAt,
    };
}
