using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Session;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.Infrastructure.Repositories;

public class SessionRepository : GenericRepository<Session>, ISessionRepository
{
    public SessionRepository(JalsaDbContext context) : base(context) { }

    public async Task<Session?> GetByIdWithNoteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Session>()
            .Include(s => s.SessionNote)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Session>> GetByPatientIdWithNotesAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Session>()
            .Include(s => s.SessionNote)
            .Where(s => s.PatientId == patientId)
            .OrderByDescending(s => s.SessionDate)
            .ThenByDescending(s => s.SessionNumber)
            .ToListAsync(cancellationToken);
    }
}
