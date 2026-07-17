using FluentAssertions;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Chat;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Patient;
using Jalsa.Infrastructure.Data;
using Jalsa.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.Tests;

/// <summary>
/// Proves the therapist AI assistant and patient support chat are structurally isolated:
/// distinct conversation id spaces, no cross-role visibility, and no cross-therapist
/// visibility. Uses the real TherapistAiChatService/PatientSupportChatService against an
/// EF InMemory JalsaDbContext (matching SystemHealthServiceTests' InMemory pattern) rather
/// than mocks, so the actual EF query filters are exercised end to end.
/// </summary>
public class ChatIsolationTests : IDisposable
{
    private readonly JalsaDbContext _context;
    private readonly TherapistAiChatService _therapistChat;
    private readonly PatientSupportChatService _supportChat;

    private readonly Guid _therapistAUserId = Guid.NewGuid();
    private readonly Guid _therapistBUserId = Guid.NewGuid();
    private readonly Guid _patientUserId = Guid.NewGuid();
    private Guid _therapistAId;
    private Guid _therapistBId;
    private Guid _patientId;

    public ChatIsolationTests()
    {
        var options = new DbContextOptionsBuilder<JalsaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new JalsaDbContext(options);
        var unitOfWork = new UnitOfWork(_context);
        _therapistChat = new TherapistAiChatService(unitOfWork);
        _supportChat = new PatientSupportChatService(unitOfWork);

        SeedClinicalData();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private void SeedClinicalData()
    {
        _therapistAId = Guid.NewGuid();
        _therapistBId = Guid.NewGuid();
        _patientId = Guid.NewGuid();

        _context.Therapists.Add(new Therapist
        {
            Id = _therapistAId,
            UserId = _therapistAUserId,
            FullName = "Therapist A",
            LicenseNumber = "LIC-A",
            CreatedAt = DateTime.UtcNow,
        });
        _context.Therapists.Add(new Therapist
        {
            Id = _therapistBId,
            UserId = _therapistBUserId,
            FullName = "Therapist B",
            LicenseNumber = "LIC-B",
            CreatedAt = DateTime.UtcNow,
        });
        _context.Patients.Add(new Patient
        {
            Id = _patientId,
            TherapistId = _therapistAId,
            UserId = _patientUserId,
            FullName = "Patient X",
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        });

        _context.SaveChanges();
    }

    [Fact]
    public async Task TherapistConversation_NeverAppearsInPatientSupportHistory()
    {
        var therapistConv = await _therapistChat.CreateConversationAsync(_therapistAUserId, _patientId);
        await _therapistChat.SendMessageAsync(_therapistAUserId, therapistConv.Id, "Summarize this patient's last three sessions.", "Therapist");

        var supportConv = await _supportChat.GetOrCreateConversationAsync(_patientUserId);
        await _supportChat.SendMessageAsync(_patientUserId, supportConv.Id, "I'm anxious today.", "Patient");

        var patientHistory = await _supportChat.GetHistoryAsync(_patientUserId, supportConv.Id);

        patientHistory.Should().NotBeNull();
        patientHistory!.ConversationId.Should().NotBe(therapistConv.Id);
        patientHistory.Messages.Should().OnlyContain(m => m.Content != "Summarize this patient's last three sessions.");
    }

    [Fact]
    public async Task PatientSupportMessage_NeverAppearsInTherapistConversation()
    {
        var therapistConv = await _therapistChat.CreateConversationAsync(_therapistAUserId, _patientId);
        var supportConv = await _supportChat.GetOrCreateConversationAsync(_patientUserId);
        await _supportChat.SendMessageAsync(_patientUserId, supportConv.Id, "I'm anxious today.", "Patient");

        var therapistHistory = await _therapistChat.GetHistoryAsync(_therapistAUserId, therapistConv.Id);

        therapistHistory.Should().NotBeNull();
        therapistHistory!.Messages.Should().OnlyContain(m => m.Content != "I'm anxious today.");
    }

    [Fact]
    public async Task ConversationIds_AreIsolated_PatientCannotResolveTherapistConversationViaSupportChatService()
    {
        var therapistConv = await _therapistChat.CreateConversationAsync(_therapistAUserId, _patientId);

        var result = await _supportChat.GetHistoryAsync(_patientUserId, therapistConv.Id);

        result.Should().BeNull();
    }

    [Fact]
    public async Task ConversationIds_AreIsolated_TherapistCannotResolvePatientConversationViaTherapistChatService()
    {
        var supportConv = await _supportChat.GetOrCreateConversationAsync(_patientUserId);

        var result = await _therapistChat.GetHistoryAsync(_therapistAUserId, supportConv.Id);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Therapist_CannotOpenAnotherTherapistsPatientsConversation()
    {
        var therapistAConv = await _therapistChat.CreateConversationAsync(_therapistAUserId, _patientId);

        Func<Task> act = () => _therapistChat.CreateConversationAsync(_therapistBUserId, _patientId);

        await act.Should().ThrowAsync<KeyNotFoundException>();

        var therapistBAccessingTherapistAConv = await _therapistChat.GetHistoryAsync(_therapistBUserId, therapistAConv.Id);
        therapistBAccessingTherapistAConv.Should().BeNull();
    }

    [Fact]
    public async Task DatabaseRecords_AreIsolated_SeparateTablesForEachChatType()
    {
        await _therapistChat.CreateConversationAsync(_therapistAUserId, _patientId);
        await _supportChat.GetOrCreateConversationAsync(_patientUserId);

        (await _context.TherapistAiConversations.CountAsync()).Should().Be(1);
        (await _context.PatientSupportConversations.CountAsync()).Should().Be(1);

        var therapistConvId = (await _context.TherapistAiConversations.FirstAsync()).Id;
        var supportConvId = (await _context.PatientSupportConversations.FirstAsync()).Id;
        therapistConvId.Should().NotBe(supportConvId);
    }
}
