using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.Application.DTOs.Notification;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class NotificationControllerTests
{
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly NotificationController _sut;
    private readonly Guid _userId = Guid.NewGuid();

    public NotificationControllerTests()
    {
        _notificationServiceMock = new Mock<INotificationService>();
        _sut = new NotificationController(_notificationServiceMock.Object);

        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _userId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };
    }

    // --- GetNotifications ---

    [Fact]
    public async Task GetNotifications_ReturnsOkWithList()
    {
        var listDto = new NotificationListDto
        {
            Notifications = new[]
            {
                new NotificationViewDto { Id = Guid.NewGuid(), Type = "Exercise", Title = "تمرين جديد", IsRead = false, CreatedAt = DateTime.UtcNow },
                new NotificationViewDto { Id = Guid.NewGuid(), Type = "Session", Title = "جلسة قادمة", IsRead = true, CreatedAt = DateTime.UtcNow }
            },
            UnreadCount = 1
        };
        _notificationServiceMock.Setup(x => x.GetNotificationsAsync(_userId, false)).ReturnsAsync(listDto);

        var result = await _sut.GetNotifications();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var data = ok.Value.Should().BeOfType<NotificationListDto>().Subject;
        data.Notifications.Should().HaveCount(2);
        data.UnreadCount.Should().Be(1);
    }

    [Fact]
    public async Task GetNotifications_UnreadOnly_PassesFilterToService()
    {
        var listDto = new NotificationListDto
        {
            Notifications = new[] { new NotificationViewDto { Id = Guid.NewGuid(), Type = "Exercise", Title = "تمرين", IsRead = false, CreatedAt = DateTime.UtcNow } },
            UnreadCount = 1
        };
        _notificationServiceMock.Setup(x => x.GetNotificationsAsync(_userId, true)).ReturnsAsync(listDto);

        var result = await _sut.GetNotifications(unreadOnly: true);

        result.Should().BeOfType<OkObjectResult>();
        _notificationServiceMock.Verify(x => x.GetNotificationsAsync(_userId, true), Times.Once);
    }

    [Fact]
    public async Task GetNotifications_Empty_ReturnsOkWithEmptyList()
    {
        var listDto = new NotificationListDto { Notifications = Enumerable.Empty<NotificationViewDto>(), UnreadCount = 0 };
        _notificationServiceMock.Setup(x => x.GetNotificationsAsync(_userId, false)).ReturnsAsync(listDto);

        var result = await _sut.GetNotifications();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var data = ok.Value.Should().BeOfType<NotificationListDto>().Subject;
        data.Notifications.Should().BeEmpty();
        data.UnreadCount.Should().Be(0);
    }

    // --- MarkAsRead ---

    [Fact]
    public async Task MarkAsRead_ExistingNotification_ReturnsOk()
    {
        var notifId = Guid.NewGuid();
        _notificationServiceMock.Setup(x => x.MarkAsReadAsync(notifId, _userId))
            .ReturnsAsync(new NotificationViewDto { Id = notifId, IsRead = true, ReadAt = DateTime.UtcNow, Type = "Exercise", Title = "تمرين", CreatedAt = DateTime.UtcNow });

        var result = await _sut.MarkAsRead(notifId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task MarkAsRead_NonExistent_Returns404()
    {
        _notificationServiceMock.Setup(x => x.MarkAsReadAsync(It.IsAny<Guid>(), _userId))
            .ReturnsAsync((NotificationViewDto?)null);

        var result = await _sut.MarkAsRead(Guid.NewGuid());

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    // --- MarkAllAsRead ---

    [Fact]
    public async Task MarkAllAsRead_ReturnsOkWithCount()
    {
        _notificationServiceMock.Setup(x => x.MarkAllAsReadAsync(_userId)).ReturnsAsync(5);

        var result = await _sut.MarkAllAsRead();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task MarkAllAsRead_NoneUnread_ReturnsZero()
    {
        _notificationServiceMock.Setup(x => x.MarkAllAsReadAsync(_userId)).ReturnsAsync(0);

        var result = await _sut.MarkAllAsRead();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().NotBeNull();
    }
}
