using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.PatientSession;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/patient-sessions")]
[Authorize(Roles = "Patient")]
public class PatientSessionController : BaseController
{
    private readonly IPatientSessionService _patientSessionService;

    public PatientSessionController(IPatientSessionService patientSessionService)
    {
        _patientSessionService = patientSessionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _patientSessionService.GetListAsync(GetCurrentUserId());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var result = await _patientSessionService.GetDetailAsync(GetCurrentUserId(), id);
        return Ok(result);
    }

    [HttpPost("{id:guid}/request-reschedule")]
    public async Task<IActionResult> RequestReschedule(Guid id, [FromBody] SessionChangeRequestDto dto)
    {
        await _patientSessionService.RequestRescheduleAsync(GetCurrentUserId(), id, dto);
        return NoContent();
    }

    [HttpPost("{id:guid}/request-cancel")]
    public async Task<IActionResult> RequestCancel(Guid id, [FromBody] SessionChangeRequestDto dto)
    {
        await _patientSessionService.RequestCancelAsync(GetCurrentUserId(), id, dto);
        return NoContent();
    }
}
