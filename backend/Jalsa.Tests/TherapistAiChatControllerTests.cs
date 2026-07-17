using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.API.DTOs.AI;
using Jalsa.API.Hubs;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.DTOs.TherapistChat;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Jalsa.Tests;

public class TherapistAiChatControllerTests
{
    private readonly Mock<ITherapistAiChatService> _chatServiceMock;
    private readonly Mock<IHubContext<TherapistAiChatHub>> _hubContextMock;
    private readonly Mock<ITherapistChatAiService> _therapistChatAiMock;
    private readonly Mock<ITherapistAiMemoryService> _memoryMock;
    private readonly TherapistAiChatController _sut;
    private readonly Guid _userId;

    public TherapistAiChatControllerTests()
    {
        _chatServiceMock = new Mock<ITherapistAiChatService>();
        _hubContextMock = new Mock<IHubContext<TherapistAiChatHub>>();
        _therapistChatAiMock = new Mock<ITherapistChatAiService>();
        _memoryMock = new Mock<ITherapistAiMemoryService>();

        var clientsMock = new Mock<IHubClients>();
        var clientProxyMock = new Mock<IClientProxy>();
        clientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(clientProxyMock.Object);
        _hubContextMock.Setup(h => h.Clients).Returns(clientsMock.Object);

        _sut = new TherapistAiChatController(
            _chatServiceMock.Object,
            _hubContextMock.Object,
            _therapistChatAiMock.Object,
            _memoryMock.Object);

        _userId = Guid.NewGuid();
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _userId.ToString()) };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [Fact]
    public void Controller_IsRestrictedToTherapistRole()
    {
        var attribute = typeof(TherapistAiChatController)
            .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true)
            .Cast<AuthorizeAttribute>()
            .Single();

        attribute.Roles.Should().Be("Therapist");
    }

    [Fact]
    public async Task GetConversations_ReturnsServiceResult()
    {
        var patientId = Guid.NewGuid();
        var conversations = new[]
        {
            new TherapistConversationViewDto { Id = Guid.NewGuid(), PatientId = patientId, Status = "Open" }
        };
        _chatServiceMock.Setup(s => s.GetConversationsAsync(_userId, null)).ReturnsAsync(conversations);

        var result = await _sut.GetConversations(null);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(conversations);
    }

    [Fact]
    public async Task GetHistory_ConversationNotFound_ReturnsNotFound()
    {
        _chatServiceMock.Setup(s => s.GetHistoryAsync(_userId, It.IsAny<Guid>())).ReturnsAsync((TherapistChatHistoryDto?)null);

        var result = await _sut.GetHistory(Guid.NewGuid());

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task CreateConversation_PatientNotOwnedByTherapist_ReturnsNotFound()
    {
        // Mirrors the real service behavior: TherapistAiChatService.CreateConversationAsync
        // throws KeyNotFoundException when Patient.TherapistId != CurrentTherapistId.
        _chatServiceMock
            .Setup(s => s.CreateConversationAsync(_userId, It.IsAny<Guid>()))
            .ThrowsAsync(new KeyNotFoundException("المريض غير موجود"));

        var result = await _sut.CreateConversation(new CreateTherapistConversationDto(Guid.NewGuid()));

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Send_PersistsTherapistMessage_RegardlessOfCallerRoleClaim()
    {
        // Unlike the old shared ChatController, senderType is never derived from the
        // caller's role claim here — it's hardcoded "Therapist" because this controller is
        // already restricted to the Therapist role.
        var conversationId = Guid.NewGuid();
        var savedMessage = new TherapistChatMessageViewDto
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderType = "Therapist",
            Content = "Summarize this patient",
        };
        _chatServiceMock
            .Setup(s => s.SendMessageAsync(_userId, conversationId, "Summarize this patient", "Therapist"))
            .ReturnsAsync(savedMessage);
        _chatServiceMock
            .Setup(s => s.GetHistoryAsync(_userId, conversationId))
            .ReturnsAsync((TherapistChatHistoryDto?)null);

        var result = await _sut.Send(new SendTherapistMessageDto(conversationId, "Summarize this patient"));

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(savedMessage);
        _chatServiceMock.Verify(s => s.SendMessageAsync(_userId, conversationId, "Summarize this patient", "Therapist"), Times.Once);
    }
}
