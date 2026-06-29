namespace Jalsa.Application.DTOs.Notification;

public class NotificationListDto
{
    public IEnumerable<NotificationViewDto> Notifications { get; set; } = Enumerable.Empty<NotificationViewDto>();
    public int UnreadCount { get; set; }
}
