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
public class AiController : BaseController
{
    private readonly ISummarizationService _summarizationService;
    private readonly IReportGenerationService _reportGenerationService;
    private readonly ITherapistChatAiService _therapistChatAi;
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorStore _vectorStore;
    private readonly GeminiSettings _geminiSettings;
    private readonly ILogger<AiController> _logger;

    public AiController(
        ISummarizationService summarizationService,
        IReportGenerationService reportGenerationService,
        ITherapistChatAiService therapistChatAi,
        IEmbeddingService embeddingService,
        IVectorStore vectorStore,
        IOptions<GeminiSettings> geminiSettings,
        ILogger<AiController> logger)
    {
        _summarizationService = summarizationService;
        _reportGenerationService = reportGenerationService;
        _therapistChatAi = therapistChatAi;
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
        _geminiSettings = geminiSettings.Value;
        _logger = logger;
    }

    [HttpPost("summarize/{patientId:guid}")]
    public async Task<IActionResult> SummarizePatient(Guid patientId, [FromBody] SummarizeRequest? request)
    {
        var summary = await _summarizationService.SummarizePatientAsync(
            patientId,
            request?.Language ?? "ar",
            GetCurrentUserId());
        return Ok(new { summary });
    }

    [HttpPost("report-draft/{patientId:guid}")]
    public async Task<IActionResult> GenerateReportDraft(Guid patientId, [FromBody] ReportDraftRequest? request)
    {
        var draft = await _reportGenerationService.GenerateDraftAsync(
            patientId,
            request?.TherapistInstructions,
            request?.Language ?? "ar",
            GetCurrentUserId());
        return Ok(new { draft });
    }

    [HttpGet("diagnostics/patient-summary/{patientId:guid}")]
    public async Task<IActionResult> DiagnosePatientSummary(Guid patientId, [FromQuery] string language = "ar")
    {
        var diagnostics = await _summarizationService.SummarizePatientWithDiagnosticsAsync(patientId, language);
        return Ok(diagnostics);
    }

    [HttpGet("diagnostics/report-draft/{patientId:guid}")]
    public async Task<IActionResult> DiagnoseReportDraft(Guid patientId, [FromQuery] string language = "ar", [FromQuery] string? instructions = null)
    {
        var diagnostics = await _reportGenerationService.GenerateDraftWithDiagnosticsAsync(patientId, instructions, language);
        return Ok(diagnostics);
    }

    [HttpPost("diagnostics/chat")]
    public async Task<IActionResult> DiagnoseChatAnswer([FromBody] ChatDiagnosticsRequest request)
    {
        var diagnostics = await _therapistChatAi.AnswerQuestionWithDiagnosticsAsync(
            request.ConversationId,
            request.PatientId,
            request.Question,
            request.Language ?? "ar");
        return Ok(diagnostics);
    }

    [HttpGet("diagnostics/embeddings/{patientId:guid}")]
    public async Task<IActionResult> GetEmbeddingCount(Guid patientId)
    {
        var count = await _vectorStore.CountAsync(patientId);
        return Ok(new { patientId, embeddingCount = count });
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
