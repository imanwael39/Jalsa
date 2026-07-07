using Jalsa.Application.DTOs.Admin;

namespace Jalsa.Application.Interfaces.Services;

public interface IAuditLogService
{
    Task LogAsync(
        Guid? userId,
        string entityName,
        string? entityId,
        string action,
        object? oldValues = null,
        object? newValues = null,
        string? ipAddress = null,
        string? userAgent = null);

    Task<PagedResultDto<AuditLogViewDto>> GetLogsAsync(AuditLogFilterDto filter);

    Task<PagedResultDto<AuditLogViewDto>> GetLogsForUserAsync(Guid userId, int page, int pageSize);
}
