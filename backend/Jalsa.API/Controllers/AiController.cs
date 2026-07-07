using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Jalsa.API.DTOs.AI;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.API.Configurations;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize(Roles = "Therapist")]
[EnableRateLimiting("ai")]
public class AiController : ControllerBase
{
    private readonly ISummarizationService _summarizationService;
    private readonly IReportGenerationService _reportGenerationService;
    private readonly IEmbeddingService _embeddingService;
    private readonly GeminiSettings _geminiSettings;
    private readonly ILogger<AiController> _logger;

    public AiController(
        ISummarizationService summarizationService,
        IReportGenerationService reportGenerationService,
        IEmbeddingService embeddingService,
        IOptions<GeminiSettings> geminiSettings,
        ILogger<AiController> logger)
    {
        _summarizationService = summarizationService;
        _reportGenerationService = reportGenerationService;
        _embeddingService = embeddingService;
        _geminiSettings = geminiSettings.Value;
        _logger = logger;
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

    [HttpGet("test-embed")]
    public async Task<IActionResult> TestEmbed([FromQuery] string text = "مرحبا")
    {
        var modelId = _geminiSettings.EmbeddingModelId;
        var baseUrl = _geminiSettings.BaseUrl;
        var apiKey = _geminiSettings.ApiKey;
        var apiKeyPresent = !string.IsNullOrWhiteSpace(apiKey);

        try
        {
            var vector = await _embeddingService.GenerateEmbeddingAsync(text);
            return Ok(new
            {
                ok = true,
                provider = "gemini",
                modelId,
                baseUrl,
                dimensions = vector.Length,
                preview = vector.Take(5).ToArray(),
                text
            });
        }
        catch (Exception ex)
        {
            string? gatewayBody = null;
            try
            {
                if (apiKeyPresent)
                {
                    using var probe = new HttpClient { BaseAddress = new Uri(baseUrl) };
                    var payload = new StringContent(
                        System.Text.Json.JsonSerializer.Serialize(new
                        {
                            model = $"models/{modelId}",
                            content = new { parts = new[] { new { text } } },
                            taskType = "RETRIEVAL_DOCUMENT",
                            outputDimensionality = _geminiSettings.EmbeddingDimensions
                        }),
                        System.Text.Encoding.UTF8, "application/json");
                    using var resp = await probe.PostAsync(
                        $"models/{modelId}:embedContent?key={apiKey}", payload);
                    gatewayBody = await resp.Content.ReadAsStringAsync();
                }
            }
            catch { }

            _logger.LogError(ex, "test-embed failed");
            return StatusCode(502, new
            {
                ok = false,
                provider = "gemini",
                modelId,
                baseUrl,
                apiKeyPresent,
                error = ex.Message,
                innerError = ex.InnerException?.Message,
                gatewayBody
            });
        }
    }
}
