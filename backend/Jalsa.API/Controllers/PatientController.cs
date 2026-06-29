using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.Patient;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Therapist")]
public class PatientController : BaseController
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var currentUserId = GetCurrentUserId();
        var result = await _patientService.GetByIdAsync(id, currentUserId);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PatientFilterDto? filter)
    {
        var currentUserId = GetCurrentUserId();
        var result = await _patientService.GetAllAsync(currentUserId, filter);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PatientCreateDto dto)
    {
        var currentUserId = GetCurrentUserId();
        var result = await _patientService.CreateAsync(dto, currentUserId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, PatientUpdateDto dto)
    {
        var currentUserId = GetCurrentUserId();
        var result = await _patientService.UpdateAsync(id, dto, currentUserId);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/archive")]
    public async Task<IActionResult> Archive(Guid id)
    {
        var currentUserId = GetCurrentUserId();
        await _patientService.ArchiveAsync(id, currentUserId);
        return NoContent();
    }

    [HttpPatch("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        var currentUserId = GetCurrentUserId();
        await _patientService.RestoreAsync(id, currentUserId);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var currentUserId = GetCurrentUserId();
        await _patientService.DeleteAsync(id, currentUserId);
        return NoContent();
    }
}
