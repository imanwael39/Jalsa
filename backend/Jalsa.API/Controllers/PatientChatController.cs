using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.Chat;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/patient/chat")]
[Authorize(Roles = "Patient")]
public class PatientChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly ICrisisService _crisisService;

    public PatientChatController(IChatService chatService, ICrisisService crisisService)
    {
        _chatService = chatService;
        _crisisService = crisisService;
    }

    private Guid GetPatientId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim is null) throw new Jalsa.API.Exceptions.ApiException(401, "User not authenticated.");
        return Guid.Parse(claim.Value);
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartSession()
    {
        var patientId = GetPatientId();
        var session = await _chatService.StartSessionAsync(patientId);
        return Ok(session);
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ChatSendMessageDto dto)
    {
        var patientId = GetPatientId();

        var owned = await _chatService.IsSessionOwnedByPatientAsync(dto.SessionId, patientId);
        if (!owned) return Forbid();

        var result = await _chatService.SendMessageAsync(patientId, dto);
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] Guid? sessionId)
    {
        var patientId = GetPatientId();
        var messages = await _chatService.GetHistoryAsync(patientId, sessionId);
        return Ok(messages);
    }

    [HttpPost("crisis")]
    public async Task<IActionResult> TriggerCrisisAlert()
    {
        var patientId = GetPatientId();
        await _crisisService.CreateManualAlertAsync(patientId);
        return Ok(new { message = "Crisis alert has been sent to your therapist." });
    }
}
