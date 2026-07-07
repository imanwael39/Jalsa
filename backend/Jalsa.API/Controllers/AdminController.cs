using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : BaseController
{
    private readonly IUserManagementService _userManagementService;

    public AdminController(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilterDto filter)
    {
        var result = await _userManagementService.GetUsersAsync(filter);
        return Ok(result);
    }

    [HttpPatch("users/{id:guid}/status")]
    public async Task<IActionResult> SetActive(Guid id, [FromBody] bool isActive)
    {
        var result = await _userManagementService.SetActiveAsync(id, isActive, GetCurrentUserId(), GetClientIpAddress(), GetUserAgent());
        if (result is null)
            return NotFound(new { message = "المستخدم غير موجود" });

        return Ok(result);
    }

    [HttpPatch("users/{id:guid}/unlock")]
    public async Task<IActionResult> Unlock(Guid id)
    {
        var result = await _userManagementService.UnlockAsync(id, GetCurrentUserId(), GetClientIpAddress(), GetUserAgent());
        if (result is null)
            return NotFound(new { message = "المستخدم غير موجود" });

        return Ok(result);
    }

    [HttpPatch("users/{id:guid}/role")]
    public async Task<IActionResult> ChangeRole(Guid id, [FromBody] ChangeRoleDto dto)
    {
        var result = await _userManagementService.ChangeRoleAsync(id, dto.RoleName, GetCurrentUserId(), GetClientIpAddress(), GetUserAgent());
        if (result is null)
            return NotFound(new { message = "المستخدم غير موجود" });

        return Ok(result);
    }

    [HttpDelete("users/{id:guid}")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var result = await _userManagementService.SoftDeleteAsync(id, GetCurrentUserId(), GetClientIpAddress(), GetUserAgent());
        if (result is null)
            return NotFound(new { message = "المستخدم غير موجود" });

        return Ok(result);
    }

    [HttpPatch("users/{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _userManagementService.RestoreAsync(id, GetCurrentUserId(), GetClientIpAddress(), GetUserAgent());
        if (result is null)
            return NotFound(new { message = "المستخدم غير موجود" });

        return Ok(result);
    }

    [HttpGet("users/{id:guid}/audit-logs")]
    public async Task<IActionResult> GetUserAuditLogs(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _userManagementService.GetUserAuditHistoryAsync(id, page, pageSize);
        return Ok(result);
    }
}
