using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Jalsa.API.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userIdClaim = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? Context.User?.FindFirstValue("sub");

        if (!string.IsNullOrEmpty(userIdClaim))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userIdClaim);
        }

        await base.OnConnectedAsync();
    }
}
