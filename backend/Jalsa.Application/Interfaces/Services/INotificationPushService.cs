using Jalsa.Application.DTOs.Notification;

namespace Jalsa.Application.Interfaces.Services;

public interface INotificationPushService
{
    Task PushToUserAsync(Guid recipientUserId, NotificationViewDto notification);
}
