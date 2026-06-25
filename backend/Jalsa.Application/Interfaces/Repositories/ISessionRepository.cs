using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Domain.Models.Session;

namespace Jalsa.Application.Interfaces.Repositories;

public interface ISessionRepository : IGenericRepository<Session>
{
    Task<IEnumerable<Session>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Session>> GetByDateRangeAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
    Task<Session?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Session>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<int> GetNextSessionNumberAsync(Guid patientId, CancellationToken cancellationToken = default);
}
