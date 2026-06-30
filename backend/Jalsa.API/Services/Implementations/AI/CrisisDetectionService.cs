using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace Jalsa.API.Services.Implementations.AI;

public class CrisisDetectionService : ICrisisDetectionService
{
    internal ChatClient _client;
    private readonly ILlmObservabilityService? _observability;
    private readonly string _model = "gpt-4o";
    private static readonly string[] Keywords =
    [
        "suicide", "kill myself", "end my life", "want to die", "self-harm",
        "self harm", "hurt myself", "not worth living", "better off dead",
        "suicidal", "take my own life",
        "انتحار", "أقتل نفسي", "إنهاء حياتي", "أريد الموت", "إيذاء النفس",
        "لا أستحق العيش", "الأفضل أن أموت", "أفكار انتحارية", "وداعا"
    ];

    internal CrisisDetectionService(ChatClient client)
    {
        _client = client;
    }

    public CrisisDetectionService(IOptions<OpenAiSettings> settings, ILlmObservabilityService observability)
    {
        var config = settings.Value;
        _model = config.ChatModel;
        _observability = observability;
        OpenAI.OpenAIClient openAi = string.IsNullOrWhiteSpace(config.Endpoint)
            ? new OpenAI.OpenAIClient(config.ApiKey)
            : new Azure.AI.OpenAI.AzureOpenAIClient(
                new Uri(config.Endpoint),
                new System.ClientModel.ApiKeyCredential(config.ApiKey));

        _client = openAi.GetChatClient(_model);
    }

    public async Task<CrisisDetectionResult> AnalyzeAsync(string message)
    {
        var lower = message.ToLowerInvariant();
        var keywordHit = Keywords.Any(k => lower.Contains(k));

        if (!keywordHit)
            return new CrisisDetectionResult { IsCrisis = false };

        var lang = message.Any(c => c >= 0x0600 && c <= 0x06FF) ? "ar" : "en";

        var systemPrompt = lang == "ar"
            ? "أنت مساعد كشف الأزمات. حدد ما إذا كانت رسالة المستخدم تشير إلى خطر إيذاء النفس أو الانتحار."
            : "You are a crisis detection assistant. Determine if the user message indicates imminent self-harm or suicide risk.";

        var userPrompt = lang == "ar"
            ? $"هل تشير هذه الرسالة إلى أزمة؟ أجب بتنسيق JSON: {{\"isCrisis\": true/false, \"reason\": \"...\", \"suggestedResponse\": \"...\"}}\n\nالرسالة: {message}"
            : $"Does this message indicate a crisis? Reply with JSON: {{\"isCrisis\": true/false, \"reason\": \"...\", \"suggestedResponse\": \"...\"}}\n\nMessage: {message}";

        try
        {
            var startTime = DateTime.UtcNow;
            var response = await _client.CompleteChatAsync(
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userPrompt));

            var content = response.Value.Content[0].Text ?? "";

            if (_observability != null)
            {
                await _observability.LogGenerationAsync(new LlmGenerationLog
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
                var doc = System.Text.Json.JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("isCrisis", out var crisisProp) && crisisProp.GetBoolean())
                {
                    var reason = doc.RootElement.TryGetProperty("reason", out var r) ? r.GetString() : null;
                    var suggested = doc.RootElement.TryGetProperty("suggestedResponse", out var s) ? s.GetString() : null;

                    return new CrisisDetectionResult
                    {
                        IsCrisis = true,
                        Reason = reason ?? (lang == "ar"
                            ? "مطابقة كلمات مفتاحية + تحقق الذكاء الاصطناعي"
                            : "Keyword match + AI verification"),
                        SuggestedMessage = suggested ?? (lang == "ar"
                            ? "أنا قلق بشأن ما تشاركه. يرجى التواصل مع معالجك أو الاتصال بخدمات الطوارئ فورًا إذا كنت في خطر. يمكنك الاتصال بالأمانة العامة للصحة النفسية: 16328"
                            : "I'm concerned about what you're sharing. Please contact your therapist or call emergency services immediately if you're in danger.")
                    };
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Crisis detection AI call failed: {ex.Message}");
        }

        return new CrisisDetectionResult { IsCrisis = false };
    }
}
