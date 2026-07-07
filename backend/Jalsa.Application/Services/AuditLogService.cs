using System.Linq.Expressions;
using System.Text.Json;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Audit;

namespace Jalsa.Application.Services;

public class AuditLogService : IAuditLogService
{
    private static readonly Expression<Func<AuditLog, AuditLogViewDto>> ToDto = a => new AuditLogViewDto
    {
        Id = a.Id,
        UserId = a.UserId,
        UserEmail = a.User != null ? a.User.Email : null,
        EntityName = a.EntityName,
        EntityId = a.EntityId,
        Action = a.Action,
        OldValues = a.OldValues,
        NewValues = a.NewValues,
        IpAddress = a.IpAddress,
        UserAgent = a.UserAgent,
        OccurredAt = a.OccurredAt
    };

    private readonly IUnitOfWork _unitOfWork;

    public AuditLogService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task LogAsync(
        Guid? userId,
        string entityName,
        string? entityId,
        string action,
        object? oldValues = null,
        object? newValues = null,
        string? ipAddress = null,
        string? userAgent = null)
    {
        await _unitOfWork.Repository<AuditLog>().AddAsync(new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            EntityName = entityName,
            EntityId = entityId,
            Action = action,
            OldValues = oldValues is null ? null : JsonSerializer.Serialize(oldValues),
            NewValues = newValues is null ? null : JsonSerializer.Serialize(newValues),
            IpAddress = ipAddress,
            UserAgent = userAgent,
            OccurredAt = DateTime.UtcNow
        });

        await _unitOfWork.SaveChangesAsync();
    }

    public Task<PagedResultDto<AuditLogViewDto>> GetLogsAsync(AuditLogFilterDto filter)
    {
        var query = _unitOfWork.Repository<AuditLog>().Query();

        if (filter.UserId.HasValue)
            query = query.Where(a => a.UserId == filter.UserId.Value);

        if (!string.IsNullOrWhiteSpace(filter.EntityName))
            query = query.Where(a => a.EntityName == filter.EntityName);

        if (!string.IsNullOrWhiteSpace(filter.EntityId))
            query = query.Where(a => a.EntityId == filter.EntityId);

        if (!string.IsNullOrWhiteSpace(filter.Action))
            query = query.Where(a => a.Action == filter.Action);

        if (filter.DateFrom.HasValue)
            query = query.Where(a => a.OccurredAt >= filter.DateFrom.Value);

        if (filter.DateTo.HasValue)
            query = query.Where(a => a.OccurredAt <= filter.DateTo.Value);

        var totalCount = query.Count();

        var page = filter.Page > 0 ? filter.Page : 1;
        var pageSize = filter.PageSize > 0 ? filter.PageSize : 20;

        var items = query
            .OrderByDescending(a => a.OccurredAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ToDto)
            .ToList();

        return Task.FromResult(new PagedResultDto<AuditLogViewDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    public Task<PagedResultDto<AuditLogViewDto>> GetLogsForUserAsync(Guid userId, int page, int pageSize)
    {
        // Covers both directions: actions this user performed themselves (UserId match,
        // e.g. their own logins) and actions an admin performed on their account
        // (EntityName="User" + EntityId match, e.g. role change/deactivation) — a single
        // UserId filter would only ever catch the first, silently hiding the second.
        var userIdString = userId.ToString();
        var query = _unitOfWork.Repository<AuditLog>().Query()
            .Where(a => a.UserId == userId || (a.EntityName == "User" && a.EntityId == userIdString));

        var totalCount = query.Count();

        var effectivePage = page > 0 ? page : 1;
        var effectivePageSize = pageSize > 0 ? pageSize : 20;

        var items = query
            .OrderByDescending(a => a.OccurredAt)
            .Skip((effectivePage - 1) * effectivePageSize)
            .Take(effectivePageSize)
            .Select(ToDto)
            .ToList();

        return Task.FromResult(new PagedResultDto<AuditLogViewDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = effectivePage,
            PageSize = effectivePageSize
        });
    }
}
