using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.Exceptions;
using Jalsa.Application.DTOs.Assessment;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/patient/{patientId:guid}/assessments")]
[Authorize(Roles = "Therapist")]
public class AssessmentController : ControllerBase
{
    private readonly IAssessmentService _assessmentService;

    public AssessmentController(IAssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var therapistId = GetCurrentUserId();
        var result = await _assessmentService.GetByPatientIdAsync(patientId, therapistId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Guid patientId, [FromBody] AssessmentCreateDto dto)
    {
        var therapistId = GetCurrentUserId();
        var result = await _assessmentService.CreateAsync(patientId, dto, therapistId);
        return CreatedAtAction(nameof(GetByPatientId), new { patientId }, result);
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

        if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var userId))
            throw new ApiException(401, "Invalid authentication token");

        return userId;
    }
}
