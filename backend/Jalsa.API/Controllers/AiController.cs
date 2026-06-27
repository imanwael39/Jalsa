using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.Services.Interfaces.AI;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize(Roles = "Therapist")]
public class AiController : ControllerBase
{
    private readonly ISummarizationService _summarizationService;
    private readonly IReportGenerationService _reportGenerationService;

    public AiController(
        ISummarizationService summarizationService,
        IReportGenerationService reportGenerationService)
    {
        _summarizationService = summarizationService;
        _reportGenerationService = reportGenerationService;
    }

    [HttpPost("summarize/{patientId:guid}")]
    public async Task<IActionResult> SummarizePatient(Guid patientId, [FromBody] SummarizeRequest? request)
    {
        var summary = await _summarizationService.SummarizePatientAsync(
            patientId,
            request?.Language ?? "ar");
        return Ok(new { summary });
    }

    [HttpPost("report-draft/{patientId:guid}")]
    public async Task<IActionResult> GenerateReportDraft(Guid patientId, [FromBody] ReportDraftRequest? request)
    {
        var draft = await _reportGenerationService.GenerateDraftAsync(
            patientId,
            request?.TherapistInstructions,
            request?.Language ?? "ar");
        return Ok(new { draft });
    }
}

public class SummarizeRequest
{
    public string Language { get; set; } = "ar";
}

public class ReportDraftRequest
{
    public string? TherapistInstructions { get; set; }
    public string Language { get; set; } = "ar";
}
