using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.DTOs.Admin;
using Jalsa.API.Services.Interfaces;

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
    public async Task<IActionResult> GetUsers()
    {
        var result = await _userManagementService.GetUsersAsync();
        return Ok(result);
    }

    [HttpPatch("users/{id:guid}/status")]
    public async Task<IActionResult> SetActive(Guid id, [FromBody] bool isActive)
    {
        var result = await _userManagementService.SetActiveAsync(id, isActive);
        if (result is null)
            return NotFound(new { message = "المستخدم غير موجود" });

        return Ok(result);
    }

    [HttpPatch("users/{id:guid}/unlock")]
    public async Task<IActionResult> Unlock(Guid id)
    {
        var result = await _userManagementService.UnlockAsync(id);
        if (result is null)
            return NotFound(new { message = "المستخدم غير موجود" });

        return Ok(result);
    }

    [HttpPatch("users/{id:guid}/role")]
    public async Task<IActionResult> ChangeRole(Guid id, [FromBody] ChangeRoleDto dto)
    {
        var result = await _userManagementService.ChangeRoleAsync(id, dto.RoleName);
        if (result is null)
            return NotFound(new { message = "المستخدم غير موجود" });

        return Ok(result);
    }
}
