using System.Security.Claims;
using Jalsa.API.Controllers;
using Jalsa.API.Services.Interfaces;
using Jalsa.Application.DTOs.Chat;
using Jalsa.Infrastructure.Data;
using Jalsa.Domain.Models.Patient;
using Jalsa.Domain.Models.Crisis;
using Jalsa.Domain.Models.Chat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;

namespace Jalsa.Tests;

public class ChatMonitoringControllerTests
{
    private Galsa_DBDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<Galsa_DBDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new Galsa_DBDbContext(options);
    }

    private async Task SeedCrisisAlert(Galsa_DBDbContext context)
    {
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = "Test Patient",
            TherapistId = Guid.NewGuid(),
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Patients.Add(patient);

        context.CrisisAlerts.Add(new CrisisAlert
        {
            Id = Guid.NewGuid(),
            PatientId = patient.Id,
            Severity = "High",
            Status = "Open",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
    }

    // ── Engagement ─────────────────────────────────────────────

    [Fact]
    public async Task GetEngagementSummaries_ReturnsOk()
    {
        var mockService = new Mock<IChatMonitoringService>();
        mockService
            .Setup(s => s.GetEngagementSummariesAsync(null, null, null))
            .ReturnsAsync(new List<ChatEngagementSummaryDto>
            {
                new() { PatientId = Guid.NewGuid(), PatientName = "Test", TotalMessages = 5 }
            });

        using var context = CreateContext();
        var controller = new ChatMonitoringController(mockService.Object, context);

        var result = await controller.GetEngagementSummaries(null, null, null);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsAssignableFrom<List<ChatEngagementSummaryDto>>(ok.Value);
        data.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetEngagementSummaries_InvalidDateRange_ReturnsBadRequest()
    {
        var mockService = new Mock<IChatMonitoringService>();
        using var context = CreateContext();
        var controller = new ChatMonitoringController(mockService.Object, context);

        var result = await controller.GetEngagementSummaries(
            from: DateTime.UtcNow, to: DateTime.UtcNow.AddDays(-1), patientId: null);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    // ── Crisis Alerts ──────────────────────────────────────────

    [Fact]
    public async Task GetCrisisAlerts_ReturnsOk()
    {
        var mockService = new Mock<IChatMonitoringService>();
        using var context = CreateContext();
        await SeedCrisisAlert(context);

        var controller = new ChatMonitoringController(mockService.Object, context);

        var result = await controller.GetCrisisAlerts(null, null);

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value;
        value.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCrisisAlerts_FilterResolved_ExcludesOpen()
    {
        var mockService = new Mock<IChatMonitoringService>();
        using var context = CreateContext();
        await SeedCrisisAlert(context);
        var controller = new ChatMonitoringController(mockService.Object, context);

        var result = await controller.GetCrisisAlerts(resolved: true, null);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = ok.Value as IEnumerable<object>;
        data.Should().BeEmpty();
    }

    // ── Resolve ────────────────────────────────────────────────

    [Fact]
    public async Task ResolveCrisisAlert_ExistingAlert_ReturnsOk()
    {
        var mockService = new Mock<IChatMonitoringService>();
        using var context = CreateContext();
        await SeedCrisisAlert(context);
        var alertId = context.CrisisAlerts.First().Id;

        var controller = new ChatMonitoringController(mockService.Object, context);

        var result = await controller.ResolveCrisisAlert(alertId);

        Assert.IsType<OkResult>(result);

        var resolved = await context.CrisisAlerts.FindAsync(alertId);
        resolved.Should().NotBeNull();
        resolved!.Status.Should().Be("Resolved");
        resolved.ResolvedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task ResolveCrisisAlert_NonExistent_ReturnsNotFound()
    {
        var mockService = new Mock<IChatMonitoringService>();
        using var context = CreateContext();
        var controller = new ChatMonitoringController(mockService.Object, context);

        var result = await controller.ResolveCrisisAlert(Guid.NewGuid());

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ResolveCrisisAlert_AlreadyResolved_StillReturnsOk()
    {
        var mockService = new Mock<IChatMonitoringService>();
        using var context = CreateContext();
        await SeedCrisisAlert(context);
        var alertId = context.CrisisAlerts.First().Id;

        var alert = await context.CrisisAlerts.FindAsync(alertId);
        alert!.Status = "Resolved";
        alert.ResolvedAt = DateTime.UtcNow.AddDays(-1);
        await context.SaveChangesAsync();

        var controller = new ChatMonitoringController(mockService.Object, context);

        var result = await controller.ResolveCrisisAlert(alertId);

        Assert.IsType<OkResult>(result);

        var resolved = await context.CrisisAlerts.FindAsync(alertId);
        resolved!.ResolvedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task GetCrisisAlerts_FilterByPatient_ReturnsFiltered()
    {
        var mockService = new Mock<IChatMonitoringService>();
        using var context = CreateContext();

        var patient1 = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = "P1",
            TherapistId = Guid.NewGuid(),
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        var patient2 = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = "P2",
            TherapistId = Guid.NewGuid(),
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Patients.AddRange(patient1, patient2);

        context.CrisisAlerts.AddRange(
            new CrisisAlert
            {
                Id = Guid.NewGuid(),
                PatientId = patient1.Id,
                Severity = "High",
                Status = "Open",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new CrisisAlert
            {
                Id = Guid.NewGuid(),
                PatientId = patient2.Id,
                Severity = "Medium",
                Status = "Open",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        await context.SaveChangesAsync();

        var controller = new ChatMonitoringController(mockService.Object, context);

        var result = await controller.GetCrisisAlerts(null, patient1.Id);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = ok.Value as IEnumerable<object>;
        data.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetCrisisAlerts_MixedResolvedUnresolved_FiltersCorrectly()
    {
        var mockService = new Mock<IChatMonitoringService>();
        using var context = CreateContext();

        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = "Test",
            TherapistId = Guid.NewGuid(),
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        context.Patients.Add(patient);

        context.CrisisAlerts.AddRange(
            new CrisisAlert
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                Severity = "High",
                Status = "Open",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new CrisisAlert
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                Severity = "Low",
                Status = "Resolved",
                ResolvedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            });
        await context.SaveChangesAsync();

        var controller = new ChatMonitoringController(mockService.Object, context);

        var unresolvedResult = await controller.GetCrisisAlerts(resolved: false, null);
        var unresolvedOk = Assert.IsType<OkObjectResult>(unresolvedResult);
        var unresolvedData = unresolvedOk.Value as IEnumerable<object>;
        unresolvedData.Should().HaveCount(1);

        var resolvedResult = await controller.GetCrisisAlerts(resolved: true, null);
        var resolvedOk = Assert.IsType<OkObjectResult>(resolvedResult);
        var resolvedData = resolvedOk.Value as IEnumerable<object>;
        resolvedData.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetCrisisAlerts_NoAlerts_ReturnsEmpty()
    {
        var mockService = new Mock<IChatMonitoringService>();
        using var context = CreateContext();
        var controller = new ChatMonitoringController(mockService.Object, context);

        var result = await controller.GetCrisisAlerts(null, null);

        var ok = Assert.IsType<OkObjectResult>(result);
        var data = ok.Value as IEnumerable<object>;
        data.Should().BeEmpty();
    }
}
