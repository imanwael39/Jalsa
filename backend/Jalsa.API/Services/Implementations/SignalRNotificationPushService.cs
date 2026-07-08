using Jalsa.API.Hubs;
using Jalsa.Application.DTOs.Notification;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace Jalsa.API.Services.Implementations;

public class SignalRNotificationPushService : INotificationPushService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationPushService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task PushToUserAsync(Guid recipientUserId, NotificationViewDto notification)
    {
        await _hubContext.Clients
            .Group(recipientUserId.ToString())
            .SendAsync("ReceiveNotification", notification);
    }
}
