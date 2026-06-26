using FluentAssertions;
using Jalsa.Application.DTOs.Chat;
using Jalsa.Application.DTOs.Crisis;
using Jalsa.Application.Interfaces.Services;
using Moq;
using Xunit;

namespace Jalsa.Tests.Services;

public class PatientChatServiceTests
{
    private readonly Mock<IChatService> _chatServiceMock;
    private readonly Mock<ICrisisService> _crisisServiceMock;
    private readonly Guid _patientId;

    public PatientChatServiceTests()
    {
        _chatServiceMock = new Mock<IChatService>();
        _crisisServiceMock = new Mock<ICrisisService>();
        _patientId = Guid.NewGuid();
    }

    [Fact]
    public async Task StartSession_ReturnsNewSession()
    {
        var session = new ChatSessionViewDto { Id = Guid.NewGuid(), Status = "Open" };

        _chatServiceMock
            .Setup(x => x.StartSessionAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        var result = await _chatServiceMock.Object.StartSessionAsync(_patientId);

        result.Should().NotBeNull();
        result.Status.Should().Be("Open");
    }

    [Fact]
    public async Task SendMessage_WhenOwned_ReturnsResponse()
    {
        var sessionId = Guid.NewGuid();
        var dto = new ChatSendMessageDto { SessionId = sessionId, Message = "Hello" };
        var response = new ChatSendResponseDto
        {
            PatientMessage = new ChatMessageViewDto { Id = Guid.NewGuid(), Content = "Hello", SenderType = "Patient" },
            AiMessage = new ChatMessageViewDto { Id = Guid.NewGuid(), Content = "Hi there", SenderType = "AI" }
        };

        _chatServiceMock
            .Setup(x => x.SendMessageAsync(_patientId, dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await _chatServiceMock.Object.SendMessageAsync(_patientId, dto);

        result.Should().NotBeNull();
        result!.PatientMessage.SenderType.Should().Be("Patient");
        result.AiMessage.SenderType.Should().Be("AI");
    }

    [Fact]
    public async Task SendMessage_WhenNotOwned_ReturnsNull()
    {
        var sessionId = Guid.NewGuid();
        var dto = new ChatSendMessageDto { SessionId = sessionId, Message = "Hello" };

        _chatServiceMock
            .Setup(x => x.SendMessageAsync(_patientId, dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ChatSendResponseDto?)null);

        var result = await _chatServiceMock.Object.SendMessageAsync(_patientId, dto);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetHistory_ReturnsMessages()
    {
        var messages = new List<ChatMessageViewDto>
        {
            new() { Id = Guid.NewGuid(), Content = "Hello", SenderType = "Patient", CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Content = "Hi there", SenderType = "AI", CreatedAt = DateTime.UtcNow }
        };

        _chatServiceMock
            .Setup(x => x.GetHistoryAsync(_patientId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(messages);

        var result = await _chatServiceMock.Object.GetHistoryAsync(_patientId);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task TriggerCrisisAlert_ReturnsAlert()
    {
        var alert = new CrisisAlertViewDto { Id = Guid.NewGuid(), Severity = "High", Status = "Open" };

        _crisisServiceMock
            .Setup(x => x.CreateManualAlertAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(alert);

        var result = await _crisisServiceMock.Object.CreateManualAlertAsync(_patientId);

        result.Should().NotBeNull();
        result.Severity.Should().Be("High");
    }

    [Fact]
    public async Task IsSessionOwned_WhenOwned_ReturnsTrue()
    {
        var sessionId = Guid.NewGuid();

        _chatServiceMock
            .Setup(x => x.IsSessionOwnedByPatientAsync(sessionId, _patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _chatServiceMock.Object.IsSessionOwnedByPatientAsync(sessionId, _patientId);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsSessionOwned_WhenNotOwned_ReturnsFalse()
    {
        var sessionId = Guid.NewGuid();

        _chatServiceMock
            .Setup(x => x.IsSessionOwnedByPatientAsync(sessionId, _patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _chatServiceMock.Object.IsSessionOwnedByPatientAsync(sessionId, _patientId);

        result.Should().BeFalse();
    }
}
