using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Domain.Models.Report;

namespace Jalsa.Application.Interfaces.Repositories;

public interface IReportRepository : IGenericRepository<ReferralReport>
{
    Task<ReferralReport?> GetByIdWithVersionsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReferralReport>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default);
}
