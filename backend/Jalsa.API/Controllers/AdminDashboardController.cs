using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.Services.Interfaces;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminDashboardController : BaseController
{
    private readonly IAdminDashboardService _adminDashboardService;
    private readonly ISystemHealthService _systemHealthService;

    public AdminDashboardController(IAdminDashboardService adminDashboardService, ISystemHealthService systemHealthService)
    {
        _adminDashboardService = adminDashboardService;
        _systemHealthService = systemHealthService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await _adminDashboardService.GetDashboardAsync();
        return Ok(result);
    }

    [HttpGet("system-health")]
    public async Task<IActionResult> GetSystemHealth()
    {
        var result = await _systemHealthService.GetSystemHealthAsync();
        return Ok(result);
    }
}
