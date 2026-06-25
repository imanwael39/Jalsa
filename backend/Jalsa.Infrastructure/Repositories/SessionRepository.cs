using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.Infrastructure.Repositories;

public class SessionRepository : GenericRepository<Domain.Models.Session.Session>, ISessionRepository
{
    private readonly Galsa_DBDbContext _dbContext;

    public SessionRepository(Galsa_DBDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<IEnumerable<Domain.Models.Session.Session>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Sessions
            .Where(s => s.PatientId == patientId)
            .OrderByDescending(s => s.SessionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domain.Models.Session.Session>> GetByDateRangeAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Sessions
            .Where(s => s.SessionDate >= from && s.SessionDate <= to)
            .OrderByDescending(s => s.SessionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Domain.Models.Session.Session?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Sessions
            .Include(s => s.Patient)
            .Include(s => s.SessionNote)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Models.Session.Session>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Sessions
            .Include(s => s.Patient)
            .Include(s => s.SessionNote)
            .OrderByDescending(s => s.SessionDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetNextSessionNumberAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        var lastSession = await _dbContext.Sessions
            .Where(s => s.PatientId == patientId)
            .OrderByDescending(s => s.SessionNumber)
            .FirstOrDefaultAsync(cancellationToken);

        return lastSession?.SessionNumber + 1 ?? 1;
    }
}
