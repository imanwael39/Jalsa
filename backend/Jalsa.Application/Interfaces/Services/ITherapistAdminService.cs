using Jalsa.Application.DTOs.Admin;

namespace Jalsa.Application.Interfaces.Services;

public interface ITherapistAdminService
{
    Task<PagedResultDto<TherapistAdminViewDto>> GetTherapistsAsync(TherapistFilterDto filter);

    Task<TherapistAdminDetailDto?> GetTherapistDetailAsync(Guid therapistId);

    Task<TherapistAdminViewDto?> UpdateApprovalStatusAsync(
        Guid therapistId,
        string newStatus,
        Guid? actingAdminUserId = null,
        string? ipAddress = null,
        string? userAgent = null);
}
