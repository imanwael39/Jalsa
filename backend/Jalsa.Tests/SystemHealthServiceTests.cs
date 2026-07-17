using FluentAssertions;
using Jalsa.API.Services.Implementations;
using Jalsa.Domain.Models.Ai;
using Jalsa.Domain.Models.Chat;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.Tests;

/// <summary>
/// Only the AI-usage-count portion is reliably testable against EF InMemory — DB size
/// (raw SQL against sys.master_files) and Hangfire's JobStorage both require a real SQL
/// Server / configured storage, so the service wraps them in try/catch and degrades to
/// null, which is exactly what these tests confirm happens under InMemory.
/// </summary>
public class SystemHealthServiceTests : IDisposable
{
    private readonly JalsaDbContext _context;
    private readonly SystemHealthService _sut;

    public SystemHealthServiceTests()
    {
        var options = new DbContextOptionsBuilder<JalsaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new JalsaDbContext(options);
        _sut = new SystemHealthService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetSystemHealthAsync_DbSizeAndHangfireUnavailable_DegradeToNullInsteadOfThrowing()
    {
        var result = await _sut.GetSystemHealthAsync();

        result.DatabaseSizeMb.Should().BeNull();
        result.HangfireEnqueued.Should().BeNull();
        result.HangfireProcessing.Should().BeNull();
        result.HangfireSucceeded.Should().BeNull();
        result.HangfireFailed.Should().BeNull();
    }

    [Fact]
    public async Task GetSystemHealthAsync_ReportsProcessMemoryAndGcCounters()
    {
        var result = await _sut.GetSystemHealthAsync();

        result.ProcessMemoryMb.Should().BeGreaterThan(0);
        result.Gen0Collections.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task GetSystemHealthAsync_CountsAiChatAndReportCallsWithinTimeWindows()
    {
        var now = DateTime.UtcNow;
        _context.TherapistAiChatLogs.AddRange(
            new TherapistAiChatLog { Id = Guid.NewGuid(), ConversationId = Guid.NewGuid(), PatientId = Guid.NewGuid(), TokensUsed = 100, CreatedAt = now },
            new TherapistAiChatLog { Id = Guid.NewGuid(), ConversationId = Guid.NewGuid(), PatientId = Guid.NewGuid(), TokensUsed = 50, CreatedAt = now.AddDays(-1) }
        );
        _context.PatientSupportAiChatLogs.Add(
            new PatientSupportAiChatLog { Id = Guid.NewGuid(), ConversationId = Guid.NewGuid(), PatientId = Guid.NewGuid(), TokensUsed = 25, CreatedAt = now.AddMonths(-2) }
        );
        _context.AiReportGenerationLogs.Add(new AiReportGenerationLog
        {
            Id = Guid.NewGuid(),
            PatientId = Guid.NewGuid(),
            GeneratedByTherapistId = Guid.NewGuid(),
            TokensUsed = 200,
            CreatedAt = now
        });
        await _context.SaveChangesAsync();

        var result = await _sut.GetSystemHealthAsync();

        result.AiChatCallsToday.Should().Be(1);
        result.AiChatCallsThisMonth.Should().Be(2);
        result.AiReportCallsToday.Should().Be(1);
        result.AiReportCallsThisMonth.Should().Be(1);
        result.AiTokensUsedThisMonth.Should().Be(350);
    }
}
