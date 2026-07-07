using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/patient-dashboard")]
[EnableRateLimiting("general")]
public class PatientDashboardController : BaseController
{
    private readonly IPatientDashboardService _patientDashboardService;
    private readonly ILogger<PatientDashboardController> _logger;

    public PatientDashboardController(IPatientDashboardService patientDashboardService, ILogger<PatientDashboardController> logger)
    {
        _patientDashboardService = patientDashboardService;
        _logger = logger;
    }

    [Authorize(Roles = "Patient")]
    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        try
        {
            var result = await _patientDashboardService.GetDashboardAsync(GetCurrentUserId());
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Patient dashboard load failed for user {UserId}", GetCurrentUserId());
            return StatusCode(500, new { error = "حدث خطأ أثناء تحميل بيانات لوحة التحكم" });
        }
    }
}
