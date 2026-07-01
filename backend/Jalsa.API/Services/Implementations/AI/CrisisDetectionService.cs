using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace Jalsa.API.Services.Implementations.AI;

public class CrisisDetectionService : ICrisisDetectionService
{
    internal ChatClient _client;
<<<<<<< Updated upstream
    private static readonly string[] Keywords =
    [
        "suicide", "kill myself", "end my life", "want to die", "self-harm",
        "self harm", "hurt myself", "not worth living", "better off dead",
        "suicidal", "take my own life",
        "انتحار", "أقتل نفسي", "إنهاء حياتي", "أريد الموت", "إيذاء النفس",
        "لا أستحق العيش", "الأفضل أن أموت", "أفكار انتحارية", "وداعا"
    ];
=======
    private readonly ILlmObservabilityService? _observability;
    private readonly IPromptService _prompts;
    private readonly string _model = "gpt-4o";
>>>>>>> Stashed changes

    internal CrisisDetectionService(ChatClient client)
    {
        _client = client;
        _prompts = null!;
    }

<<<<<<< Updated upstream
    public CrisisDetectionService(IOptions<OpenAiSettings> settings)
    {
        var config = settings.Value;
=======
    public CrisisDetectionService(IOptions<OpenAiSettings> settings, ILlmObservabilityService observability, IPromptService prompts)
    {
        var config = settings.Value;
        _model = config.ChatModel;
        _observability = observability;
        _prompts = prompts;
>>>>>>> Stashed changes
        OpenAI.OpenAIClient openAi = string.IsNullOrWhiteSpace(config.Endpoint)
            ? new OpenAI.OpenAIClient(config.ApiKey)
            : new Azure.AI.OpenAI.AzureOpenAIClient(
                new Uri(config.Endpoint),
                new System.ClientModel.ApiKeyCredential(config.ApiKey));

        _client = openAi.GetChatClient(config.ChatModel);
    }

    public async Task<CrisisDetectionResult> AnalyzeAsync(string message)
    {
        var lower = message.ToLowerInvariant();
        var keywordHit = _prompts.GetKeywords("crisis-detection").Any(k => lower.Contains(k));

        if (!keywordHit)
            return new CrisisDetectionResult { IsCrisis = false };

        var lang = message.Any(c => c >= 0x0600 && c <= 0x06FF) ? "ar" : "en";

        var systemPrompt = _prompts.Get("crisis-detection", lang);

        var userPrompt = _prompts.Get("crisis-detection", lang, new Dictionary<string, string>
        {
            ["message"] = message
        });

        try
        {
            var response = await _client.CompleteChatAsync(
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userPrompt));

            var content = response.Value.Content[0].Text ?? "";
            var jsonStart = content.IndexOf('{');
            var jsonEnd = content.LastIndexOf('}');

            if (jsonStart >= 0 && jsonEnd > jsonStart)
            {
                var json = content[jsonStart..(jsonEnd + 1)];
                var doc = System.Text.Json.JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("isCrisis", out var crisisProp) && crisisProp.GetBoolean())
                {
                    var reason = doc.RootElement.TryGetProperty("reason", out var r) ? r.GetString() : null;
                    var suggested = doc.RootElement.TryGetProperty("suggestedResponse", out var s) ? s.GetString() : null;

                    return new CrisisDetectionResult
                    {
                        IsCrisis = true,
                        Reason = reason ?? _prompts.GetFallbackReason("crisis-detection", lang),
                        SuggestedMessage = suggested ?? _prompts.GetFallbackMessage("crisis-detection", lang)
                    };
                }
            }
        }
        catch
        {
        }

        return new CrisisDetectionResult { IsCrisis = false };
    }
}
