using Jalsa.Application.DTOs.Admin;

namespace Jalsa.Application.Interfaces.Services;

public interface ISystemSettingsService
{
    Task<SystemSettingsDto> GetSettingsAsync();

    Task<SystemSettingsDto> UpdateSettingsAsync(
        SystemSettingsDto dto,
        Guid? actingAdminUserId = null,
        string? ipAddress = null,
        string? userAgent = null);
}
