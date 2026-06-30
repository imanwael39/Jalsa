using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Session;

namespace Jalsa.Application.Interfaces.Repositories;

public interface ISessionRepository : IGenericRepository<Session>
{
    Task<Session?> GetByIdWithNoteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Session>> GetByPatientIdWithNotesAsync(Guid patientId, CancellationToken cancellationToken = default);
}
