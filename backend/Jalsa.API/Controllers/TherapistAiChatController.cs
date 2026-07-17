using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Jalsa.Application.DTOs.TherapistChat;
using Jalsa.Application.Interfaces.Services;
using Jalsa.API.Hubs;
using Jalsa.API.Services.Interfaces.AI;

namespace Jalsa.API.Controllers;

/// <summary>
/// Therapist-only clinical assistant chat. Class-level Therapist role restriction is the
/// first isolation boundary — a patient token can never reach any action here.
/// </summary>
[ApiController]
[Route("api/therapist-chat")]
[Authorize(Roles = "Therapist")]
public class TherapistAiChatController : BaseController
{
    private readonly ITherapistAiChatService _chatService;
    private readonly IHubContext<TherapistAiChatHub> _chatHub;
    private readonly ITherapistChatAiService _therapistChatAi;
    private readonly ITherapistAiMemoryService _memory;

    public TherapistAiChatController(
        ITherapistAiChatService chatService,
        IHubContext<TherapistAiChatHub> chatHub,
        ITherapistChatAiService therapistChatAi,
        ITherapistAiMemoryService memory)
    {
        _chatService = chatService;
        _chatHub = chatHub;
        _therapistChatAi = therapistChatAi;
        _memory = memory;
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
    public async Task<IActionResult> CreateConversation([FromBody] CreateTherapistConversationDto dto)
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
    public async Task<IActionResult> Send([FromBody] SendTherapistMessageDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _chatService.SendMessageAsync(userId, dto.ConversationId, dto.Content, "Therapist");

        if (result is null)
            return NotFound(new { message = "المحادثة غير موجودة" });

        await _chatHub.Clients.Group(dto.ConversationId.ToString()).SendAsync("ReceiveHumanMessage", result);

        try
        {
            var history = await _chatService.GetHistoryAsync(userId, dto.ConversationId);
            if (history is not null)
            {
                await _memory.StoreMessageMemoryAsync(dto.ConversationId, history.PatientId, result.Id, dto.Content);

                var lang = dto.Content.Any(c => c >= 0x0600 && c <= 0x06FF) ? "ar" : "en";
                var group = dto.ConversationId.ToString();
                var diagnostics = await _therapistChatAi.AnswerQuestionStreamingAsync(
                    dto.ConversationId,
                    history.PatientId,
                    dto.Content,
                    lang,
                    delta => _chatHub.Clients.Group(group).SendAsync("ReceiveMessageChunk", delta));
                var answer = diagnostics.Output;
                var aiResult = await _chatService.SendMessageAsync(userId, dto.ConversationId, answer, "AI");
                if (aiResult is not null)
                {
                    await _chatHub.Clients.Group(dto.ConversationId.ToString()).SendAsync("ReceiveHumanMessage", aiResult);
                    await _memory.StoreMessageMemoryAsync(dto.ConversationId, history.PatientId, aiResult.Id, answer);
                }
            }
        }
        catch
        {
            // Best-effort: a Gateway/memory failure must never block the therapist's own message from sending.
        }

        return Ok(result);
    }

    [HttpPost("{conversationId:guid}/regenerate")]
    public async Task<IActionResult> Regenerate(Guid conversationId)
    {
        var userId = GetCurrentUserId();
        var history = await _chatService.GetHistoryAsync(userId, conversationId);
        if (history is null)
            return NotFound(new { message = "المحادثة غير موجودة" });

        var lastTherapistMessage = history.Messages
            .LastOrDefault(m => m.SenderType == "Therapist" && !string.IsNullOrWhiteSpace(m.Content));

        if (lastTherapistMessage?.Content is null)
            return BadRequest(new { message = "لا توجد رسالة معالج لإعادة توليد رد عليها" });

        var lang = lastTherapistMessage.Content.Any(c => c >= 0x0600 && c <= 0x06FF) ? "ar" : "en";
        var group = conversationId.ToString();
        var diagnostics = await _therapistChatAi.AnswerQuestionStreamingAsync(
            conversationId,
            history.PatientId,
            lastTherapistMessage.Content,
            lang,
            delta => _chatHub.Clients.Group(group).SendAsync("ReceiveMessageChunk", delta));
        var answer = diagnostics.Output;
        var aiResult = await _chatService.SendMessageAsync(userId, conversationId, answer, "AI");

        if (aiResult is null)
            return NotFound(new { message = "المحادثة غير موجودة" });

        await _chatHub.Clients.Group(conversationId.ToString()).SendAsync("ReceiveHumanMessage", aiResult);

        try
        {
            await _memory.StoreMessageMemoryAsync(conversationId, history.PatientId, aiResult.Id, answer);
        }
        catch
        {
            // Best-effort: memory storage must never block the regenerated reply from returning.
        }

        return Ok(aiResult);
    }
}
