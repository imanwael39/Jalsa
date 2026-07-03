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

    public ProgressController(IProgressService progressService)
    {
        _progressService = progressService;
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
        catch (Exception)
        {
            return StatusCode(500, new { error = "حدث خطأ أثناء تحميل بيانات لوحة التحكم" });
        }
    }
}
