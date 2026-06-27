using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Domain.Models.Session;

namespace Jalsa.Application.Interfaces.Repositories;

public interface ISessionRepository : IGenericRepository<Session>
{
    Task<Session?> GetByIdWithNoteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Session>> GetByPatientIdWithNotesAsync(Guid patientId, CancellationToken cancellationToken = default);
}
