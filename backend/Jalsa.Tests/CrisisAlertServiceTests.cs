using FluentAssertions;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Crisis;
using Jalsa.Domain.Models.Patient;
using Jalsa.Infrastructure.Data;
using Jalsa.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.Tests;

public class CrisisAlertServiceTests : IDisposable
{
    private readonly JalsaDbContext _context;
    private readonly CrisisAlertService _sut;

    private readonly Guid _therapistAUserId = Guid.NewGuid();
    private readonly Guid _therapistBUserId = Guid.NewGuid();
    private Guid _therapistAId;
    private Guid _patientOfAId;
    private Guid _patientOfBId;

    public CrisisAlertServiceTests()
    {
        var options = new DbContextOptionsBuilder<JalsaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new JalsaDbContext(options);
        _sut = new CrisisAlertService(new UnitOfWork(_context));

        Seed();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private void Seed()
    {
        _therapistAId = Guid.NewGuid();
        var therapistBId = Guid.NewGuid();
        _patientOfAId = Guid.NewGuid();
        _patientOfBId = Guid.NewGuid();

        _context.Therapists.AddRange(
            new Therapist { Id = _therapistAId, UserId = _therapistAUserId, FullName = "Therapist A", LicenseNumber = "LIC-A", CreatedAt = DateTime.UtcNow },
            new Therapist { Id = therapistBId, UserId = _therapistBUserId, FullName = "Therapist B", LicenseNumber = "LIC-B", CreatedAt = DateTime.UtcNow });

        _context.Patients.AddRange(
            new Patient { Id = _patientOfAId, TherapistId = _therapistAId, FullName = "Patient of A", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Patient { Id = _patientOfBId, TherapistId = therapistBId, FullName = "Patient of B", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

        _context.CrisisAlerts.AddRange(
            new CrisisAlert { Id = Guid.NewGuid(), PatientId = _patientOfAId, TherapistId = _therapistAId, Severity = "Critical", Status = "New", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new CrisisAlert { Id = Guid.NewGuid(), PatientId = _patientOfBId, TherapistId = therapistBId, Severity = "High", Status = "New", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });

        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAlertsAsync_TherapistReceivesOnlyAlertsForTheirOwnPatients()
    {
        var alerts = (await _sut.GetAlertsAsync(_therapistAUserId)).ToList();

        alerts.Should().ContainSingle();
        alerts[0].PatientId.Should().Be(_patientOfAId);
    }

    [Fact]
    public async Task GetAlertsAsync_OpenOnly_ExcludesResolvedButIncludesAcknowledged()
    {
        var alert = _context.CrisisAlerts.First(a => a.PatientId == _patientOfAId);
        var acknowledged = new CrisisAlert { Id = Guid.NewGuid(), PatientId = _patientOfAId, TherapistId = _therapistAId, Severity = "Medium", Status = "Acknowledged", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var resolved = new CrisisAlert { Id = Guid.NewGuid(), PatientId = _patientOfAId, TherapistId = _therapistAId, Severity = "Low", Status = "Resolved", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        _context.CrisisAlerts.AddRange(acknowledged, resolved);
        await _context.SaveChangesAsync();

        var openAlerts = (await _sut.GetAlertsAsync(_therapistAUserId, openOnly: true)).ToList();

        openAlerts.Select(a => a.Id).Should().Contain(new[] { alert.Id, acknowledged.Id });
        openAlerts.Select(a => a.Id).Should().NotContain(resolved.Id);
    }

    [Fact]
    public async Task AcknowledgeAsync_OwnPatientAlert_SetsStatusAcknowledged()
    {
        var alertId = _context.CrisisAlerts.First(a => a.PatientId == _patientOfAId).Id;

        var result = await _sut.AcknowledgeAsync(_therapistAUserId, alertId);

        result.Should().NotBeNull();
        result!.Status.Should().Be("Acknowledged");
    }

    [Fact]
    public async Task AcknowledgeAsync_AnotherTherapistsAlert_ReturnsNull()
    {
        var alertIdForB = _context.CrisisAlerts.First(a => a.PatientId == _patientOfBId).Id;

        var result = await _sut.AcknowledgeAsync(_therapistAUserId, alertIdForB);

        result.Should().BeNull();
    }

    [Fact]
    public async Task ResolveAsync_OwnPatientAlert_SetsStatusResolved()
    {
        var alertId = _context.CrisisAlerts.First(a => a.PatientId == _patientOfAId).Id;

        var result = await _sut.ResolveAsync(_therapistAUserId, alertId);

        result.Should().NotBeNull();
        result!.Status.Should().Be("Resolved");
    }

    [Fact]
    public async Task ResolveAsync_AnotherTherapistsAlert_ReturnsNull()
    {
        var alertIdForB = _context.CrisisAlerts.First(a => a.PatientId == _patientOfBId).Id;

        var result = await _sut.ResolveAsync(_therapistAUserId, alertIdForB);

        result.Should().BeNull();
    }
}
