using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.API.Hubs;

/// <summary>
/// Therapist-only channel used purely for streaming AI reply chunks to the therapist's own
/// browser tab. Message persistence stays REST-driven (TherapistAiChatController.Send) —
/// this hub has no client-invokable send method, only group membership for a conversation
/// the caller owns. A patient token can never join a group here: the ownership check below
/// is Patient.TherapistId == CurrentTherapistId, with no Patient.UserId fallback.
/// </summary>
[Authorize(Roles = "Therapist")]
public class TherapistAiChatHub : Hub
{
    private readonly JalsaDbContext _context;

    public TherapistAiChatHub(JalsaDbContext context)
    {
        _context = context;
    }

    public async Task JoinConversation(string conversationId)
    {
        var userIdClaim = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? Context.User?.FindFirstValue("sub");

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new HubException("Unauthorized: invalid user identity.");

        if (!Guid.TryParse(conversationId, out var convGuid))
            throw new HubException("Invalid conversation ID.");

        var therapist = await _context.Therapists.FirstOrDefaultAsync(t => t.UserId == userId);
        if (therapist is null)
            throw new HubException("غير مصرح لك بالوصول إلى هذه المحادثة");

        var conversation = await _context.TherapistAiConversations
            .FirstOrDefaultAsync(c => c.Id == convGuid);

        if (conversation is null)
            throw new HubException("Conversation not found.");

        if (conversation.TherapistId != therapist.Id)
            throw new HubException("غير مصرح لك بالوصول إلى هذه المحادثة");

        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
    }
}
