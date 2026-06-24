using Jalsa.Application.DTOs.Session;

namespace Jalsa.Application.Interfaces.Services;

public interface ISessionService
{
    Task<SessionViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PagedResult<SessionViewDto>> GetPagedAsync(SessionSearchDto search, CancellationToken cancellationToken = default);
    Task<SessionViewDto> CreateAsync(SessionCreateDto dto, CancellationToken cancellationToken = default);
    Task<SessionViewDto?> UpdateAsync(Guid id, SessionUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SessionViewDto>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default);
}
