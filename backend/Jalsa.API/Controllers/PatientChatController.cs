using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jalsa.Application.DTOs.Chat;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Infrastructure.Data;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/patient/chat")]
[Authorize(Roles = "Patient")]
public class PatientChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly ICrisisService _crisisService;
    private readonly Galsa_DBDbContext _context;

    public PatientChatController(IChatService chatService, ICrisisService crisisService, Galsa_DBDbContext context)
    {
        _chatService = chatService;
        _crisisService = crisisService;
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

    [HttpPost("start")]
    public async Task<IActionResult> StartSession()
    {
        var patientId = await GetPatientIdAsync();
        var session = await _chatService.StartSessionAsync(patientId);
        return Ok(session);
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ChatSendMessageDto dto)
    {
        var patientId = await GetPatientIdAsync();

        var owned = await _chatService.IsSessionOwnedByPatientAsync(dto.SessionId, patientId);
        if (!owned) return Forbid();

        var result = await _chatService.SendMessageAsync(patientId, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] Guid? sessionId)
    {
        var patientId = await GetPatientIdAsync();
        var messages = await _chatService.GetHistoryAsync(patientId, sessionId);
        return Ok(messages);
    }

    [HttpPost("crisis")]
    public async Task<IActionResult> TriggerCrisisAlert()
    {
        var patientId = await GetPatientIdAsync();
        await _crisisService.CreateManualAlertAsync(patientId);
        return Ok(new { message = "Crisis alert has been sent to your therapist." });
    }
}
