namespace Jalsa.Application.Interfaces.Services;

public interface INotificationService
{
    Task SendExerciseReminderAsync(Guid patientId, string exerciseDescription, DateOnly dueDate);
    Task SendCrisisAlertAsync(Guid patientId, string severity, string messageSnippet);
}
