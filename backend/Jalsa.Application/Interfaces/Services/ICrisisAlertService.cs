using Jalsa.Application.DTOs.CrisisAlert;

namespace Jalsa.Application.Interfaces.Services;

public interface ICrisisAlertService
{
    Task<IEnumerable<CrisisAlertViewDto>> GetAlertsAsync(Guid userId, bool openOnly = false);
    Task<CrisisAlertViewDto?> AcknowledgeAsync(Guid userId, Guid alertId);
    Task<CrisisAlertViewDto?> ResolveAsync(Guid userId, Guid alertId);
}
