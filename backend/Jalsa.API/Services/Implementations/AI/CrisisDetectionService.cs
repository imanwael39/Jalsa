using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace Jalsa.API.Services.Implementations.AI;

public class CrisisDetectionService : ICrisisDetectionService
{
    internal ChatClient _client;
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

    public CrisisDetectionService(IOptions<OpenAiSettings> settings)
    {
        var config = settings.Value;
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
            await _client.CompleteChatAsync(
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userPrompt));
        }
        catch
        {
        }

        return new CrisisDetectionResult
        {
            IsCrisis = true,
            Reason = lang == "ar"
                ? "مطابقة كلمات مفتاحية + تحقق الذكاء الاصطناعي"
                : "Keyword match + AI verification",
            SuggestedMessage = lang == "ar"
                ? "أنا قلق بشأن ما تشاركه. يرجى التواصل مع معالجك أو الاتصال بخدمات الطوارئ فورًا إذا كنت في خطر. يمكنك الاتصال بالأمانة العامة للصحة النفسية: 16328"
                : "I'm concerned about what you're sharing. Please contact your therapist or call emergency services immediately if you're in danger."
        };
    }
}
