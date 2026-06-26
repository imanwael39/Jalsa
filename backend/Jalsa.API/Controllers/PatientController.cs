using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jalsa.Application.DTOs.Exercise;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Infrastructure.Data;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/patient")]
[Authorize(Roles = "Patient")]
public class PatientController : ControllerBase
{
    private readonly IExerciseService _exerciseService;
    private readonly Galsa_DBDbContext _context;

    public PatientController(IExerciseService exerciseService, Galsa_DBDbContext context)
    {
        _exerciseService = exerciseService;
        _context = context;
    }

    private async Task<Guid> GetPatientIdAsync()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim is null) throw new Jalsa.API.Exceptions.ApiException(401, "User not authenticated.");

        var userId = Guid.Parse(userIdClaim.Value);
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
        if (patient is null) throw new Jalsa.API.Exceptions.ApiException(404, "Patient profile not found.");

        return patient.Id;
    }

    [HttpGet("exercises")]
    public async Task<IActionResult> GetExercises([FromQuery] string? status)
    {
        var patientId = await GetPatientIdAsync();
        var exercises = await _exerciseService.GetPatientExercisesAsync(patientId, status);
        return Ok(exercises);
    }

    [HttpGet("exercises/{id:guid}")]
    public async Task<IActionResult> GetExerciseById(Guid id)
    {
        var patientId = await GetPatientIdAsync();
        var owned = await _exerciseService.IsExerciseOwnedByPatientAsync(id, patientId);
        if (!owned) return Forbid();

        var exercise = await _exerciseService.GetExerciseByIdAsync(id);
        if (exercise is null) return NotFound();
        return Ok(exercise);
    }

    [HttpPut("exercises/{id:guid}/status")]
    public async Task<IActionResult> UpdateExerciseStatus(Guid id, [FromBody] ExerciseStatusUpdateDto dto)
    {
        var patientId = await GetPatientIdAsync();
        var result = await _exerciseService.UpdateExerciseStatusAsync(id, patientId, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }
}
