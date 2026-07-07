using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Identity;
using Moq;

namespace Jalsa.Tests;

public class AdminUserServiceTests
{
    private readonly List<User> _users = [];
    private readonly List<Role> _roles = [];
    private readonly List<UserRole> _userRoles = [];
    private readonly List<RefreshToken> _refreshTokens = [];

    private readonly Mock<IGenericRepository<User>> _userRepoMock;
    private readonly Mock<IGenericRepository<Role>> _roleRepoMock;
    private readonly Mock<IGenericRepository<UserRole>> _userRoleRepoMock;
    private readonly Mock<IGenericRepository<RefreshToken>> _refreshTokenRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAuditLogService> _auditLogServiceMock;
    private readonly UserManagementService _sut;

    public AdminUserServiceTests()
    {
        _userRepoMock = new Mock<IGenericRepository<User>>();
        _userRepoMock.Setup(r => r.Query()).Returns(() => _users.AsQueryable());
        _userRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<User, bool>> predicate, CancellationToken _) => _users.AsQueryable().FirstOrDefault(predicate));

        _roleRepoMock = new Mock<IGenericRepository<Role>>();
        _roleRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Role, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<Role, bool>> predicate, CancellationToken _) => _roles.AsQueryable().FirstOrDefault(predicate));

        _userRoleRepoMock = new Mock<IGenericRepository<UserRole>>();
        _userRoleRepoMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<UserRole, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<UserRole, bool>> predicate, CancellationToken _) => _userRoles.AsQueryable().Where(predicate).ToList());
        _userRoleRepoMock
            .Setup(r => r.AddAsync(It.IsAny<UserRole>(), It.IsAny<CancellationToken>()))
            .Callback<UserRole, CancellationToken>((ur, _) => _userRoles.Add(ur))
            .Returns(Task.CompletedTask);
        _userRoleRepoMock
            .Setup(r => r.RemoveRange(It.IsAny<IEnumerable<UserRole>>()))
            .Callback<IEnumerable<UserRole>>(rows => { foreach (var r in rows.ToList()) _userRoles.Remove(r); });

        _refreshTokenRepoMock = new Mock<IGenericRepository<RefreshToken>>();
        _refreshTokenRepoMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<RefreshToken, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<RefreshToken, bool>> predicate, CancellationToken _) => _refreshTokens.AsQueryable().Where(predicate).ToList());

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(_userRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Role>()).Returns(_roleRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<UserRole>()).Returns(_userRoleRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<RefreshToken>()).Returns(_refreshTokenRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _auditLogServiceMock = new Mock<IAuditLogService>();
        _auditLogServiceMock
            .Setup(a => a.GetLogsForUserAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new PagedResultDto<AuditLogViewDto> { Items = [], TotalCount = 0 });

        _sut = new UserManagementService(_unitOfWorkMock.Object, _auditLogServiceMock.Object);
    }

    private User AddUser(bool isActive = true, bool isDeleted = false, string? email = null)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email ?? $"{Guid.NewGuid()}@test.com",
            PasswordHash = "x",
            IsActive = isActive,
            IsDeleted = isDeleted,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _users.Add(user);
        return user;
    }

    [Fact]
    public async Task GetUsersAsync_ExcludesDeletedByDefault()
    {
        AddUser();
        AddUser(isDeleted: true);

        var result = await _sut.GetUsersAsync(new UserFilterDto());

        result.TotalCount.Should().Be(1);
        result.Items.Should().OnlyContain(u => !u.IsDeleted);
    }

    [Fact]
    public async Task GetUsersAsync_IncludeDeletedTrue_ReturnsAll()
    {
        AddUser();
        AddUser(isDeleted: true);

        var result = await _sut.GetUsersAsync(new UserFilterDto { IncludeDeleted = true });

        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetUsersAsync_FiltersByIsActiveAndPaginates()
    {
        for (var i = 0; i < 5; i++) AddUser(isActive: true);
        for (var i = 0; i < 3; i++) AddUser(isActive: false);

        var result = await _sut.GetUsersAsync(new UserFilterDto { IsActive = true, Page = 1, PageSize = 2 });

        result.TotalCount.Should().Be(5);
        result.Items.Should().HaveCount(2);
        result.Items.Should().OnlyContain(u => u.IsActive);
    }

    [Fact]
    public async Task GetUsersAsync_SearchTerm_MatchesEmail()
    {
        AddUser(email: "unique-match@test.com");
        AddUser(email: "other@test.com");

        var result = await _sut.GetUsersAsync(new UserFilterDto { SearchTerm = "unique-match" });

        result.Items.Should().ContainSingle(u => u.Email == "unique-match@test.com");
    }

    [Fact]
    public async Task SoftDeleteAsync_SetsIsDeletedAndRevokesActiveRefreshTokens()
    {
        var user = AddUser();
        var activeToken = new RefreshToken { Id = Guid.NewGuid(), UserId = user.Id, Token = "t1", ExpiresAt = DateTime.UtcNow.AddDays(1), CreatedAt = DateTime.UtcNow };
        _refreshTokens.Add(activeToken);

        var result = await _sut.SoftDeleteAsync(user.Id, Guid.NewGuid(), "1.1.1.1", "Agent");

        result.Should().NotBeNull();
        user.IsDeleted.Should().BeTrue();
        activeToken.RevokedAt.Should().NotBeNull();
        _auditLogServiceMock.Verify(a => a.LogAsync(
            It.IsAny<Guid?>(), "User", user.Id.ToString(), "User.SoftDelete",
            null, null, "1.1.1.1", "Agent"), Times.Once);
    }

    [Fact]
    public async Task SoftDeleteAsync_NotFound_ReturnsNull()
    {
        var result = await _sut.SoftDeleteAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task RestoreAsync_ClearsIsDeleted()
    {
        var user = AddUser(isDeleted: true);

        var result = await _sut.RestoreAsync(user.Id);

        result.Should().NotBeNull();
        user.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task GetUserAuditHistoryAsync_DelegatesToAuditLogService()
    {
        var userId = Guid.NewGuid();

        await _sut.GetUserAuditHistoryAsync(userId, 2, 5);

        _auditLogServiceMock.Verify(a => a.GetLogsForUserAsync(userId, 2, 5), Times.Once);
    }

    [Fact]
    public async Task ChangeRoleAsync_InvalidRole_Throws()
    {
        var user = AddUser();

        var act = () => _sut.ChangeRoleAsync(user.Id, "NotARealRole");

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
