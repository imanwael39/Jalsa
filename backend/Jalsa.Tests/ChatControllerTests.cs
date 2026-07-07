using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.API.Hubs;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.DTOs.Chat;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Jalsa.Tests;

public class ChatControllerTests
{
    private readonly Mock<IChatService> _chatServiceMock;
    private readonly Mock<ITherapistChatAiService> _therapistChatAiMock;
    private readonly Mock<IConversationMemoryService> _conversationMemoryMock;
    private readonly ChatController _sut;
    private readonly Guid _userId;
    private readonly Guid _patientId = Guid.NewGuid();
    private readonly Guid _conversationId = Guid.NewGuid();

    public ChatControllerTests()
    {
        _chatServiceMock = new Mock<IChatService>();
        _therapistChatAiMock = new Mock<ITherapistChatAiService>();
        _conversationMemoryMock = new Mock<IConversationMemoryService>();

        var clientProxyMock = new Mock<IClientProxy>();
        clientProxyMock
            .Setup(x => x.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), default))
            .Returns(Task.CompletedTask);
        var hubClientsMock = new Mock<IHubClients>();
        hubClientsMock.Setup(x => x.Group(It.IsAny<string>())).Returns(clientProxyMock.Object);
        var hubContextMock = new Mock<IHubContext<ChatHub>>();
        hubContextMock.Setup(x => x.Clients).Returns(hubClientsMock.Object);

        _sut = new ChatController(_chatServiceMock.Object, hubContextMock.Object, _therapistChatAiMock.Object, _conversationMemoryMock.Object);

        _userId = Guid.NewGuid();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, _userId.ToString()),
            new(ClaimTypes.Role, "Therapist")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    private ConversationViewDto MakeConversationDto(Guid? id = null) => new()
    {
        Id = id ?? _conversationId,
        PatientId = _patientId,
        PatientName = "أحمد محمد",
        Status = "Active",
        LastActivityAt = DateTime.UtcNow,
        CreatedAt = DateTime.UtcNow,
        MessageCount = 0
    };

    private ChatHistoryDto MakeHistoryDto() => new()
    {
        ConversationId = _conversationId,
        PatientId = _patientId,
        PatientName = "أحمد محمد",
        Messages = new List<ChatMessageViewDto>
        {
            new() { Id = Guid.NewGuid(), ConversationId = _conversationId, SenderType = "Patient", Content = "مرحبا", CreatedAt = DateTime.UtcNow }
        }
    };

    // --- GetConversations ---

    [Fact]
    public async Task GetConversations_ReturnsOkWithList()
    {
        var conversations = new[] { MakeConversationDto(), MakeConversationDto(Guid.NewGuid()) };
        _chatServiceMock.Setup(x => x.GetConversationsAsync(_userId, null)).ReturnsAsync(conversations);

        var result = await _sut.GetConversations(null);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var data = ok.Value.Should().BeAssignableTo<IEnumerable<ConversationViewDto>>().Subject;
        data.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetConversations_WithPatientId_PassesFilterToService()
    {
        _chatServiceMock.Setup(x => x.GetConversationsAsync(_userId, _patientId)).ReturnsAsync(new[] { MakeConversationDto() });

        var result = await _sut.GetConversations(_patientId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        _chatServiceMock.Verify(x => x.GetConversationsAsync(_userId, _patientId), Times.Once);
    }

    // --- GetHistory ---

    [Fact]
    public async Task GetHistory_ExistingConversation_ReturnsOk()
    {
        _chatServiceMock.Setup(x => x.GetHistoryAsync(_userId, _conversationId)).ReturnsAsync(MakeHistoryDto());

        var result = await _sut.GetHistory(_conversationId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var history = ok.Value.Should().BeOfType<ChatHistoryDto>().Subject;
        history.ConversationId.Should().Be(_conversationId);
        history.Messages.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetHistory_NonExistent_Returns404()
    {
        _chatServiceMock.Setup(x => x.GetHistoryAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((ChatHistoryDto?)null);

        var result = await _sut.GetHistory(Guid.NewGuid());

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    // --- CreateConversation ---

    [Fact]
    public async Task CreateConversation_ValidPatient_Returns201()
    {
        var dto = MakeConversationDto();
        _chatServiceMock.Setup(x => x.CreateConversationAsync(_userId, _patientId)).ReturnsAsync(dto);

        var result = await _sut.CreateConversation(new CreateConversationDto(_patientId));

        var created = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        created.StatusCode.Should().Be(201);
        var value = created.Value.Should().BeOfType<ConversationViewDto>().Subject;
        value.PatientId.Should().Be(_patientId);
    }

    [Fact]
    public async Task CreateConversation_PatientNotFound_Returns404()
    {
        _chatServiceMock.Setup(x => x.CreateConversationAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ThrowsAsync(new KeyNotFoundException("Patient not found"));

        var result = await _sut.CreateConversation(new CreateConversationDto(Guid.NewGuid()));

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    // --- CloseConversation ---

    [Fact]
    public async Task CloseConversation_Existing_ReturnsOk()
    {
        var dto = MakeConversationDto();
        dto.Status = "Closed";
        _chatServiceMock.Setup(x => x.CloseConversationAsync(_userId, _conversationId)).ReturnsAsync(dto);

        var result = await _sut.CloseConversation(_conversationId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = ok.Value.Should().BeOfType<ConversationViewDto>().Subject;
        value.Status.Should().Be("Closed");
    }

    [Fact]
    public async Task CloseConversation_NonExistent_Returns404()
    {
        _chatServiceMock.Setup(x => x.CloseConversationAsync(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync((ConversationViewDto?)null);

        var result = await _sut.CloseConversation(Guid.NewGuid());

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    // --- Send ---

    [Fact]
    public async Task Send_ValidMessage_ReturnsOk()
    {
        var messageDto = new ChatMessageViewDto
        {
            Id = Guid.NewGuid(),
            ConversationId = _conversationId,
            SenderType = "Therapist",
            Content = "كيف حالك اليوم؟",
            CreatedAt = DateTime.UtcNow
        };
        _chatServiceMock.Setup(x => x.SendMessageAsync(_userId, _conversationId, "كيف حالك اليوم؟", "Therapist"))
            .ReturnsAsync(messageDto);

        var result = await _sut.Send(new SendMessageDto(_conversationId, "كيف حالك اليوم؟"));

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = ok.Value.Should().BeOfType<ChatMessageViewDto>().Subject;
        value.SenderType.Should().Be("Therapist");
        value.Content.Should().Be("كيف حالك اليوم؟");
    }

    [Fact]
    public async Task Send_NonExistentConversation_Returns404()
    {
        _chatServiceMock.Setup(x => x.SendMessageAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((ChatMessageViewDto?)null);

        var result = await _sut.Send(new SendMessageDto(Guid.NewGuid(), "test"));

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Send_PatientRole_SetsSenderTypePatient()
    {
        var patientUserId = Guid.NewGuid();
        var patientClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, patientUserId.ToString()),
            new(ClaimTypes.Role, "Patient")
        };
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(patientClaims, "Test")) }
        };

        _chatServiceMock.Setup(x => x.SendMessageAsync(patientUserId, _conversationId, "مرحبا", "Patient"))
            .ReturnsAsync(new ChatMessageViewDto { Id = Guid.NewGuid(), ConversationId = _conversationId, SenderType = "Patient", Content = "مرحبا", CreatedAt = DateTime.UtcNow });

        var result = await _sut.Send(new SendMessageDto(_conversationId, "مرحبا"));

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        _chatServiceMock.Verify(x => x.SendMessageAsync(patientUserId, _conversationId, "مرحبا", "Patient"), Times.Once);
    }
}
