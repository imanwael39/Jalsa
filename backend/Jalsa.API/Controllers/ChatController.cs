using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jalsa.API.Exceptions;
using Jalsa.Infrastructure.Data;
using Jalsa.Domain.Models.Chat;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/chat")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly JalsaDbContext _context;

    public ChatController(JalsaDbContext context)
    {
        _context = context;
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetConversations([FromQuery] Guid? patientId)
    {
        var query = _context.ChatConversations.AsQueryable();

        if (patientId.HasValue)
            query = query.Where(c => c.PatientId == patientId.Value);

        var conversations = await query
            .OrderByDescending(c => c.LastActivityAt ?? c.CreatedAt)
            .Select(c => new
            {
                c.Id,
                c.PatientId,
                PatientName = c.Patient.FullName,
                c.Status,
                c.LastActivityAt,
                c.CreatedAt,
                MessageCount = c.ChatMessages.Count
            })
            .ToListAsync();

        return Ok(conversations);
    }

    [HttpGet("{conversationId:guid}/history")]
    public async Task<IActionResult> GetHistory(Guid conversationId)
    {
        var conversation = await _context.ChatConversations
            .Include(c => c.ChatMessages.OrderBy(m => m.CreatedAt))
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation is null)
            return NotFound(new { message = "المحادثة غير موجودة" });

        var messages = conversation.ChatMessages.Select(m => new
        {
            m.Id,
            m.ConversationId,
            m.SenderType,
            m.Content,
            m.CreatedAt
        });

        return Ok(messages);
    }

    [HttpPost("conversations")]
    public async Task<IActionResult> CreateConversation([FromBody] CreateConversationRequest request)
    {
        var patientExists = await _context.Patients.AnyAsync(p => p.Id == request.PatientId);
        if (!patientExists)
            return NotFound(new { message = "المريض غير موجود" });

        var existing = await _context.ChatConversations
            .Where(c => c.PatientId == request.PatientId && c.Status == "Open")
            .FirstOrDefaultAsync();

        if (existing is not null)
            return Ok(new { existing.Id, existing.PatientId, existing.Status, existing.CreatedAt });

        var conversation = new ChatConversation
        {
            Id = Guid.NewGuid(),
            PatientId = request.PatientId,
            Status = "Open",
            LastActivityAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.ChatConversations.Add(conversation);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHistory), new { conversationId = conversation.Id }, new
        {
            conversation.Id,
            conversation.PatientId,
            conversation.Status,
            conversation.CreatedAt
        });
    }

    [HttpPatch("conversations/{conversationId:guid}/close")]
    public async Task<IActionResult> CloseConversation(Guid conversationId)
    {
        var conversation = await _context.ChatConversations.FindAsync(conversationId);
        if (conversation is null)
            return NotFound(new { message = "المحادثة غير موجودة" });

        conversation.Status = "Closed";
        conversation.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { conversation.Id, conversation.Status });
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] SendMessageRequest request)
    {
        var conversation = await _context.ChatConversations.FindAsync(request.ConversationId);
        if (conversation is null)
            return NotFound(new { message = "المحادثة غير موجودة" });

        var userId = GetCurrentUserId();
        var senderType = User.IsInRole("Patient") ? "Patient" : "Therapist";

        var message = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = request.ConversationId,
            SenderType = senderType,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        _context.ChatMessages.Add(message);
        conversation.LastActivityAt = DateTime.UtcNow;
        conversation.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(new { message.Id, message.ConversationId, message.SenderType, message.Content, message.CreatedAt });
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var userId))
            throw new ApiException(401, "Invalid authentication token");
        return userId;
    }
}

public record CreateConversationRequest(Guid PatientId);
public record SendMessageRequest(Guid ConversationId, string Content);
