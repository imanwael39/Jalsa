using FluentAssertions;
using Jalsa.Application.DTOs.Notification;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Patient;
using Jalsa.Infrastructure.Data;
using Jalsa.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Jalsa.Tests;

public class EmailNotificationServiceTests
{
    private readonly JalsaDbContext _context;
    private readonly Mock<INotificationPushService> _pushServiceMock;
    private readonly EmailNotificationService _sut;

    private readonly Guid _userId = Guid.NewGuid();

    public EmailNotificationServiceTests()
    {
        var options = new DbContextOptionsBuilder<JalsaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new JalsaDbContext(options);
        _pushServiceMock = new Mock<INotificationPushService>();
        _sut = new EmailNotificationService(_context, _pushServiceMock.Object);
    }

    [Fact]
    public async Task CreateInAppNotificationAsync_PersistsNotification()
    {
        await _sut.CreateInAppNotificationAsync(_userId, "ExerciseReminder", "تذكير بالتمرين", "تفاصيل");

        var saved = await _context.Notifications.SingleAsync();
        saved.RecipientUserId.Should().Be(_userId);
        saved.Type.Should().Be("ExerciseReminder");
        saved.Title.Should().Be("تذكير بالتمرين");
        saved.Body.Should().Be("تفاصيل");
        saved.IsRead.Should().BeFalse();
    }

    [Fact]
    public async Task CreateInAppNotificationAsync_PushesToRecipientAfterSave()
    {
        await _sut.CreateInAppNotificationAsync(_userId, "ExerciseReminder", "تذكير بالتمرين");

        _pushServiceMock.Verify(
            x => x.PushToUserAsync(
                _userId,
                It.Is<NotificationViewDto>(n => n.Type == "ExerciseReminder" && n.Title == "تذكير بالتمرين")),
            Times.Once);
    }

    [Fact]
    public async Task CreateInAppNotificationAsync_PushFailure_DoesNotThrow_NotificationStillPersisted()
    {
        _pushServiceMock
            .Setup(x => x.PushToUserAsync(It.IsAny<Guid>(), It.IsAny<NotificationViewDto>()))
            .ThrowsAsync(new InvalidOperationException("hub unreachable"));

        var act = () => _sut.CreateInAppNotificationAsync(_userId, "ExerciseReminder", "تذكير بالتمرين");

        await act.Should().NotThrowAsync();
        (await _context.Notifications.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task SendExerciseReminderAsync_PatientHasUser_CreatesAndPushesNotification()
    {
        var patientId = Guid.NewGuid();
        _context.Patients.Add(new Patient
        {
            Id = patientId,
            UserId = _userId,
            TherapistId = Guid.NewGuid(),
            FullName = "سارة أحمد"
        });
        await _context.SaveChangesAsync();

        await _sut.SendExerciseReminderAsync(patientId, "تمرين التنفس", DateOnly.FromDateTime(DateTime.UtcNow));

        (await _context.Notifications.CountAsync()).Should().Be(1);
        _pushServiceMock.Verify(x => x.PushToUserAsync(_userId, It.IsAny<NotificationViewDto>()), Times.Once);
    }

    [Fact]
    public async Task SendExerciseReminderAsync_PatientHasNoUser_DoesNotCreateNotification()
    {
        var patientId = Guid.NewGuid();
        _context.Patients.Add(new Patient
        {
            Id = patientId,
            UserId = null,
            TherapistId = Guid.NewGuid(),
            FullName = "سارة أحمد"
        });
        await _context.SaveChangesAsync();

        await _sut.SendExerciseReminderAsync(patientId, "تمرين التنفس", DateOnly.FromDateTime(DateTime.UtcNow));

        (await _context.Notifications.CountAsync()).Should().Be(0);
        _pushServiceMock.Verify(x => x.PushToUserAsync(It.IsAny<Guid>(), It.IsAny<NotificationViewDto>()), Times.Never);
    }
}
