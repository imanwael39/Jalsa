using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.Assessment;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/patient/{patientId:guid}/assessments")]
[Authorize(Roles = "Therapist")]
public class AssessmentController : BaseController
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

    [HttpPost("assign")]
    public async Task<IActionResult> Assign(Guid patientId, [FromBody] AssessmentAssignDto dto)
    {
        var therapistId = GetCurrentUserId();
        var result = await _assessmentService.AssignAsync(patientId, dto, therapistId);
        return CreatedAtAction(nameof(GetByPatientId), new { patientId }, result);
    }
}
