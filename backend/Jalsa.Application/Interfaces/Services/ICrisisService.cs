using Jalsa.Application.DTOs.Crisis;

namespace Jalsa.Application.Interfaces.Services;

public interface ICrisisService
{
    Task<CrisisAlertViewDto> CreateManualAlertAsync(Guid patientId, CancellationToken cancellationToken = default);
}
