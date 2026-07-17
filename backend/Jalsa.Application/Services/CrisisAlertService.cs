using Jalsa.Application.DTOs.CrisisAlert;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Chat;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Crisis;
using Jalsa.Domain.Models.Patient;

namespace Jalsa.Application.Services;

public class CrisisAlertService : ICrisisAlertService
{
    private readonly IUnitOfWork _unitOfWork;

    public CrisisAlertService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CrisisAlertViewDto>> GetAlertsAsync(Guid userId, bool openOnly = false)
    {
        var therapistId = await TryResolveTherapistIdAsync(userId);
        var alerts = await _unitOfWork.Repository<CrisisAlert>()
            .FindAsync(a => a.Patient.TherapistId == therapistId && (!openOnly || a.Status != "Resolved"));

        var ordered = alerts.OrderByDescending(a => a.CreatedAt).ToList();

        var dtos = new List<CrisisAlertViewDto>();
        foreach (var alert in ordered)
            dtos.Add(await MapToDtoAsync(alert));

        return dtos;
    }

    public async Task<CrisisAlertViewDto?> AcknowledgeAsync(Guid userId, Guid alertId)
    {
        var therapistId = await TryResolveTherapistIdAsync(userId);
        var repo = _unitOfWork.Repository<CrisisAlert>();
        var alert = await repo.FindSingleAsync(a => a.Id == alertId && a.Patient.TherapistId == therapistId);

        if (alert is null)
            return null;

        alert.Status = "Acknowledged";
        alert.UpdatedAt = DateTime.UtcNow;
        repo.Update(alert);
        await _unitOfWork.SaveChangesAsync();

        return await MapToDtoAsync(alert);
    }

    public async Task<CrisisAlertViewDto?> ResolveAsync(Guid userId, Guid alertId)
    {
        var therapistId = await TryResolveTherapistIdAsync(userId);
        var repo = _unitOfWork.Repository<CrisisAlert>();
        var alert = await repo.FindSingleAsync(a => a.Id == alertId && a.Patient.TherapistId == therapistId);

        if (alert is null)
            return null;

        alert.Status = "Resolved";
        alert.UpdatedAt = DateTime.UtcNow;
        repo.Update(alert);
        await _unitOfWork.SaveChangesAsync();

        return await MapToDtoAsync(alert);
    }

    private async Task<CrisisAlertViewDto> MapToDtoAsync(CrisisAlert alert)
    {
        var patient = await _unitOfWork.Repository<Patient>().FindSingleAsync(p => p.Id == alert.PatientId);

        PatientSupportMessage? message = alert.ChatMessageId.HasValue
            ? await _unitOfWork.Repository<PatientSupportMessage>().FindSingleAsync(m => m.Id == alert.ChatMessageId.Value)
            : null;

        return new CrisisAlertViewDto
        {
            Id = alert.Id,
            PatientId = alert.PatientId,
            PatientName = patient?.FullName ?? string.Empty,
            ConversationId = alert.ConversationId,
            Severity = alert.Severity,
            Reason = alert.Reason,
            Confidence = alert.Confidence,
            Status = alert.Status,
            TriggeringMessage = message?.Content,
            CreatedAt = alert.CreatedAt,
            UpdatedAt = alert.UpdatedAt
        };
    }

    private async Task<Guid?> TryResolveTherapistIdAsync(Guid userId)
    {
        var therapist = await _unitOfWork.Repository<Therapist>().FindSingleAsync(t => t.UserId == userId);
        return therapist?.Id;
    }
}
