using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Jalsa.Application.DTOs.Chat;
using Jalsa.Application.Interfaces.Services;
using Jalsa.API.Hubs;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class ChatController : BaseController
{
    private readonly IChatService _chatService;
    private readonly IHubContext<ChatHub> _chatHub;

    public ChatController(IChatService chatService, IHubContext<ChatHub> chatHub)
    {
        _chatService = chatService;
        _chatHub = chatHub;
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations([FromQuery] Guid? patientId)
    {
        var userId = GetCurrentUserId();
        var conversations = await _chatService.GetConversationsAsync(userId, patientId);
        return Ok(conversations);
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

    [HttpPost("conversations")]
    public async Task<IActionResult> CreateConversation([FromBody] CreateConversationDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _chatService.CreateConversationAsync(userId, dto.PatientId);
            return CreatedAtAction(nameof(GetHistory), new { conversationId = result.Id }, result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPatch("conversations/{conversationId:guid}/close")]
    public async Task<IActionResult> CloseConversation(Guid conversationId)
    {
        var userId = GetCurrentUserId();
        var result = await _chatService.CloseConversationAsync(userId, conversationId);
        if (result is null)
            return NotFound(new { message = "المحادثة غير موجودة" });

        return Ok(result);
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendMessageDto dto)
    {
        var userId = GetCurrentUserId();
        var senderType = User.IsInRole("Patient") ? "Patient" : "Therapist";
        var result = await _chatService.SendMessageAsync(userId, dto.ConversationId, dto.Content, senderType);

        if (result is null)
            return NotFound(new { message = "المحادثة غير موجودة" });

        await _chatHub.Clients.Group(dto.ConversationId.ToString()).SendAsync("ReceiveHumanMessage", result);

        return Ok(result);
    }
}
