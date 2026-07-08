using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.DTOs.Session;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/sessions")]
[Authorize(Roles = "Therapist")]
public class SessionController : BaseController
{
    private readonly ISessionService _sessionService;
    private readonly ISummarizationService _summarizationService;

    public SessionController(
        ISessionService sessionService,
        ISummarizationService summarizationService)
    {
        _sessionService = sessionService;
        _summarizationService = summarizationService;
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

    [HttpGet("{id:guid}/summary")]
    public async Task<IActionResult> GetSummary(Guid id, [FromQuery] string language = "ar")
    {
        var therapistId = GetCurrentUserId();
        await _sessionService.GetByIdAsync(id, therapistId);

        var summary = await _summarizationService.SummarizeSessionAsync(id, language, therapistId);
        return Ok(new { sessionId = id, summary });
    }

    [HttpPost("{id:guid}/patient-request/approve")]
    public async Task<IActionResult> ApprovePatientRequest(Guid id, [FromBody] ApprovePatientRequestDto dto)
    {
        var therapistId = GetCurrentUserId();
        var result = await _sessionService.ApprovePatientRequestAsync(id, dto, therapistId);
        return Ok(result);
    }

    [HttpPost("{id:guid}/patient-request/reject")]
    public async Task<IActionResult> RejectPatientRequest(Guid id)
    {
        var therapistId = GetCurrentUserId();
        var result = await _sessionService.RejectPatientRequestAsync(id, therapistId);
        return Ok(result);
    }
}
