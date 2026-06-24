using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.Application.Jobs;

public class ExerciseReminderJob
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly INotificationService _notificationService;

    public ExerciseReminderJob(
        IExerciseRepository exerciseRepository,
        INotificationService notificationService)
    {
        _exerciseRepository = exerciseRepository;
        _notificationService = notificationService;
    }

    public async Task SendRemindersAsync()
    {
        var exercises = await _exerciseRepository.GetDueSoonAsync(2);

        foreach (var exercise in exercises)
        {
            if (exercise.DueDate.HasValue)
            {
                await _notificationService.SendExerciseReminderAsync(
                    exercise.PatientId,
                    exercise.Description ?? string.Empty,
                    exercise.DueDate.Value);
            }
        }
    }
}
