using Jalsa.API.Services.Implementations;
using Jalsa.Infrastructure.Data;
using Jalsa.Domain.Models.Patient;
using Jalsa.Domain.Models.Chat;
using Jalsa.Domain.Models.Crisis;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace Jalsa.Tests;

public class ChatMonitoringServiceTests
{
    private Galsa_DBDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<Galsa_DBDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new Galsa_DBDbContext(options);
    }

    private static readonly DateTime BaseTime = new DateTime(2026, 6, 20, 0, 0, 0, DateTimeKind.Utc);

    private async Task SeedData(Galsa_DBDbContext context)
    {
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = "Test Patient",
            TherapistId = Guid.NewGuid(),
            Status = "Active",
            CreatedAt = BaseTime,
            UpdatedAt = BaseTime
        };
        context.Patients.Add(patient);

        var conversation = new ChatConversation
        {
            Id = Guid.NewGuid(),
            PatientId = patient.Id,
            Status = "Open",
            CreatedAt = BaseTime.AddDays(-5),
            UpdatedAt = BaseTime
        };
        context.ChatConversations.Add(conversation);

        for (int i = 0; i < 10; i++)
        {
            context.ChatMessages.Add(new ChatMessage
            {
                Id = Guid.NewGuid(),
                ConversationId = conversation.Id,
                SenderType = i % 2 == 0 ? "Patient" : "AI",
                Content = $"Message {i}",
                CreatedAt = BaseTime.AddDays(-5 + i)
            });
        }

        context.CrisisAlerts.Add(new CrisisAlert
        {
            Id = Guid.NewGuid(),
            PatientId = patient.Id,
            Severity = "High",
            Status = "Open",
            CreatedAt = BaseTime.AddDays(-1),
            UpdatedAt = BaseTime
        });

        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetEngagementSummariesAsync_ReturnsCorrectAggregates()
    {
        using var context = CreateContext();
        await SeedData(context);
        var service = new ChatMonitoringService(context);

        var summaries = await service.GetEngagementSummariesAsync();

        summaries.Should().HaveCount(1);
        var summary = summaries[0];
        summary.PatientName.Should().Be("Test Patient");
        summary.TotalMessages.Should().Be(10);
        summary.CrisisFlagsCount.Should().Be(1);
        summary.LastMessageTimestamp.Should().NotBeNull();
    }

    [Fact]
    public async Task GetEngagementSummariesAsync_WithDateFilter_FiltersCorrectly()
    {
        using var context = CreateContext();
        await SeedData(context);
        var service = new ChatMonitoringService(context);

        // messages at indexes 3 (Jun 18), 4 (Jun 19), 5 (Jun 20 00:00) fall in this range
        var from = new DateTime(2026, 6, 18, 0, 0, 0, DateTimeKind.Utc);
        var to = new DateTime(2026, 6, 20, 12, 0, 0, DateTimeKind.Utc);
        var summaries = await service.GetEngagementSummariesAsync(from: from, to: to);

        summaries.Should().HaveCount(1);
        summaries[0].TotalMessages.Should().Be(3);
    }

    [Fact]
    public async Task GetEngagementSummariesAsync_WithPatientId_FiltersCorrectly()
    {
        using var context = CreateContext();
        await SeedData(context);
        var service = new ChatMonitoringService(context);

        var patientId = context.Patients.First().Id;
        var summaries = await service.GetEngagementSummariesAsync(patientId: patientId);

        summaries.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetEngagementSummariesAsync_NoData_ReturnsEmptyList()
    {
        using var context = CreateContext();
        var service = new ChatMonitoringService(context);

        var summaries = await service.GetEngagementSummariesAsync();

        summaries.Should().BeEmpty();
    }

    [Fact]
    public async Task GetEngagementSummariesAsync_IsActive_Within7Days()
    {
        using var context = CreateContext();
        await SeedData(context);
        var service = new ChatMonitoringService(context);

        var summaries = await service.GetEngagementSummariesAsync();
        summaries[0].IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task GetEngagementSummariesAsync_IsActive_FalseWhenLastMessageOld()
    {
        using var context = CreateContext();
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = "Old Patient",
            TherapistId = Guid.NewGuid(),
            Status = "Active",
            CreatedAt = DateTime.UtcNow.AddMonths(-1),
            UpdatedAt = DateTime.UtcNow.AddMonths(-1)
        };
        context.Patients.Add(patient);

        var conversation = new ChatConversation
        {
            Id = Guid.NewGuid(),
            PatientId = patient.Id,
            Status = "Closed",
            CreatedAt = DateTime.UtcNow.AddMonths(-1),
            UpdatedAt = DateTime.UtcNow.AddMonths(-1)
        };
        context.ChatConversations.Add(conversation);

        context.ChatMessages.Add(new ChatMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = conversation.Id,
            SenderType = "Patient",
            Content = "Old message",
            CreatedAt = DateTime.UtcNow.AddDays(-10)
        });
        await context.SaveChangesAsync();

        var service = new ChatMonitoringService(context);
        var summaries = await service.GetEngagementSummariesAsync();

        summaries.Should().HaveCount(1);
        summaries[0].IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetEngagementSummariesAsync_AverageMessagesPerDay_CalculatedCorrectly()
    {
        using var context = CreateContext();
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = "Avg Patient",
            TherapistId = Guid.NewGuid(),
            Status = "Active",
            CreatedAt = BaseTime,
            UpdatedAt = BaseTime
        };
        context.Patients.Add(patient);

        var conversation = new ChatConversation
        {
            Id = Guid.NewGuid(),
            PatientId = patient.Id,
            Status = "Open",
            CreatedAt = BaseTime,
            UpdatedAt = BaseTime
        };
        context.ChatConversations.Add(conversation);

        // 5 messages over 4 days → avg = 5/5 = 1.0 (days = 4+1)
        for (int i = 0; i < 5; i++)
        {
            context.ChatMessages.Add(new ChatMessage
            {
                Id = Guid.NewGuid(),
                ConversationId = conversation.Id,
                SenderType = "Patient",
                Content = $"Msg {i}",
                CreatedAt = BaseTime.AddDays(i)
            });
        }
        await context.SaveChangesAsync();

        var service = new ChatMonitoringService(context);
        var summaries = await service.GetEngagementSummariesAsync();

        var days = 4.0 + 1; // max - min + 1
        var expectedAvg = Math.Round(5.0 / days, 1);
        summaries[0].AverageMessagesPerDay.Should().Be(expectedAvg);
    }

    [Fact]
    public async Task GetEngagementSummariesAsync_SingleMessage_AverageIsOne()
    {
        using var context = CreateContext();
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = "Single Msg",
            TherapistId = Guid.NewGuid(),
            Status = "Active",
            CreatedAt = BaseTime,
            UpdatedAt = BaseTime
        };
        context.Patients.Add(patient);

        var conversation = new ChatConversation
        {
            Id = Guid.NewGuid(),
            PatientId = patient.Id,
            Status = "Open",
            CreatedAt = BaseTime,
            UpdatedAt = BaseTime
        };
        context.ChatConversations.Add(conversation);

        context.ChatMessages.Add(new ChatMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = conversation.Id,
            SenderType = "Patient",
            Content = "Only message",
            CreatedAt = BaseTime
        });
        await context.SaveChangesAsync();

        var service = new ChatMonitoringService(context);
        var summaries = await service.GetEngagementSummariesAsync();

        summaries[0].AverageMessagesPerDay.Should().Be(1.0);
    }

    [Fact]
    public async Task GetEngagementSummariesAsync_CrisisFlagsCount_NotFilteredByMessageDate()
    {
        using var context = CreateContext();
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = "Crisis Patient",
            TherapistId = Guid.NewGuid(),
            Status = "Active",
            CreatedAt = BaseTime,
            UpdatedAt = BaseTime
        };
        context.Patients.Add(patient);

        // Crisis alert outside the message date range
        context.CrisisAlerts.Add(new CrisisAlert
        {
            Id = Guid.NewGuid(),
            PatientId = patient.Id,
            Severity = "Low",
            Status = "Open",
            CreatedAt = BaseTime.AddDays(-10),
            UpdatedAt = BaseTime.AddDays(-10)
        });
        await context.SaveChangesAsync();

        var service = new ChatMonitoringService(context);
        // Filter to only recent dates — crisis alert from -10 days should be excluded
        var from = BaseTime.AddDays(-1);
        var to = BaseTime.AddDays(1);
        var summaries = await service.GetEngagementSummariesAsync(from: from, to: to);

        summaries.Should().HaveCount(1);
        summaries[0].CrisisFlagsCount.Should().Be(0);
    }

    [Fact]
    public async Task GetEngagementSummariesAsync_MultiplePatients_ReturnsAll()
    {
        using var context = CreateContext();
        for (int i = 0; i < 3; i++)
        {
            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                FullName = $"Patient {i}",
                TherapistId = Guid.NewGuid(),
                Status = "Active",
                CreatedAt = BaseTime,
                UpdatedAt = BaseTime
            };
            context.Patients.Add(patient);
        }
        await context.SaveChangesAsync();

        var service = new ChatMonitoringService(context);
        var summaries = await service.GetEngagementSummariesAsync();

        summaries.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetEngagementSummariesAsync_MultipleConversations_AggregatesAllMessages()
    {
        using var context = CreateContext();
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = "Multi Convo",
            TherapistId = Guid.NewGuid(),
            Status = "Active",
            CreatedAt = BaseTime,
            UpdatedAt = BaseTime
        };
        context.Patients.Add(patient);

        for (int c = 0; c < 3; c++)
        {
            var conv = new ChatConversation
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                Status = "Open",
                CreatedAt = BaseTime,
                UpdatedAt = BaseTime
            };
            context.ChatConversations.Add(conv);

            context.ChatMessages.Add(new ChatMessage
            {
                Id = Guid.NewGuid(),
                ConversationId = conv.Id,
                SenderType = "Patient",
                Content = $"Convo {c} msg",
                CreatedAt = BaseTime
            });
        }
        await context.SaveChangesAsync();

        var service = new ChatMonitoringService(context);
        var summaries = await service.GetEngagementSummariesAsync();

        summaries.Should().HaveCount(1);
        summaries[0].TotalMessages.Should().Be(3);
    }

    [Fact]
    public async Task GetEngagementSummariesAsync_NoMessages_TotalIsZero()
    {
        using var context = CreateContext();
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = "Silent Patient",
            TherapistId = Guid.NewGuid(),
            Status = "Active",
            CreatedAt = BaseTime,
            UpdatedAt = BaseTime
        };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        var service = new ChatMonitoringService(context);
        var summaries = await service.GetEngagementSummariesAsync();

        summaries.Should().HaveCount(1);
        summaries[0].TotalMessages.Should().Be(0);
        summaries[0].AverageMessagesPerDay.Should().Be(0);
        summaries[0].LastMessageTimestamp.Should().BeNull();
        summaries[0].IsActive.Should().BeFalse();
    }
}
