using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

/// <summary>
/// Account-level patient administration only. Admin never touches sessions, exercises,
/// notes, or reports through this controller — that stays behind the Therapist-only
/// PatientController/PatientService.
/// </summary>
[ApiController]
[Route("api/admin/patients")]
[Authorize(Roles = "Admin")]
public class AdminPatientsController : BaseController
{
    private readonly IPatientAccountAdminService _patientAccountAdminService;

    public AdminPatientsController(IPatientAccountAdminService patientAccountAdminService)
    {
        _patientAccountAdminService = patientAccountAdminService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPatientAccounts([FromQuery] PatientAccountFilterDto filter)
    {
        var result = await _patientAccountAdminService.GetPatientAccountsAsync(filter);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/disable")]
    public async Task<IActionResult> Disable(Guid id)
    {
        var result = await _patientAccountAdminService.DisableAccountAsync(id, GetCurrentUserId(), GetClientIpAddress(), GetUserAgent());
        if (result is null)
            return NotFound(new { message = "المريض غير موجود" });

        return Ok(result);
    }

    [HttpPatch("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var result = await _patientAccountAdminService.RestoreAccountAsync(id, GetCurrentUserId(), GetClientIpAddress(), GetUserAgent());
        if (result is null)
            return NotFound(new { message = "المريض غير موجود" });

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _patientAccountAdminService.DeleteAccountAsync(id, GetCurrentUserId(), GetClientIpAddress(), GetUserAgent());
        if (result is null)
            return NotFound(new { message = "المريض غير موجود" });

        return Ok(result);
    }
}
