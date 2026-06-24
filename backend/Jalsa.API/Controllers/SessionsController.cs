using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.Session;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    [Authorize(Roles = "Therapist")]
    public async Task<IActionResult> GetPaged([FromQuery] SessionSearchDto search)
    {
        var result = await _sessionService.GetPagedAsync(search);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var session = await _sessionService.GetByIdAsync(id);
        if (session is null) return NotFound();
        return Ok(session);
    }

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var sessions = await _sessionService.GetByPatientIdAsync(patientId);
        return Ok(sessions);
    }

    [HttpPost]
    [Authorize(Roles = "Therapist")]
    public async Task<IActionResult> Create([FromBody] SessionCreateDto dto)
    {
        var session = await _sessionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = session.Id }, session);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Therapist")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SessionUpdateDto dto)
    {
        var session = await _sessionService.UpdateAsync(id, dto);
        if (session is null) return NotFound();
        return Ok(session);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Therapist")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _sessionService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
