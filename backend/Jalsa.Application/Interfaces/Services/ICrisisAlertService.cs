using Jalsa.Application.DTOs.CrisisAlert;

namespace Jalsa.Application.Interfaces.Services;

public interface ICrisisAlertService
{
    Task<IEnumerable<CrisisAlertViewDto>> GetAlertsAsync(Guid userId, bool openOnly = false);
    Task<CrisisAlertViewDto?> ResolveAsync(Guid userId, Guid alertId);
}
