using Jalsa.Application.DTOs.Notification;

namespace Jalsa.Application.Interfaces.Services;

public interface INotificationService
{
    Task SendExerciseReminderAsync(Guid patientId, string exerciseDescription, DateOnly dueDate);
    Task CreateInAppNotificationAsync(Guid recipientUserId, string type, string title, string? body = null);
    Task<NotificationListDto> GetNotificationsAsync(Guid userId, bool unreadOnly = false);
    Task<NotificationViewDto?> MarkAsReadAsync(Guid notificationId, Guid userId);
    Task<int> MarkAllAsReadAsync(Guid userId);
}
