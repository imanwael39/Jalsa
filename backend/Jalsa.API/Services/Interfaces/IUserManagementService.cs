using Jalsa.API.DTOs.Admin;

namespace Jalsa.API.Services.Interfaces;

public interface IUserManagementService
{
    Task<IEnumerable<UserAdminViewDto>> GetUsersAsync();
    Task<UserAdminViewDto?> SetActiveAsync(Guid userId, bool isActive);
    Task<UserAdminViewDto?> UnlockAsync(Guid userId);
    Task<UserAdminViewDto?> ChangeRoleAsync(Guid userId, string roleName);
}
