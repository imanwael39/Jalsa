using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Domain.Models.Crisis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

/// <summary>
/// LLM-based crisis classifier. Intentionally has no keyword pre-filter: a fixed keyword
/// list cannot recognize indirect phrasing ("مش هصحى بكرة" / "I won't wake up tomorrow")
/// and would silently create false negatives, so every message is sent to the model for
/// intent classification. This is the only implementation of ICrisisDetectionService the
/// Patient Support Chat depends on — swapping in Azure AI Content Safety or a custom model
/// later means adding a new class against that interface, not touching the chat pipeline.
/// </summary>
public class CrisisDetectionService : ICrisisDetectionService
{
    private readonly IGeminiClient _client;
    private readonly ILlmObservabilityService? _observability;
    private readonly IPromptService _prompts;
    private readonly ILogger<CrisisDetectionService> _logger;
    private readonly string _model;

    public CrisisDetectionService(
        IOptions<GeminiSettings> settings,
        IGeminiClient client,
        ILlmObservabilityService observability,
        IPromptService prompts,
        ILogger<CrisisDetectionService> logger)
    {
        _client = client;
        _model = settings.Value.ChatModelId;
        _observability = observability;
        _prompts = prompts;
        _logger = logger;
    }

    public async Task<CrisisDetectionResult> AnalyzeAsync(string message, IReadOnlyList<string>? conversationHistory = null)
    {
        if (string.IsNullOrWhiteSpace(message))
            return new CrisisDetectionResult { IsCrisis = false };

        var lang = message.Any(c => c >= 0x0600 && c <= 0x06FF) ? "ar" : "en";

        var historyText = conversationHistory is { Count: > 0 }
            ? (lang == "ar"
                ? "سياق المحادثة الأخير:\n" + string.Join("\n", conversationHistory) + "\n\n"
                : "Recent conversation context:\n" + string.Join("\n", conversationHistory) + "\n\n")
            : string.Empty;

        var systemPrompt = _prompts.Get("crisis-detection", lang);
        var userPrompt = _prompts.Get(
            "crisis-detection",
            lang,
            new Dictionary<string, string>
            {
                ["message"] = message,
                ["historyText"] = historyText
            });

        var startTime = DateTime.UtcNow;

        try
        {
            var content = await _client.ChatAsync(systemPrompt, userPrompt);

            if (_observability != null)
            {
                await _observability.LogGenerationAsync(
                    new LlmGenerationLog
                    {
                        Name = "crisis-detection",
                        Model = _model,
                        // Deliberately not logging the patient's message/prompt content —
                        // only the classifier's structured output, per privacy requirements.
                        Input = "[redacted patient message]",
                        Output = content,
                        StartTime = startTime,
                        EndTime = DateTime.UtcNow
                    });
            }

            var result = ParseResult(content, lang);

            _logger.LogInformation(
                "Crisis detection completed: IsCrisis={IsCrisis} Severity={Severity} Confidence={Confidence}",
                result.IsCrisis, result.Severity, result.Confidence);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Crisis detection AI call failed");
            return new CrisisDetectionResult { IsCrisis = false };
        }
    }

    private CrisisDetectionResult ParseResult(string content, string lang)
    {
        var jsonStart = content.IndexOf('{');
        var jsonEnd = content.LastIndexOf('}');

        if (jsonStart < 0 || jsonEnd <= jsonStart)
            return new CrisisDetectionResult { IsCrisis = false };

        try
        {
            var json = content[jsonStart..(jsonEnd + 1)];
            var doc = System.Text.Json.JsonDocument.Parse(json);
            var root = doc.RootElement;

            var isCrisis = root.TryGetProperty("isCrisis", out var crisisProp) && crisisProp.ValueKind == System.Text.Json.JsonValueKind.True;

            if (!isCrisis)
                return new CrisisDetectionResult { IsCrisis = false };

            var severity = CrisisSeverity.Medium;
            if (root.TryGetProperty("severity", out var sevProp)
                && sevProp.ValueKind == System.Text.Json.JsonValueKind.String
                && Enum.TryParse<CrisisSeverity>(sevProp.GetString(), ignoreCase: true, out var parsedSeverity))
            {
                severity = parsedSeverity;
            }

            var confidence = 0.5;
            if (root.TryGetProperty("confidence", out var confProp) && confProp.ValueKind == System.Text.Json.JsonValueKind.Number)
            {
                confidence = Math.Clamp(confProp.GetDouble(), 0.0, 1.0);
            }

            var reason = root.TryGetProperty("reason", out var r) && r.ValueKind == System.Text.Json.JsonValueKind.String
                ? r.GetString()
                : null;

            return new CrisisDetectionResult
            {
                IsCrisis = true,
                Severity = severity,
                Confidence = confidence,
                Reason = string.IsNullOrWhiteSpace(reason)
                    ? _prompts.GetFallbackReason("crisis-detection", lang)
                    : reason
            };
        }
        catch (System.Text.Json.JsonException ex)
        {
            _logger.LogWarning(ex, "Crisis detection model returned unparseable JSON");
            return new CrisisDetectionResult { IsCrisis = false };
        }
    }
}
