using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.Exercise;
using Jalsa.Application.Interfaces.Services;
using ExtendDueDateRequest = Jalsa.Application.DTOs.Exercise.ExtendDueDateRequest;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/exercises")]
public class ExerciseController : BaseController
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    // ──────────────────────────── Therapist Endpoints ────────────────────────────

    [Authorize(Roles = "Therapist")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        var result = await _exerciseService.GetAllAsync(userId);
        return Ok(result);
    }

    [Authorize(Roles = "Therapist")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetCurrentUserId();
        var result = await _exerciseService.GetByIdAsync(id, userId);
        return Ok(result);
    }

    [Authorize(Roles = "Therapist")]
    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var userId = GetCurrentUserId();
        var result = await _exerciseService.GetByPatientIdAsync(patientId, userId);
        return Ok(result);
    }

    [Authorize(Roles = "Therapist")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExerciseCreateDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _exerciseService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [Authorize(Roles = "Therapist")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ExerciseUpdateDto dto)
    {
        var userId = GetCurrentUserId();
        dto.Id = id;
        var result = await _exerciseService.UpdateAsync(dto, userId);
        return Ok(result);
    }

    [Authorize(Roles = "Therapist")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetCurrentUserId();
        await _exerciseService.DeleteAsync(id, userId);
        return NoContent();
    }

    [Authorize(Roles = "Therapist")]
    [HttpPut("{id:guid}/extend")]
    public async Task<IActionResult> ExtendDueDate(Guid id, [FromBody] ExtendDueDateRequest request)
    {
        var userId = GetCurrentUserId();
        await _exerciseService.ExtendDueDateAsync(id, request.NewDueDate, userId);
        return NoContent();
    }

    // ──────────────────────────── Patient Endpoints ──────────────────────────────

    [Authorize(Roles = "Patient")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyExercises()
    {
        var patientId = GetPatientId();
        if (patientId is null) return Unauthorized();

        var result = await _exerciseService.GetByPatientIdAsync(patientId.Value);
        return Ok(result);
    }

    [Authorize(Roles = "Patient")]
    [HttpPost("log")]
    public async Task<IActionResult> LogCompletion([FromBody] ExerciseLogCreateDto dto)
    {
        var patientId = GetPatientId();
        if (patientId is null) return Unauthorized();

        dto.PatientId = patientId.Value;
        var result = await _exerciseService.LogCompletionAsync(dto);
        return Created("", result);
    }

    [Authorize(Roles = "Patient")]
    [HttpGet("my/logs")]
    public async Task<IActionResult> GetMyLogs()
    {
        var patientId = GetPatientId();
        if (patientId is null) return Unauthorized();

        var result = await _exerciseService.GetLogsByPatientIdAsync(patientId.Value);
        return Ok(result);
    }

    // ──────────────────────────── Helpers ────────────────────────────────────────

    private Guid? GetPatientId()
    {
        var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return value is null ? null : Guid.Parse(value);
    }
}
