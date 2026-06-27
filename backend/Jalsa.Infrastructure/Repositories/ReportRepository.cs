using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Report;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.Infrastructure.Repositories;

public class ReportRepository : GenericRepository<ReferralReport>, IReportRepository
{
    public ReportRepository(Galsa_DBDbContext context) : base(context) { }

    public async Task<ReferralReport?> GetByIdWithVersionsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<ReferralReport>()
            .Include(r => r.CurrentVersion)
            .Include(r => r.Versions)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<ReferralReport>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<ReferralReport>()
            .Include(r => r.CurrentVersion)
            .Where(r => r.PatientId == patientId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
