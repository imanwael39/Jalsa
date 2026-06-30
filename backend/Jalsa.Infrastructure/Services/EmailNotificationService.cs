using Jalsa.Application.DTOs.Notification;
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
        // In-app notification created below; SMTP delivery not yet configured

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

    public async Task<NotificationListDto> GetNotificationsAsync(Guid userId, bool unreadOnly = false)
    {
        var query = _context.Notifications
            .Where(n => n.RecipientUserId == userId);

        if (unreadOnly)
            query = query.Where(n => !n.IsRead);

        var notifications = await query
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .Select(n => new NotificationViewDto
            {
                Id = n.Id,
                Type = n.Type,
                Title = n.Title,
                Body = n.Body,
                IsRead = n.IsRead,
                ReadAt = n.ReadAt,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync();

        var unreadCount = await _context.Notifications
            .CountAsync(n => n.RecipientUserId == userId && !n.IsRead);

        return new NotificationListDto
        {
            Notifications = notifications,
            UnreadCount = unreadCount
        };
    }

    public async Task<NotificationViewDto?> MarkAsReadAsync(Guid notificationId, Guid userId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.RecipientUserId == userId);

        if (notification is null)
            return null;

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return new NotificationViewDto
        {
            Id = notification.Id,
            Type = notification.Type,
            Title = notification.Title,
            Body = notification.Body,
            IsRead = notification.IsRead,
            ReadAt = notification.ReadAt,
            CreatedAt = notification.CreatedAt
        };
    }

    public async Task<int> MarkAllAsReadAsync(Guid userId)
    {
        var unread = await _context.Notifications
            .Where(n => n.RecipientUserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var n in unread)
        {
            n.IsRead = true;
            n.ReadAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return unread.Count;
    }
}
