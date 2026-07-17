using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.DTOs.Notification;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Chat;
using Jalsa.Domain.Models.Crisis;
using Jalsa.Domain.Models.Notification;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.API.Hubs;

/// <summary>
/// Patient-only real-time support chat. Ownership check is Patient.UserId == CurrentUserId
/// only — no therapist branch, so a therapist's token can never join a patient's group or
/// invoke SendMessage here. Crisis detection still fires on every patient message and still
/// notifies the assigned therapist (that's a safety mechanism, not a chat-content leak: the
/// therapist gets a notification + CrisisAlert record, never the conversation itself).
/// </summary>
[Authorize(Roles = "Patient")]
public class PatientSupportChatHub : Hub
{
    private readonly IPatientSupportAiService _chatAi;
    private readonly IPatientSupportMemoryService _memory;
    private readonly ICrisisDetectionService _crisisDetection;
    private readonly INotificationPushService _pushService;
    private readonly JalsaDbContext _context;

    public PatientSupportChatHub(
        IPatientSupportAiService chatAi,
        IPatientSupportMemoryService memory,
        ICrisisDetectionService crisisDetection,
        INotificationPushService pushService,
        JalsaDbContext context)
    {
        _chatAi = chatAi;
        _memory = memory;
        _crisisDetection = crisisDetection;
        _pushService = pushService;
        _context = context;
    }

    public async Task SendMessage(Guid conversationId, string message)
    {
        var userId = await ResolveUserIdAsync();
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
        if (patient is null)
            throw new HubException("Unauthorized: no patient profile for this account.");

        var conversation = await _context.PatientSupportConversations
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null)
            throw new HubException("Conversation not found.");

        if (conversation.PatientId != patient.Id)
            throw new HubException("غير مصرح لك بالوصول إلى هذه المحادثة");

        var patientMsg = new PatientSupportMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderType = "Patient",
            Content = message,
            CreatedAt = DateTime.UtcNow
        };
        _context.PatientSupportMessages.Add(patientMsg);

        var lang = message.Any(c => c >= 0x0600 && c <= 0x06FF) ? "ar" : "en";
        var crisisResult = await _crisisDetection.AnalyzeAsync(message);
        Notification? crisisNotification = null;
        Guid? crisisNotificationRecipientId = null;
        if (crisisResult.IsCrisis)
        {
            var therapist = await _context.Therapists
                .Where(t => t.Patients.Any(p => p.Id == patient.Id))
                .FirstOrDefaultAsync();

            _context.CrisisAlerts.Add(new CrisisAlert
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
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
                crisisNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    RecipientUserId = therapist.UserId,
                    Type = "CrisisAlert",
                    Title = lang == "ar" ? "تنبيه أزمة" : "Crisis Alert",
                    Body = lang == "ar" ? $"المريض: {snippet}..." : $"Patient: {snippet}...",
                    CreatedAt = DateTime.UtcNow
                };
                crisisNotificationRecipientId = therapist.UserId;
                _context.Notifications.Add(crisisNotification);
            }
        }

        await _context.SaveChangesAsync();

        if (crisisNotification != null && crisisNotificationRecipientId.HasValue)
        {
            // A push failure must never break the chat flow — the notification row
            // is already persisted and will show up on the therapist's next poll.
            try
            {
                await _pushService.PushToUserAsync(crisisNotificationRecipientId.Value, new NotificationViewDto
                {
                    Id = crisisNotification.Id,
                    Type = crisisNotification.Type,
                    Title = crisisNotification.Title,
                    Body = crisisNotification.Body,
                    IsRead = crisisNotification.IsRead,
                    ReadAt = crisisNotification.ReadAt,
                    CreatedAt = crisisNotification.CreatedAt
                });
            }
            catch
            {
                // Real-time push is a convenience layer; polling/page load is the source of truth.
            }
        }

        await _memory.StoreMessageMemoryAsync(conversationId, patient.Id, patientMsg.Id, message);

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
                patient.Id,
                message,
                delta => Clients.Group(group).SendAsync("ReceiveMessageChunk", delta));
        }

        var aiMsg = new PatientSupportMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderType = "AI",
            Content = reply,
            CreatedAt = DateTime.UtcNow
        };
        _context.PatientSupportMessages.Add(aiMsg);
        await _context.SaveChangesAsync();

        await _memory.StoreMessageMemoryAsync(conversationId, patient.Id, aiMsg.Id, reply!);

        var senderName = lang == "ar" ? "المساعد" : "AI";
        await Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", senderName, reply);
    }

    public async Task JoinConversation(string conversationId)
    {
        var userId = await ResolveUserIdAsync();

        if (!Guid.TryParse(conversationId, out var convGuid))
            throw new HubException("Invalid conversation ID.");

        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
        if (patient is null)
            throw new HubException("Unauthorized: no patient profile for this account.");

        var conversation = await _context.PatientSupportConversations
            .FirstOrDefaultAsync(c => c.Id == convGuid);

        if (conversation == null)
            throw new HubException("Conversation not found.");

        if (conversation.PatientId != patient.Id)
            throw new HubException("غير مصرح لك بالوصول إلى هذه المحادثة");

        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
    }

    private Task<Guid> ResolveUserIdAsync()
    {
        var userIdClaim = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? Context.User?.FindFirstValue("sub");

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new HubException("Unauthorized: invalid user identity.");

        return Task.FromResult(userId);
    }
}
