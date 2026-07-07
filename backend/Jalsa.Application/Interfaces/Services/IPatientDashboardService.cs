using Jalsa.Application.DTOs.PatientDashboard;

namespace Jalsa.Application.Interfaces.Services;

public interface IPatientDashboardService
{
    Task<PatientDashboardDto> GetDashboardAsync(Guid userId);
}
