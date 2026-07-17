using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Hubs;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.DTOs.Notification;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Chat;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Crisis;
using Jalsa.Domain.Models.Patient;
using Jalsa.Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Jalsa.Tests;

/// <summary>
/// Exercises PatientSupportChatHub.SendMessage directly (Hub.Context/Clients/Groups are
/// publicly settable specifically to support this kind of unit test, without a live
/// SignalR connection).
/// </summary>
public class PatientSupportChatHubTests : IDisposable
{
    private readonly JalsaDbContext _context;
    private readonly Mock<IPatientSupportAiService> _chatAiMock;
    private readonly Mock<IPatientSupportMemoryService> _memoryMock;
    private readonly Mock<ICrisisDetectionService> _crisisDetectionMock;
    private readonly Mock<INotificationPushService> _pushServiceMock;
    private readonly PatientSupportChatHub _sut;

    private readonly Guid _patientUserId = Guid.NewGuid();
    private readonly Guid _therapistUserId = Guid.NewGuid();
    private Guid _patientId;
    private Guid _conversationId;

    public PatientSupportChatHubTests()
    {
        var options = new DbContextOptionsBuilder<JalsaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new JalsaDbContext(options);

        _chatAiMock = new Mock<IPatientSupportAiService>();
        _memoryMock = new Mock<IPatientSupportMemoryService>();
        _crisisDetectionMock = new Mock<ICrisisDetectionService>();
        _pushServiceMock = new Mock<INotificationPushService>();

        _sut = new PatientSupportChatHub(
            _chatAiMock.Object,
            _memoryMock.Object,
            _crisisDetectionMock.Object,
            _pushServiceMock.Object,
            _context,
            NullLogger<PatientSupportChatHub>.Instance);

        Seed();
        WireHubTestHarness();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private void Seed()
    {
        var therapistId = Guid.NewGuid();
        _patientId = Guid.NewGuid();
        _conversationId = Guid.NewGuid();

        _context.Therapists.Add(new Therapist { Id = therapistId, UserId = _therapistUserId, FullName = "Dr. Test", LicenseNumber = "LIC-1", CreatedAt = DateTime.UtcNow });
        _context.Patients.Add(new Patient { Id = _patientId, TherapistId = therapistId, UserId = _patientUserId, FullName = "Test Patient", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
        _context.PatientSupportConversations.Add(new PatientSupportConversation { Id = _conversationId, PatientId = _patientId, Status = "Open", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
        _context.SaveChanges();
    }

    private void WireHubTestHarness()
    {
        var contextMock = new Mock<HubCallerContext>();
        contextMock.Setup(c => c.ConnectionId).Returns("test-connection");
        contextMock.Setup(c => c.User).Returns(new ClaimsPrincipal(
            new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, _patientUserId.ToString()) }, "Test")));
        _sut.Context = contextMock.Object;

        var callerProxyMock = new Mock<ISingleClientProxy>();
        var groupProxyMock = new Mock<IClientProxy>();
        var clientsMock = new Mock<IHubCallerClients>();
        clientsMock.Setup(c => c.Caller).Returns(callerProxyMock.Object);
        clientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(groupProxyMock.Object);
        _sut.Clients = clientsMock.Object;

        var groupsMock = new Mock<IGroupManager>();
        _sut.Groups = groupsMock.Object;
    }

    [Fact]
    public async Task SendMessage_CrisisDetected_CreatesCrisisAlertWithSeverityAndConfidence()
    {
        _crisisDetectionMock
            .Setup(s => s.AnalyzeAsync("عاوز أموت نفسي", It.IsAny<IReadOnlyList<string>>()))
            .ReturnsAsync(new CrisisDetectionResult { IsCrisis = true, Severity = CrisisSeverity.Critical, Confidence = 0.95, Reason = "Explicit suicidal intent" });
        _chatAiMock
            .Setup(s => s.GenerateResponseStreamingAsync(_conversationId, _patientId, It.IsAny<string>(), It.IsAny<Func<string, Task>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("I'm really sorry you're feeling this overwhelmed. I'm here to listen.");

        await _sut.SendMessage(_conversationId, "عاوز أموت نفسي");

        var alert = await _context.CrisisAlerts.SingleAsync(a => a.PatientId == _patientId);
        alert.Severity.Should().Be("Critical");
        alert.Confidence.Should().Be(0.95);
        alert.ConversationId.Should().Be(_conversationId);
        alert.Status.Should().Be("New");
    }

    [Fact]
    public async Task SendMessage_CrisisDetected_NotifiesAssignedTherapist()
    {
        _crisisDetectionMock
            .Setup(s => s.AnalyzeAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>()))
            .ReturnsAsync(new CrisisDetectionResult { IsCrisis = true, Severity = CrisisSeverity.High, Confidence = 0.8 });
        _chatAiMock
            .Setup(s => s.GenerateResponseStreamingAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Func<string, Task>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("supportive reply");

        await _sut.SendMessage(_conversationId, "I want to end it.");

        var notification = await _context.Notifications.SingleAsync(n => n.Type == "CrisisAlert");
        notification.RecipientUserId.Should().Be(_therapistUserId);
        notification.Title.Should().Contain("🚨");

        _pushServiceMock.Verify(
            p => p.PushToUserAsync(_therapistUserId, It.Is<NotificationViewDto>(n => n.Type == "CrisisAlert")),
            Times.Once);
    }

    [Fact]
    public async Task SendMessage_CrisisDetected_PatientNeverSeesInternalAlertLanguage()
    {
        const string aiReply = "I'm really sorry you're feeling this overwhelmed. Can you tell me what happened today? I'm here to listen.";
        _crisisDetectionMock
            .Setup(s => s.AnalyzeAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>()))
            .ReturnsAsync(new CrisisDetectionResult { IsCrisis = true, Severity = CrisisSeverity.Critical, Confidence = 0.9 });
        _chatAiMock
            .Setup(s => s.GenerateResponseStreamingAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Func<string, Task>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(aiReply);

        await _sut.SendMessage(_conversationId, "عاوز أموت نفسي");

        var savedAiMessage = await _context.PatientSupportMessages.SingleAsync(m => m.SenderType == "AI");
        savedAiMessage.Content.Should().Be(aiReply);
        savedAiMessage.Content.Should().NotContain("alert").And.NotContain("notified").And.NotContain("therapist has been");
    }

    [Fact]
    public async Task SendMessage_NoCrisis_DoesNotCreateAlertOrNotification()
    {
        _crisisDetectionMock
            .Setup(s => s.AnalyzeAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>()))
            .ReturnsAsync(new CrisisDetectionResult { IsCrisis = false });
        _chatAiMock
            .Setup(s => s.GenerateResponseStreamingAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Func<string, Task>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("Glad to hear that!");

        await _sut.SendMessage(_conversationId, "I had a great day today");

        (await _context.CrisisAlerts.AnyAsync()).Should().BeFalse();
        (await _context.Notifications.AnyAsync(n => n.Type == "CrisisAlert")).Should().BeFalse();
    }

    [Fact]
    public async Task SendMessage_CrisisDetected_ConversationContinuesNormally_BothMessagesPersistedAndAiCalledOnce()
    {
        _crisisDetectionMock
            .Setup(s => s.AnalyzeAsync(It.IsAny<string>(), It.IsAny<IReadOnlyList<string>>()))
            .ReturnsAsync(new CrisisDetectionResult { IsCrisis = true, Severity = CrisisSeverity.Critical, Confidence = 0.9 });
        _chatAiMock
            .Setup(s => s.GenerateResponseStreamingAsync(_conversationId, _patientId, "عاوز أموت نفسي", It.IsAny<Func<string, Task>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("supportive reply");

        await _sut.SendMessage(_conversationId, "عاوز أموت نفسي");

        var messages = await _context.PatientSupportMessages.Where(m => m.ConversationId == _conversationId).ToListAsync();
        messages.Should().HaveCount(2);
        messages.Should().Contain(m => m.SenderType == "Patient" && m.Content == "عاوز أموت نفسي");
        messages.Should().Contain(m => m.SenderType == "AI" && m.Content == "supportive reply");

        _chatAiMock.Verify(
            s => s.GenerateResponseStreamingAsync(_conversationId, _patientId, "عاوز أموت نفسي", It.IsAny<Func<string, Task>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
