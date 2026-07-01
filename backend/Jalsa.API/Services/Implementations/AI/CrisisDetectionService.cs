using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace Jalsa.API.Services.Implementations.AI;

public class CrisisDetectionService : ICrisisDetectionService
{
    internal ChatClient _client;

    private readonly ILlmObservabilityService? _observability;
    private readonly IPromptService _prompts;
    private string _model = "gpt-4o";

    internal CrisisDetectionService(ChatClient client)
    {
        _client = client;
        _prompts = null!;
    }

    public CrisisDetectionService(
        IOptions<OpenAiSettings> settings,
        ILlmObservabilityService observability,
        IPromptService prompts)
    {
        var config = settings.Value;

        _model = config.ChatModel;
        _observability = observability;
        _prompts = prompts;

        OpenAI.OpenAIClient openAi =
            string.IsNullOrWhiteSpace(config.Endpoint)
                ? new OpenAI.OpenAIClient(config.ApiKey)
                : new Azure.AI.OpenAI.AzureOpenAIClient(
                    new Uri(config.Endpoint),
                    new System.ClientModel.ApiKeyCredential(config.ApiKey));

        _client = openAi.GetChatClient(_model);
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

            var response =
                await _client.CompleteChatAsync(
                    new SystemChatMessage(systemPrompt),
                    new UserChatMessage(userPrompt));

            var content =
                response.Value.Content[0].Text ?? "";

            if (_observability != null)
            {
                await _observability.LogGenerationAsync(
                    new LlmGenerationLog
                    {
                        Name = "crisis-detection",
                        Model = _model,
                        Input = userPrompt,
                        Output = content,
                        InputTokens = response.Value.Usage?.InputTokenCount,
                        OutputTokens = response.Value.Usage?.OutputTokenCount,
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