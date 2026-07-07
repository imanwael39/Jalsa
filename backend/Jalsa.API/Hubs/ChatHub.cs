using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Domain.Models.Chat;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Crisis;
using Jalsa.Domain.Models.Notification;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatAiService _chatAi;
    private readonly IConversationMemoryService _memory;
    private readonly ICrisisDetectionService _crisisDetection;
    private readonly JalsaDbContext _context;

    public ChatHub(
        IChatAiService chatAi,
        IConversationMemoryService memory,
        ICrisisDetectionService crisisDetection,
        JalsaDbContext context)
    {
        _chatAi = chatAi;
        _memory = memory;
        _crisisDetection = crisisDetection;
        _context = context;
    }

    public async Task SendMessage(Guid conversationId, Guid patientId, string message)
    {
        var userIdClaim = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? Context.User?.FindFirstValue("sub");

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new HubException("Unauthorized: invalid user identity.");

        var therapistId = await TryResolveTherapistIdAsync(userId);

        var conversation = await _context.ChatConversations
            .Include(c => c.Patient)
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null)
            throw new HubException("Conversation not found.");

        if (conversation.Patient.TherapistId != therapistId && conversation.Patient.UserId != userId)
            throw new HubException("غير مصرح لك بالوصول إلى هذه المحادثة");

        var patientMsg = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderType = "Patient",
            Content = message,
            CreatedAt = DateTime.UtcNow
        };
        _context.ChatMessages.Add(patientMsg);

        var lang = message.Any(c => c >= 0x0600 && c <= 0x06FF) ? "ar" : "en";
        var crisisResult = await _crisisDetection.AnalyzeAsync(message);
        if (crisisResult.IsCrisis)
        {
            var therapist = await _context.Therapists
                .Where(t => t.Patients.Any(p => p.Id == patientId))
                .FirstOrDefaultAsync();

            _context.CrisisAlerts.Add(new CrisisAlert
            {
                Id = Guid.NewGuid(),
                PatientId = patientId,
                TherapistId = therapist?.Id,
                Severity = "High",
                ChatMessageId = patientMsg.Id,
                Status = "Open",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            if (therapist != null)
            {
                var snippet = patientMsg.Content[..Math.Min(100, patientMsg.Content.Length)];
                _context.Notifications.Add(new Notification
                {
                    Id = Guid.NewGuid(),
                    RecipientUserId = therapist.UserId,
                    Type = "CrisisAlert",
                    Title = lang == "ar" ? "تنبيه أزمة" : "Crisis Alert",
                    Body = lang == "ar" ? $"المريض: {snippet}..." : $"Patient: {snippet}...",
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await _context.SaveChangesAsync();

        await _memory.StoreMessageMemoryAsync(conversationId, patientId, patientMsg.Id, message);

        string reply;
        if (crisisResult.IsCrisis)
        {
            reply = crisisResult.SuggestedMessage!;
        }
        else
        {
            var group = conversationId.ToString();
            reply = await _chatAi.GenerateResponseStreamingAsync(
                conversationId,
                patientId,
                message,
                delta => Clients.Group(group).SendAsync("ReceiveMessageChunk", delta));
        }

        var aiMsg = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderType = "AI",
            Content = reply,
            CreatedAt = DateTime.UtcNow
        };
        _context.ChatMessages.Add(aiMsg);
        await _context.SaveChangesAsync();

        await _memory.StoreMessageMemoryAsync(conversationId, patientId, aiMsg.Id, reply!);

        var senderName = lang == "ar" ? "المساعد" : "AI";
        await Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", senderName, reply);
    }

    public async Task JoinConversation(string conversationId)
    {
        var userIdClaim = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? Context.User?.FindFirstValue("sub");

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new HubException("Unauthorized: invalid user identity.");

        if (!Guid.TryParse(conversationId, out var convGuid))
            throw new HubException("Invalid conversation ID.");

        var therapistId = await TryResolveTherapistIdAsync(userId);

        var conversation = await _context.ChatConversations
            .Include(c => c.Patient)
            .FirstOrDefaultAsync(c => c.Id == convGuid);

        if (conversation == null)
            throw new HubException("Conversation not found.");

        if (conversation.Patient.TherapistId != therapistId && conversation.Patient.UserId != userId)
            throw new HubException("غير مصرح لك بالوصول إلى هذه المحادثة");

        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
    }

    private async Task<Guid?> TryResolveTherapistIdAsync(Guid userId)
    {
        var therapist = await _context.Therapists.FirstOrDefaultAsync(t => t.UserId == userId);
        return therapist?.Id;
    }
}
