using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.Intake;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Authorize(Roles = "Therapist")]
public class IntakeController : BaseController
{
    private readonly IIntakeService _intakeService;

    public IntakeController(IIntakeService intakeService)
    {
        _intakeService = intakeService;
    }

    [HttpGet("api/patient/{patientId:guid}/intake")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var therapistId = GetCurrentUserId();
        var result = await _intakeService.GetByPatientIdAsync(patientId, therapistId);
        return Ok(result);
    }

    [HttpPost("api/patient/{patientId:guid}/intake")]
    public async Task<IActionResult> Save(Guid patientId, [FromBody] IntakeFormSaveDto dto)
    {
        var therapistId = GetCurrentUserId();
        var result = await _intakeService.SaveAsync(patientId, dto, therapistId);
        return Ok(result);
    }

    [HttpPost("api/patient/{patientId:guid}/intake/submit")]
    public async Task<IActionResult> Submit(Guid patientId)
    {
        var therapistId = GetCurrentUserId();
        var result = await _intakeService.SubmitAsync(patientId, therapistId);
        return Ok(result);
    }
}
