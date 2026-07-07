using FluentAssertions;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Audit;
using Jalsa.Domain.Models.Identity;
using Moq;

namespace Jalsa.Tests;

public class AuditLogServiceTests
{
    private readonly List<AuditLog> _logs = [];
    private readonly Mock<IGenericRepository<AuditLog>> _auditLogRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AuditLogService _sut;

    public AuditLogServiceTests()
    {
        _auditLogRepoMock = new Mock<IGenericRepository<AuditLog>>();
        _auditLogRepoMock.Setup(r => r.Query()).Returns(() => _logs.AsQueryable());
        _auditLogRepoMock
            .Setup(r => r.AddAsync(It.IsAny<AuditLog>(), It.IsAny<CancellationToken>()))
            .Callback<AuditLog, CancellationToken>((log, _) => _logs.Add(log))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(u => u.Repository<AuditLog>()).Returns(_auditLogRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _sut = new AuditLogService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task LogAsync_SerializesOldAndNewValuesAsJson()
    {
        var userId = Guid.NewGuid();

        await _sut.LogAsync(userId, "User", userId.ToString(), "User.SetActive",
            oldValues: new { IsActive = true }, newValues: new { IsActive = false });

        _logs.Should().ContainSingle();
        var log = _logs.Single();
        log.OldValues.Should().Contain("true");
        log.NewValues.Should().Contain("false");
        log.Action.Should().Be("User.SetActive");
        log.EntityName.Should().Be("User");
    }

    [Fact]
    public async Task GetLogsAsync_FiltersByEntityNameAndPaginates()
    {
        for (var i = 0; i < 5; i++)
            _logs.Add(new AuditLog { Id = Guid.NewGuid(), EntityName = "User", Action = $"Action{i}", OccurredAt = DateTime.UtcNow.AddMinutes(-i) });

        _logs.Add(new AuditLog { Id = Guid.NewGuid(), EntityName = "Settings", Action = "Settings.Update", OccurredAt = DateTime.UtcNow });

        var result = await _sut.GetLogsAsync(new AuditLogFilterDto { EntityName = "User", Page = 1, PageSize = 3 });

        result.TotalCount.Should().Be(5);
        result.Items.Should().HaveCount(3);
        result.Items.Should().OnlyContain(l => l.EntityName == "User");
    }

    [Fact]
    public async Task GetLogsAsync_OrdersByOccurredAtDescending_AndIncludesUserEmail()
    {
        var user = new User { Id = Guid.NewGuid(), Email = "a@test.com", PasswordHash = "x", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        _logs.Add(new AuditLog { Id = Guid.NewGuid(), UserId = user.Id, User = user, EntityName = "User", Action = "First", OccurredAt = DateTime.UtcNow.AddMinutes(-5) });
        _logs.Add(new AuditLog { Id = Guid.NewGuid(), UserId = user.Id, User = user, EntityName = "User", Action = "Second", OccurredAt = DateTime.UtcNow });

        var result = await _sut.GetLogsAsync(new AuditLogFilterDto());

        result.Items.First().Action.Should().Be("Second");
        result.Items.First().UserEmail.Should().Be("a@test.com");
    }

    [Fact]
    public async Task GetLogsForUserAsync_FiltersByUserId()
    {
        var userId = Guid.NewGuid();
        _logs.Add(new AuditLog { Id = Guid.NewGuid(), UserId = userId, EntityName = "User", Action = "A", OccurredAt = DateTime.UtcNow });
        _logs.Add(new AuditLog { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), EntityName = "User", Action = "B", OccurredAt = DateTime.UtcNow });

        var result = await _sut.GetLogsForUserAsync(userId, 1, 10);

        result.Items.Should().ContainSingle();
        result.Items.Single().UserId.Should().Be(userId);
    }

    [Fact]
    public async Task GetLogsForUserAsync_AlsoIncludesActionsAnAdminTookOnThisUsersAccount()
    {
        var userId = Guid.NewGuid();
        var adminId = Guid.NewGuid();

        // The admin performed the action, so UserId = adminId — but it targets this
        // user's account (EntityName="User", EntityId=userId), so it must still show up
        // in this user's history even though UserId doesn't match.
        _logs.Add(new AuditLog { Id = Guid.NewGuid(), UserId = adminId, EntityName = "User", EntityId = userId.ToString(), Action = "User.SetActive", OccurredAt = DateTime.UtcNow });
        _logs.Add(new AuditLog { Id = Guid.NewGuid(), UserId = adminId, EntityName = "User", EntityId = Guid.NewGuid().ToString(), Action = "User.SetActive", OccurredAt = DateTime.UtcNow });

        var result = await _sut.GetLogsForUserAsync(userId, 1, 10);

        result.Items.Should().ContainSingle(l => l.EntityId == userId.ToString());
    }
}
