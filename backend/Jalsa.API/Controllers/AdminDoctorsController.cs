using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/admin/doctors")]
[Authorize(Roles = "Admin")]
public class AdminDoctorsController : BaseController
{
    private readonly ITherapistAdminService _therapistAdminService;

    public AdminDoctorsController(ITherapistAdminService therapistAdminService)
    {
        _therapistAdminService = therapistAdminService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDoctors([FromQuery] TherapistFilterDto filter)
    {
        var result = await _therapistAdminService.GetTherapistsAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDoctorDetail(Guid id)
    {
        var result = await _therapistAdminService.GetTherapistDetailAsync(id);
        if (result is null)
            return NotFound(new { message = "المعالج غير موجود" });

        return Ok(result);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTherapistStatusDto dto)
    {
        var result = await _therapistAdminService.UpdateApprovalStatusAsync(
            id, dto.NewStatus, GetCurrentUserId(), GetClientIpAddress(), GetUserAgent());

        if (result is null)
            return NotFound(new { message = "المعالج غير موجود" });

        return Ok(result);
    }
}
