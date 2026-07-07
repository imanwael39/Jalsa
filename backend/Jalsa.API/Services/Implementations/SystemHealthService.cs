using System.Diagnostics;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Jalsa.API.DTOs.Admin;
using Jalsa.API.Services.Interfaces;
using Jalsa.Infrastructure.Data;

namespace Jalsa.API.Services.Implementations;

/// <summary>
/// Reports only metrics backed by real data (process diagnostics, DB size, Hangfire's
/// own monitoring API, AI usage counts from AiChatLog/AiReportGenerationLog). Each
/// metric group is isolated behind its own try/catch so a missing SQL Server permission
/// or an unconfigured Hangfire storage (e.g. in a test host) degrades that field to null
/// instead of failing the whole dashboard request.
/// </summary>
public class SystemHealthService : ISystemHealthService
{
    private readonly JalsaDbContext _context;

    public SystemHealthService(JalsaDbContext context)
    {
        _context = context;
    }

    public async Task<SystemHealthDto> GetSystemHealthAsync()
    {
        var dto = new SystemHealthDto
        {
            ProcessMemoryMb = Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024,
            Gen0Collections = GC.CollectionCount(0),
            Gen1Collections = GC.CollectionCount(1),
            Gen2Collections = GC.CollectionCount(2)
        };

        try
        {
            dto.DatabaseSizeMb = await _context.Database
                .SqlQuery<decimal>($"SELECT CAST(SUM(size) * 8 / 1024 AS decimal(18,2)) AS [Value] FROM sys.master_files WHERE database_id = DB_ID()")
                .FirstOrDefaultAsync();
        }
        catch
        {
            dto.DatabaseSizeMb = null;
        }

        try
        {
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            dto.HangfireEnqueued = monitoringApi.EnqueuedCount("default");
            dto.HangfireProcessing = monitoringApi.ProcessingCount();
            dto.HangfireSucceeded = monitoringApi.SucceededListCount();
            dto.HangfireFailed = monitoringApi.FailedCount();
        }
        catch
        {
            // Hangfire storage not configured (e.g. test host) — leave nulls.
        }

        var now = DateTime.UtcNow;
        var todayStart = now.Date;
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        dto.AiChatCallsToday = await _context.AiChatLogs.CountAsync(l => l.CreatedAt >= todayStart);
        dto.AiChatCallsThisMonth = await _context.AiChatLogs.CountAsync(l => l.CreatedAt >= monthStart);
        dto.AiReportCallsToday = await _context.AiReportGenerationLogs.CountAsync(l => l.CreatedAt >= todayStart);
        dto.AiReportCallsThisMonth = await _context.AiReportGenerationLogs.CountAsync(l => l.CreatedAt >= monthStart);

        var chatTokens = await _context.AiChatLogs
            .Where(l => l.CreatedAt >= monthStart)
            .SumAsync(l => (long?)l.TokensUsed) ?? 0;
        var reportTokens = await _context.AiReportGenerationLogs
            .Where(l => l.CreatedAt >= monthStart)
            .SumAsync(l => (long?)l.TokensUsed) ?? 0;
        dto.AiTokensUsedThisMonth = chatTokens + reportTokens;

        return dto;
    }
}
