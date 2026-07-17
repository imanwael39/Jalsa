using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Jalsa.Application.DTOs.PatientSupportChat;
using Jalsa.Application.Interfaces.Services;
using Jalsa.API.Hubs;

namespace Jalsa.API.Controllers;

/// <summary>
/// Patient-only support chat REST surface. Class-level Patient role restriction is the
/// first isolation boundary — a therapist token can never reach any action here, and no
/// action accepts a patientId from the caller (always resolved server-side from the token).
/// </summary>
[ApiController]
[Route("api/patient/support-chat")]
[Authorize(Roles = "Patient")]
public class PatientSupportChatController : BaseController
{
    private readonly IPatientSupportChatService _chatService;
    private readonly IHubContext<PatientSupportChatHub> _chatHub;

    public PatientSupportChatController(
        IPatientSupportChatService chatService,
        IHubContext<PatientSupportChatHub> chatHub)
    {
        _chatService = chatService;
        _chatHub = chatHub;
    }

    [HttpGet("conversation")]
    public async Task<IActionResult> GetOrCreateConversation()
    {
        var userId = GetCurrentUserId();
        var result = await _chatService.GetOrCreateConversationAsync(userId);
        return Ok(result);
    }

    [HttpGet("{conversationId:guid}/history")]
    public async Task<IActionResult> GetHistory(Guid conversationId)
    {
        var userId = GetCurrentUserId();
        var history = await _chatService.GetHistoryAsync(userId, conversationId);
        if (history is null)
            return NotFound(new { message = "المحادثة غير موجودة" });

        return Ok(history);
    }

    /// <summary>
    /// Fallback path when the SignalR connection is down — persists the patient's message
    /// only, mirroring the hub's own message write. It intentionally does not generate an AI
    /// reply (that only happens over the live hub connection), matching prior behavior.
    /// </summary>
    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendPatientSupportMessageDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _chatService.SendMessageAsync(userId, dto.ConversationId, dto.Content, "Patient");

        if (result is null)
            return NotFound(new { message = "المحادثة غير موجودة" });

        await _chatHub.Clients.Group(dto.ConversationId.ToString()).SendAsync("ReceiveHumanMessage", result);

        return Ok(result);
    }
}
