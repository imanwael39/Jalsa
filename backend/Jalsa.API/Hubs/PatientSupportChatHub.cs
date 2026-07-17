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
using Microsoft.Extensions.Logging;

namespace Jalsa.API.Hubs;

/// <summary>
/// Patient-only real-time support chat. Ownership check is Patient.UserId == CurrentUserId
/// only — no therapist branch, so a therapist's token can never join a patient's group or
/// invoke SendMessage here. Crisis detection runs on every patient message purely as a
/// background safety check (creates a CrisisAlert + notifies the assigned therapist) — it
/// never blocks, delays, or alters the AI's reply to the patient. This hub depends only on
/// ICrisisDetectionService, never a concrete detector, so the engine can be swapped later
/// without touching this flow.
/// </summary>
[Authorize(Roles = "Patient")]
public class PatientSupportChatHub : Hub
{
    private const int CrisisContextHistoryCount = 5;

    private readonly IPatientSupportAiService _chatAi;
    private readonly IPatientSupportMemoryService _memory;
    private readonly ICrisisDetectionService _crisisDetection;
    private readonly INotificationPushService _pushService;
    private readonly JalsaDbContext _context;
    private readonly ILogger<PatientSupportChatHub> _logger;

    public PatientSupportChatHub(
        IPatientSupportAiService chatAi,
        IPatientSupportMemoryService memory,
        ICrisisDetectionService crisisDetection,
        INotificationPushService pushService,
        JalsaDbContext context,
        ILogger<PatientSupportChatHub> logger)
    {
        _chatAi = chatAi;
        _memory = memory;
        _crisisDetection = crisisDetection;
        _pushService = pushService;
        _context = context;
        _logger = logger;
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

        var recentHistory = await _context.PatientSupportMessages
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.CreatedAt)
            .Take(CrisisContextHistoryCount)
            .OrderBy(m => m.CreatedAt)
            .Select(m => $"{m.SenderType}: {m.Content}")
            .ToListAsync();

        var crisisResult = await _crisisDetection.AnalyzeAsync(message, recentHistory);

        Notification? crisisNotification = null;
        Guid? crisisNotificationRecipientId = null;
        if (crisisResult.IsCrisis)
        {
            var therapist = await _context.Therapists
                .Where(t => t.Patients.Any(p => p.Id == patient.Id))
                .FirstOrDefaultAsync();

            var alert = new CrisisAlert
            {
                Id = Guid.NewGuid(),
                PatientId = patient.Id,
                TherapistId = therapist?.Id,
                ConversationId = conversationId,
                Severity = crisisResult.Severity.ToString(),
                Reason = crisisResult.Reason,
                Confidence = crisisResult.Confidence,
                ChatMessageId = patientMsg.Id,
                Status = "New",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.CrisisAlerts.Add(alert);

            _logger.LogWarning(
                "Crisis alert {AlertId} created for patient {PatientId} (severity={Severity}, confidence={Confidence})",
                alert.Id, patient.Id, alert.Severity, alert.Confidence);

            if (therapist != null)
            {
                crisisNotification = new Notification
                {
                    Id = Guid.NewGuid(),
                    RecipientUserId = therapist.UserId,
                    Type = "CrisisAlert",
                    Title = "🚨 تنبيه أزمة",
                    Body = $"قد يمر المريض {patient.FullName} بأزمة نفسية. الخطورة: {alert.Severity}.",
                    CreatedAt = DateTime.UtcNow
                };
                crisisNotificationRecipientId = therapist.UserId;
                _context.Notifications.Add(crisisNotification);
            }
            else
            {
                _logger.LogWarning("Crisis alert {AlertId} has no assigned therapist to notify", alert.Id);
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
                _logger.LogInformation(
                    "Crisis notification delivered in real time to therapist {TherapistUserId}",
                    crisisNotificationRecipientId.Value);
            }
            catch (Exception ex)
            {
                // Real-time push is a convenience layer; polling/page load is the source of truth.
                _logger.LogWarning(ex,
                    "Crisis notification real-time push failed for therapist {TherapistUserId}; row persisted for polling",
                    crisisNotificationRecipientId.Value);
            }
        }

        await _memory.StoreMessageMemoryAsync(conversationId, patient.Id, patientMsg.Id, message);

        // The AI reply is generated the same way regardless of crisis detection — the
        // patient must never see that a background safety check ran. The patient-support
        // system prompt already instructs the model to respond supportively and encourage
        // contacting the therapist/emergency services when crisis themes appear, so no
        // separate canned response is needed here.
        var group = conversationId.ToString();
        var reply = await _chatAi.GenerateResponseStreamingAsync(
            conversationId,
            patient.Id,
            message,
            delta => Clients.Group(group).SendAsync("ReceiveMessageChunk", delta));

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
