using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/progress")]
[EnableRateLimiting("general")]
public class ProgressController : BaseController
{
    private readonly IProgressService _progressService;
    private readonly ILogger<ProgressController> _logger;

    public ProgressController(IProgressService progressService, ILogger<ProgressController> logger)
    {
        _progressService = progressService;
        _logger = logger;
    }

    [Authorize(Roles = "Therapist")]
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        try
        {
            var result = await _progressService.GetDashboardAsync(GetCurrentUserId());
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dashboard load failed for user {UserId}", GetCurrentUserId());
            return StatusCode(500, new { error = "حدث خطأ أثناء تحميل بيانات لوحة التحكم" });
        }
    }
}
