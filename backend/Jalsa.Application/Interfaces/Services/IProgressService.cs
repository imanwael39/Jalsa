using Jalsa.Application.DTOs.Dashboard;

namespace Jalsa.Application.Interfaces.Services;

public interface IProgressService
{
    Task<DashboardSummaryDto> GetDashboardAsync(Guid userId);
}
