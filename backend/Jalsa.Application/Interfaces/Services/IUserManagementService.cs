using Jalsa.Application.DTOs.Admin;

namespace Jalsa.Application.Interfaces.Services;

public interface IUserManagementService
{
    /// <summary>Unfiltered, unpaginated list — used internally (e.g. admin dashboard's "latest users").</summary>
    Task<IEnumerable<UserAdminViewDto>> GetAllUsersAsync();

    Task<PagedResultDto<UserAdminViewDto>> GetUsersAsync(UserFilterDto filter);

    Task<UserAdminViewDto?> SetActiveAsync(Guid userId, bool isActive, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null);
    Task<UserAdminViewDto?> UnlockAsync(Guid userId, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null);
    Task<UserAdminViewDto?> ChangeRoleAsync(Guid userId, string roleName, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null);
    Task<UserAdminViewDto?> SoftDeleteAsync(Guid userId, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null);
    Task<UserAdminViewDto?> RestoreAsync(Guid userId, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null);

    Task<PagedResultDto<AuditLogViewDto>> GetUserAuditHistoryAsync(Guid userId, int page, int pageSize);
}
