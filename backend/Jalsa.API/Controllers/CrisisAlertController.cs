using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/crisis-alerts")]
[Authorize(Roles = "Therapist")]
public class CrisisAlertController : BaseController
{
    private readonly ICrisisAlertService _crisisAlertService;

    public CrisisAlertController(ICrisisAlertService crisisAlertService)
    {
        _crisisAlertService = crisisAlertService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAlerts([FromQuery] bool openOnly = false)
    {
        var userId = GetCurrentUserId();
        var result = await _crisisAlertService.GetAlertsAsync(userId, openOnly);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/resolve")]
    public async Task<IActionResult> Resolve(Guid id)
    {
        var userId = GetCurrentUserId();
        var result = await _crisisAlertService.ResolveAsync(userId, id);

        if (result is null)
            return NotFound(new { message = "التنبيه غير موجود" });

        return Ok(result);
    }
}
