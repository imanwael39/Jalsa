namespace Jalsa.Application.Interfaces.Services;

public interface INotificationService
{
    Task SendExerciseReminderAsync(Guid patientId, string exerciseDescription, DateOnly dueDate);
    Task CreateInAppNotificationAsync(Guid recipientUserId, string type, string title, string? body = null);
}
