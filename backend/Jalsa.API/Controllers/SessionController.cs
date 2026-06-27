using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.Exceptions;
using Jalsa.Application.DTOs.Session;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/sessions")]
[Authorize(Roles = "Therapist")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SessionCreateDto dto)
    {
        var therapistId = GetCurrentUserId();
        var result = await _sessionService.CreateAsync(dto, therapistId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var therapistId = GetCurrentUserId();
        var result = await _sessionService.GetByIdAsync(id, therapistId);
        return Ok(result);
    }

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var therapistId = GetCurrentUserId();
        var result = await _sessionService.GetByPatientIdAsync(patientId, therapistId);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SessionUpdateDto dto)
    {
        var therapistId = GetCurrentUserId();
        dto.Id = id;
        var result = await _sessionService.UpdateAsync(dto, therapistId);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var therapistId = GetCurrentUserId();
        await _sessionService.DeleteAsync(id, therapistId);
        return NoContent();
    }

    [HttpPost("{id:guid}/note")]
    public async Task<IActionResult> SaveNote(Guid id, [FromBody] SessionNoteDto dto)
    {
        var therapistId = GetCurrentUserId();
        var result = await _sessionService.SaveNoteAsync(id, dto, therapistId);
        return Ok(result);
    }

    [HttpGet("{id:guid}/note")]
    public async Task<IActionResult> GetNote(Guid id)
    {
        var therapistId = GetCurrentUserId();
        var result = await _sessionService.GetNoteAsync(id, therapistId);
        if (result is null) return NotFound();
        return Ok(result);
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
