using Jalsa.Application.Interfaces.Services;
using Jalsa.Infrastructure.Data;
using Jalsa.Domain.Models.Notification;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.Infrastructure.Services;

public class EmailNotificationService : INotificationService
{
    private readonly JalsaDbContext _context;

    public EmailNotificationService(JalsaDbContext context)
    {
        _context = context;
    }

    public async Task SendExerciseReminderAsync(Guid patientId, string exerciseDescription, DateOnly dueDate)
    {
        Console.WriteLine($"Reminder: Patient {patientId} - Exercise '{exerciseDescription}' due {dueDate}");

        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == patientId);
        if (patient?.UserId is not null)
        {
            await CreateInAppNotificationAsync(
                patient.UserId.Value,
                "ExerciseReminder",
                "تذكير بالتمرين",
                $"يُرجى إكمال تمرين '{exerciseDescription}' بحلول {dueDate.ToString("dd/MM/yyyy")}");
        }
    }

    public async Task CreateInAppNotificationAsync(Guid recipientUserId, string type, string title, string? body = null)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            RecipientUserId = recipientUserId,
            Type = type,
            Title = title,
            Body = body,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }
}
