using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/patient-progress")]
[EnableRateLimiting("general")]
public class PatientProgressController : BaseController
{
    private readonly IPatientProgressService _patientProgressService;
    private readonly ILogger<PatientProgressController> _logger;

    public PatientProgressController(IPatientProgressService patientProgressService, ILogger<PatientProgressController> logger)
    {
        _patientProgressService = patientProgressService;
        _logger = logger;
    }

    [Authorize(Roles = "Patient")]
    [HttpGet]
    public async Task<IActionResult> GetProgress([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        try
        {
            var result = await _patientProgressService.GetProgressAsync(GetCurrentUserId(), from, to);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Patient progress load failed for user {UserId}", GetCurrentUserId());
            return StatusCode(500, new { error = "حدث خطأ أثناء تحميل بيانات التقدم" });
        }
    }
}
