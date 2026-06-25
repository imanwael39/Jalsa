using Microsoft.EntityFrameworkCore;
using Jalsa.API.Services.Interfaces;
using Jalsa.Application.DTOs.Chat;
using Jalsa.Infrastructure.Data;

namespace Jalsa.API.Services.Implementations;

public class ChatMonitoringService : IChatMonitoringService
{
    private readonly Galsa_DBDbContext _context;

    public ChatMonitoringService(Galsa_DBDbContext context)
    {
        _context = context;
    }

    public async Task<List<ChatEngagementSummaryDto>> GetEngagementSummariesAsync(
        DateTime? from = null, DateTime? to = null, Guid? patientId = null)
    {
        var query = _context.Patients
            .Include(p => p.ChatConversations)
                .ThenInclude(c => c.ChatMessages)
            .Include(p => p.CrisisAlerts)
            .AsNoTracking()
            .AsQueryable();

        if (patientId.HasValue)
            query = query.Where(p => p.Id == patientId.Value);

        var patients = await query.ToListAsync();

        var summaries = patients.Select(p =>
        {
            var messages = p.ChatConversations
                .SelectMany(c => c.ChatMessages)
                .Where(m => (!from.HasValue || m.CreatedAt >= from.Value) &&
                            (!to.HasValue || m.CreatedAt <= to.Value))
                .ToList();

            var days = messages.Any()
                ? (messages.Max(m => m.CreatedAt) - messages.Min(m => m.CreatedAt)).TotalDays + 1
                : 1;
            if (days < 1) days = 1;

            var lastMsg = messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault();

            return new ChatEngagementSummaryDto
            {
                PatientId = p.Id,
                PatientName = p.FullName,
                TotalMessages = messages.Count,
                AverageMessagesPerDay = Math.Round(messages.Count / days, 1),
                SentimentDistribution = new Dictionary<string, int>(),
                CrisisFlagsCount = p.CrisisAlerts.Count(ca =>
                    (!from.HasValue || ca.CreatedAt >= from.Value) &&
                    (!to.HasValue || ca.CreatedAt <= to.Value)),
                LastMessageTimestamp = lastMsg?.CreatedAt,
                IsActive = lastMsg != null && lastMsg.CreatedAt >= DateTime.UtcNow.AddDays(-7)
            };
        }).ToList();

        return summaries;
    }
}
