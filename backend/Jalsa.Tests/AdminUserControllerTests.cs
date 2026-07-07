using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class AdminUserControllerTests
{
    private readonly Mock<IUserManagementService> _userManagementServiceMock;
    private readonly AdminController _sut;
    private readonly Guid _adminUserId;

    public AdminUserControllerTests()
    {
        _userManagementServiceMock = new Mock<IUserManagementService>();
        _sut = new AdminController(_userManagementServiceMock.Object);

        _adminUserId = Guid.NewGuid();
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _adminUserId.ToString()) };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));
        _sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = principal } };
    }

    [Fact]
    public async Task GetUsers_ReturnsPagedResultFromService()
    {
        var expected = new PagedResultDto<UserAdminViewDto> { TotalCount = 1, Items = [new UserAdminViewDto { Email = "a@test.com" }] };
        _userManagementServiceMock.Setup(s => s.GetUsersAsync(It.IsAny<UserFilterDto>())).ReturnsAsync(expected);

        var result = await _sut.GetUsers(new UserFilterDto());

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().Be(expected);
    }

    [Fact]
    public async Task SetActive_UserNotFound_Returns404()
    {
        _userManagementServiceMock
            .Setup(s => s.SetActiveAsync(It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<Guid?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync((UserAdminViewDto?)null);

        var result = await _sut.SetActive(Guid.NewGuid(), true);

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task SetActive_Found_PassesActingAdminUserIdFromClaims()
    {
        var userId = Guid.NewGuid();
        _userManagementServiceMock
            .Setup(s => s.SetActiveAsync(userId, false, _adminUserId, It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new UserAdminViewDto { Id = userId });

        var result = await _sut.SetActive(userId, false);

        result.Should().BeOfType<OkObjectResult>();
        _userManagementServiceMock.Verify(s => s.SetActiveAsync(userId, false, _adminUserId, It.IsAny<string?>(), It.IsAny<string?>()), Times.Once);
    }

    [Fact]
    public async Task ChangeRole_UserNotFound_Returns404()
    {
        _userManagementServiceMock
            .Setup(s => s.ChangeRoleAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync((UserAdminViewDto?)null);

        var result = await _sut.ChangeRole(Guid.NewGuid(), new ChangeRoleDto { RoleName = "Admin" });

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task SoftDelete_Found_ReturnsOk()
    {
        var userId = Guid.NewGuid();
        _userManagementServiceMock
            .Setup(s => s.SoftDeleteAsync(userId, _adminUserId, It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new UserAdminViewDto { Id = userId, IsDeleted = true });

        var result = await _sut.SoftDelete(userId);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Restore_NotFound_Returns404()
    {
        _userManagementServiceMock
            .Setup(s => s.RestoreAsync(It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync((UserAdminViewDto?)null);

        var result = await _sut.Restore(Guid.NewGuid());

        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetUserAuditLogs_DelegatesPageAndPageSizeToService()
    {
        var userId = Guid.NewGuid();
        var expected = new PagedResultDto<AuditLogViewDto> { TotalCount = 0 };
        _userManagementServiceMock.Setup(s => s.GetUserAuditHistoryAsync(userId, 2, 5)).ReturnsAsync(expected);

        var result = await _sut.GetUserAuditLogs(userId, page: 2, pageSize: 5);

        result.Should().BeOfType<OkObjectResult>();
        _userManagementServiceMock.Verify(s => s.GetUserAuditHistoryAsync(userId, 2, 5), Times.Once);
    }
}
