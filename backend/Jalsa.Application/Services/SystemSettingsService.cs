using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.System;

namespace Jalsa.Application.Services;

public class SystemSettingsService : ISystemSettingsService
{
    private const string SiteNameKey = "SiteName";
    private const string DefaultLanguageKey = "DefaultLanguage";
    private const string PasswordMinLengthKey = "PasswordMinLength";
    private const string SessionTimeoutMinutesKey = "SessionTimeoutMinutes";
    private const string MaintenanceModeKey = "MaintenanceMode";

    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogService _auditLogService;

    public SystemSettingsService(IUnitOfWork unitOfWork, IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _auditLogService = auditLogService;
    }

    public Task<SystemSettingsDto> GetSettingsAsync()
    {
        var rows = _unitOfWork.Repository<SystemSetting>().Query().ToList();
        var map = rows.ToDictionary(r => r.Key, r => r.Value);
        var defaults = new SystemSettingsDto();

        var dto = new SystemSettingsDto
        {
            SiteName = map.GetValueOrDefault(SiteNameKey) ?? defaults.SiteName,
            DefaultLanguage = map.GetValueOrDefault(DefaultLanguageKey) ?? defaults.DefaultLanguage,
            PasswordMinLength = int.TryParse(map.GetValueOrDefault(PasswordMinLengthKey), out var minLen) ? minLen : defaults.PasswordMinLength,
            SessionTimeoutMinutes = int.TryParse(map.GetValueOrDefault(SessionTimeoutMinutesKey), out var timeout) ? timeout : defaults.SessionTimeoutMinutes,
            MaintenanceMode = bool.TryParse(map.GetValueOrDefault(MaintenanceModeKey), out var maint) && maint
        };

        return Task.FromResult(dto);
    }

    public async Task<SystemSettingsDto> UpdateSettingsAsync(
        SystemSettingsDto dto,
        Guid? actingAdminUserId = null,
        string? ipAddress = null,
        string? userAgent = null)
    {
        var oldSettings = await GetSettingsAsync();

        await UpsertAsync(SiteNameKey, dto.SiteName);
        await UpsertAsync(DefaultLanguageKey, dto.DefaultLanguage);
        await UpsertAsync(PasswordMinLengthKey, dto.PasswordMinLength.ToString());
        await UpsertAsync(SessionTimeoutMinutesKey, dto.SessionTimeoutMinutes.ToString());
        await UpsertAsync(MaintenanceModeKey, dto.MaintenanceMode.ToString());

        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync(
            actingAdminUserId, "SystemSettings", null, "Settings.Update",
            oldValues: oldSettings, newValues: dto, ipAddress: ipAddress, userAgent: userAgent);

        return dto;
    }

    private async Task UpsertAsync(string key, string value)
    {
        var repo = _unitOfWork.Repository<SystemSetting>();
        var existing = await repo.FindSingleAsync(s => s.Key == key);

        if (existing is null)
        {
            await repo.AddAsync(new SystemSetting { Key = key, Value = value, UpdatedAt = DateTime.UtcNow });
        }
        else
        {
            existing.Value = value;
            existing.UpdatedAt = DateTime.UtcNow;
            repo.Update(existing);
        }
    }
}
