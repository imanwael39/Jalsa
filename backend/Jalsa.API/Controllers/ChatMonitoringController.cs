using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jalsa.API.Services.Interfaces;
using Jalsa.Application.DTOs.Crisis;
using Jalsa.Infrastructure.Data;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/chat-monitoring")]
[Authorize(Roles = "Therapist")]
public class ChatMonitoringController : ControllerBase
{
    private readonly IChatMonitoringService _chatMonitoringService;
    private readonly Galsa_DBDbContext _context;

    public ChatMonitoringController(IChatMonitoringService chatMonitoringService, Galsa_DBDbContext context)
    {
        _chatMonitoringService = chatMonitoringService;
        _context = context;
    }

    [HttpGet("engagement")]
    public async Task<IActionResult> GetEngagementSummaries(
        [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] Guid? patientId)
    {
        if (from.HasValue && to.HasValue && from > to)
            return BadRequest(new { error = "'from' date must be before or equal to 'to' date" });

        var summaries = await _chatMonitoringService.GetEngagementSummariesAsync(from, to, patientId);
        return Ok(summaries);
    }

    [HttpGet("crisis-alerts")]
    public async Task<IActionResult> GetCrisisAlerts(
        [FromQuery] bool? resolved, [FromQuery] Guid? patientId)
    {
        var query = _context.CrisisAlerts
            .Include(ca => ca.Patient)
            .Include(ca => ca.ChatMessage)
            .AsNoTracking()
            .AsQueryable();

        if (resolved.HasValue)
            query = query.Where(ca => resolved.Value ? ca.ResolvedAt != null : ca.ResolvedAt == null);

        if (patientId.HasValue)
            query = query.Where(ca => ca.PatientId == patientId.Value);

        var alerts = await query
            .OrderByDescending(ca => ca.CreatedAt)
            .Select(ca => new CrisisAlertViewDto
            {
                Id = ca.Id,
                PatientId = ca.PatientId,
                PatientName = ca.Patient.FullName,
                TherapistId = ca.TherapistId,
                Severity = ca.Severity,
                Status = ca.ResolvedAt != null ? "Resolved" : "Open",
                MessageSnippet = ca.ChatMessage != null ? ca.ChatMessage.Content : null,
                ChatMessageId = ca.ChatMessageId,
                CreatedAt = ca.CreatedAt,
                ResolvedAt = ca.ResolvedAt
            })
            .ToListAsync();

        return Ok(alerts);
    }

    [HttpPut("crisis-alerts/{id:guid}/resolve")]
    public async Task<IActionResult> ResolveCrisisAlert(Guid id)
    {
        var alert = await _context.CrisisAlerts.FindAsync(id);
        if (alert == null)
            return NotFound(new { error = "Crisis alert not found" });

        alert.Status = "Resolved";
        alert.ResolvedAt = DateTime.UtcNow;
        alert.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok();
    }
}
