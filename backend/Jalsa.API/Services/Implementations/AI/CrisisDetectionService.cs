using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class CrisisDetectionService : ICrisisDetectionService
{
    private readonly IGatewayClient _client;

    private readonly ILlmObservabilityService? _observability;
    private readonly IPromptService _prompts;
    private readonly string _model;

    public CrisisDetectionService(
        IOptions<GatewaySettings> settings,
        IGatewayClient client,
        ILlmObservabilityService observability,
        IPromptService prompts)
    {
        _client = client;
        _model = settings.Value.ChatModelId;
        _observability = observability;
        _prompts = prompts;
    }

    public async Task<CrisisDetectionResult> AnalyzeAsync(string message)
    {
        var lower = message.ToLowerInvariant();

        var keywordHit = _prompts
            .GetKeywords("crisis-detection")
            .Any(k => lower.Contains(k));

        if (!keywordHit)
            return new CrisisDetectionResult
            {
                IsCrisis = false
            };

        var lang =
            message.Any(c => c >= 0x0600 && c <= 0x06FF)
                ? "ar"
                : "en";

        var systemPrompt =
            _prompts.Get("crisis-detection", lang);

        var userPrompt =
            _prompts.Get(
                "crisis-detection",
                lang,
                new Dictionary<string, string>
                {
                    ["message"] = message
                });

        try
        {
            var startTime = DateTime.UtcNow;

            var content = await _client.ChatAsync(systemPrompt, userPrompt);

            if (_observability != null)
            {
                await _observability.LogGenerationAsync(
                    new LlmGenerationLog
                    {
                        Name = "crisis-detection",
                        Model = _model,
                        Input = userPrompt,
                        Output = content,
                        StartTime = startTime,
                        EndTime = DateTime.UtcNow
                    });
            }

            var jsonStart = content.IndexOf('{');
            var jsonEnd = content.LastIndexOf('}');

            if (jsonStart >= 0 && jsonEnd > jsonStart)
            {
                var json = content[jsonStart..(jsonEnd + 1)];

                var doc =
                    System.Text.Json.JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty(
                        "isCrisis",
                        out var crisisProp)
                    && crisisProp.GetBoolean())
                {
                    var reason =
                        doc.RootElement.TryGetProperty(
                            "reason",
                            out var r)
                            ? r.GetString()
                            : null;

                    var suggested =
                        doc.RootElement.TryGetProperty(
                            "suggestedResponse",
                            out var s)
                            ? s.GetString()
                            : null;

                    return new CrisisDetectionResult
                    {
                        IsCrisis = true,
                        Reason = reason
                            ?? _prompts.GetFallbackReason(
                                "crisis-detection",
                                lang),

                        SuggestedMessage = suggested
                            ?? _prompts.GetFallbackMessage(
                                "crisis-detection",
                                lang)
                    };
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Crisis detection AI call failed: {ex.Message}");
        }

        return new CrisisDetectionResult
        {
            IsCrisis = false
        };
    }
}