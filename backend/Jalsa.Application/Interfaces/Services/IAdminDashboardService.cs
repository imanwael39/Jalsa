using Jalsa.Application.DTOs.Admin;

namespace Jalsa.Application.Interfaces.Services;

public interface IAdminDashboardService
{
    Task<AdminDashboardDto> GetDashboardAsync();
}
