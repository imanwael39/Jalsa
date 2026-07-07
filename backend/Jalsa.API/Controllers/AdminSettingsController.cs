using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/admin/settings")]
[Authorize(Roles = "Admin")]
public class AdminSettingsController : BaseController
{
    private readonly ISystemSettingsService _systemSettingsService;

    public AdminSettingsController(ISystemSettingsService systemSettingsService)
    {
        _systemSettingsService = systemSettingsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSettings()
    {
        var result = await _systemSettingsService.GetSettingsAsync();
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSettings([FromBody] SystemSettingsDto dto)
    {
        var result = await _systemSettingsService.UpdateSettingsAsync(dto, GetCurrentUserId(), GetClientIpAddress(), GetUserAgent());
        return Ok(result);
    }
}
