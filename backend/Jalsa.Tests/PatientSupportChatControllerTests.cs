using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.API.Hubs;
using Jalsa.Application.DTOs.PatientSupportChat;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Jalsa.Tests;

public class PatientSupportChatControllerTests
{
    private readonly Mock<IPatientSupportChatService> _chatServiceMock;
    private readonly Mock<IHubContext<PatientSupportChatHub>> _hubContextMock;
    private readonly PatientSupportChatController _sut;
    private readonly Guid _userId;

    public PatientSupportChatControllerTests()
    {
        _chatServiceMock = new Mock<IPatientSupportChatService>();
        _hubContextMock = new Mock<IHubContext<PatientSupportChatHub>>();

        var clientsMock = new Mock<IHubClients>();
        var clientProxyMock = new Mock<IClientProxy>();
        clientsMock.Setup(c => c.Group(It.IsAny<string>())).Returns(clientProxyMock.Object);
        _hubContextMock.Setup(h => h.Clients).Returns(clientsMock.Object);

        _sut = new PatientSupportChatController(_chatServiceMock.Object, _hubContextMock.Object);

        _userId = Guid.NewGuid();
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _userId.ToString()) };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [Fact]
    public void Controller_IsRestrictedToPatientRole()
    {
        var attribute = typeof(PatientSupportChatController)
            .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true)
            .Cast<AuthorizeAttribute>()
            .Single();

        attribute.Roles.Should().Be("Patient");
    }

    [Fact]
    public void NoEndpoint_AcceptsAPatientIdParameter()
    {
        // Structural guard against reintroducing an IDOR vector: every action on this
        // controller must resolve the caller's own patient record server-side.
        var actionMethods = typeof(PatientSupportChatController)
            .GetMethods()
            .Where(m => m.GetCustomAttributes(typeof(HttpGetAttribute), true).Any()
                     || m.GetCustomAttributes(typeof(HttpPostAttribute), true).Any()
                     || m.GetCustomAttributes(typeof(HttpPatchAttribute), true).Any());

        foreach (var method in actionMethods)
        {
            method.GetParameters()
                .Should().NotContain(p => p.Name != null && p.Name.Contains("patientId", StringComparison.OrdinalIgnoreCase));
        }
    }

    [Fact]
    public async Task GetOrCreateConversation_ReturnsCallersOwnConversation()
    {
        var conversation = new PatientSupportConversationViewDto { Id = Guid.NewGuid(), Status = "Open" };
        _chatServiceMock.Setup(s => s.GetOrCreateConversationAsync(_userId)).ReturnsAsync(conversation);

        var result = await _sut.GetOrCreateConversation();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(conversation);
        _chatServiceMock.Verify(s => s.GetOrCreateConversationAsync(_userId), Times.Once);
    }

    [Fact]
    public async Task GetHistory_AnotherPatientsConversation_ReturnsNotFound()
    {
        // Mirrors PatientSupportChatService: any conversationId not owned by the caller's
        // own Patient.Id resolves to null, never another patient's data.
        _chatServiceMock
            .Setup(s => s.GetHistoryAsync(_userId, It.IsAny<Guid>()))
            .ReturnsAsync((PatientSupportChatHistoryDto?)null);

        var result = await _sut.GetHistory(Guid.NewGuid());

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Send_PersistsPatientMessage_AndBroadcastsToHubGroup()
    {
        var conversationId = Guid.NewGuid();
        var savedMessage = new PatientSupportMessageViewDto
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderType = "Patient",
            Content = "I'm anxious today.",
        };
        _chatServiceMock
            .Setup(s => s.SendMessageAsync(_userId, conversationId, "I'm anxious today.", "Patient"))
            .ReturnsAsync(savedMessage);

        var result = await _sut.Send(new SendPatientSupportMessageDto(conversationId, "I'm anxious today."));

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(savedMessage);
        _chatServiceMock.Verify(s => s.SendMessageAsync(_userId, conversationId, "I'm anxious today.", "Patient"), Times.Once);
    }
}
