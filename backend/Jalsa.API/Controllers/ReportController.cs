using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.Exceptions;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.DTOs.Report;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = "Therapist")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IReportGenerationService _aiService;

    public ReportController(IReportService reportService, IReportGenerationService aiService)
    {
        _reportService = reportService;
        _aiService = aiService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] ReportGenerateDto dto)
    {
        var therapistId = GetCurrentUserId();
        var aiContent = await _aiService.GenerateDraftAsync(dto.PatientId, dto.TherapistInstructions, dto.Language);
        var result = await _reportService.CreateWithAiContentAsync(dto.PatientId, aiContent, therapistId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var therapistId = GetCurrentUserId();
        var result = await _reportService.GetByIdAsync(id, therapistId);
        return Ok(result);
    }

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var therapistId = GetCurrentUserId();
        var result = await _reportService.GetByPatientIdAsync(patientId, therapistId);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ReportUpdateDto dto)
    {
        var therapistId = GetCurrentUserId();
        var result = await _reportService.UpdateAsync(id, dto, therapistId);
        return Ok(result);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var therapistId = GetCurrentUserId();
        var result = await _reportService.ApproveAsync(id, therapistId);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var therapistId = GetCurrentUserId();
        await _reportService.DeleteAsync(id, therapistId);
        return NoContent();
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
