using Jalsa.Application.Interfaces.Services;

namespace Jalsa.Infrastructure.Services;

public class EmailNotificationService : INotificationService
{
    public Task SendExerciseReminderAsync(Guid patientId, string exerciseDescription, DateOnly dueDate)
    {
        Console.WriteLine($"Reminder: Patient {patientId} - Exercise '{exerciseDescription}' due {dueDate}");
        return Task.CompletedTask;
    }
}
