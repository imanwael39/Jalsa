using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.Exercise;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/exercises")]
public class ExerciseController : ControllerBase
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
        try
        {
            var result = await _exerciseService.GetAllAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Therapist")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var result = await _exerciseService.GetByIdAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Therapist")]
    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        try
        {
            var result = await _exerciseService.GetByPatientIdAsync(patientId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Therapist")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExerciseCreateDto dto)
    {
        try
        {
            var result = await _exerciseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Therapist")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ExerciseUpdateDto dto)
    {
        try
        {
            dto.Id = id;
            var result = await _exerciseService.UpdateAsync(dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Therapist")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _exerciseService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Therapist")]
    [HttpPut("{id:guid}/extend")]
    public async Task<IActionResult> ExtendDueDate(Guid id, [FromBody] ExtendDueDateRequest request)
    {
        try
        {
            await _exerciseService.ExtendDueDateAsync(id, request.NewDueDate);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // ──────────────────────────── Patient Endpoints ──────────────────────────────

    [Authorize(Roles = "Patient")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyExercises()
    {
        try
        {
            var patientId = GetPatientId();
            if (patientId is null) return Unauthorized();

            var result = await _exerciseService.GetByPatientIdAsync(patientId.Value);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Patient")]
    [HttpPost("log")]
    public async Task<IActionResult> LogCompletion([FromBody] ExerciseLogCreateDto dto)
    {
        try
        {
            var patientId = GetPatientId();
            if (patientId is null) return Unauthorized();

            dto.PatientId = patientId.Value;
            var result = await _exerciseService.LogCompletionAsync(dto);
            return Created("", result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [Authorize(Roles = "Patient")]
    [HttpGet("my/logs")]
    public async Task<IActionResult> GetMyLogs()
    {
        try
        {
            var patientId = GetPatientId();
            if (patientId is null) return Unauthorized();

            var result = await _exerciseService.GetLogsByPatientIdAsync(patientId.Value);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // ──────────────────────────── Helpers ────────────────────────────────────────

    private Guid? GetPatientId()
    {
        var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return value is null ? null : Guid.Parse(value);
    }

    public record ExtendDueDateRequest(DateOnly NewDueDate);
}
