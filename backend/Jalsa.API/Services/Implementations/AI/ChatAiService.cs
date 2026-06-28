using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class ChatAiService : IChatAiService
{
    private readonly OpenAI.Chat.ChatClient _client;
    private readonly JalsaDbContext _context;
    private readonly IVectorStore _vectorStore;
    private readonly IEmbeddingService _embeddingService;
    private readonly IConversationMemoryService _memory;
    private readonly string _model;

    public ChatAiService(
        IOptions<OpenAiSettings> settings,
        JalsaDbContext context,
        IVectorStore vectorStore,
        IEmbeddingService embeddingService,
        IConversationMemoryService memory)
    {
        var config = settings.Value;
        _model = config.ChatModel;
        _context = context;
        _vectorStore = vectorStore;
        _embeddingService = embeddingService;
        _memory = memory;

        OpenAI.OpenAIClient openAi = string.IsNullOrWhiteSpace(config.Endpoint)
            ? new OpenAI.OpenAIClient(config.ApiKey)
            : new Azure.AI.OpenAI.AzureOpenAIClient(
                new Uri(config.Endpoint),
                new System.ClientModel.ApiKeyCredential(config.ApiKey));

        _client = openAi.GetChatClient(_model);
    }

    public async Task<string> GenerateResponseAsync(Guid conversationId, Guid patientId, string message)
    {
        var history = await _context.ChatMessages
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.CreatedAt)
            .Take(10)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        var ragResults = await _vectorStore.SearchAsync(
            await _embeddingService.GenerateEmbeddingAsync(message), topK: 3);
        var memoryResults = await _memory.RetrieveSimilarMessagesAsync(patientId, message, topK: 3);

        var ragContext = string.Join("\n\n", ragResults.Select(r => r.Text));
        var memoryContext = string.Join("\n\n", memoryResults);
        var lang = message.Any(c => c >= 0x0600 && c <= 0x06FF) ? "ar" : "en";

        var historyText = string.Join("\n", history.Select(m =>
        {
            var sender = lang == "ar" ? (m.SenderType == "Patient" ? "المريض" : m.SenderType == "AI" ? "المساعد" : m.SenderType) : m.SenderType;
            return $"{sender}: {m.Content}";
        }));

        var prompt = lang == "ar"
            ? $"""
            رسالة المريض: {message}

            سياق الجلسات ذات الصلة:
            {ragContext}

            ذاكرة المحادثات السابقة:
            {memoryContext}

            تاريخ المحادثة الأخير:
            {historyText}
            """
            : $"""
            Patient message: {message}

            Relevant session context:
            {ragContext}

            Past conversation memory:
            {memoryContext}

            Recent conversation history:
            {historyText}
            """;

        var messages = new List<OpenAI.Chat.ChatMessage>
        {
            new OpenAI.Chat.SystemChatMessage("You are a supportive mental health AI assistant. Provide empathetic, helpful responses in the same language as the user's message. Never give medical advice. If the user expresses crisis thoughts, respond supportively and encourage them to contact their therapist or emergency services. / أنت مساعد دعم نفسي متعاطف. قدم ردودًا داعمة ومفيدة بنفس لغة رسالة المستخدم. لا تقدم أبدًا نصائح طبية. إذا عبر المستخدم عن أفكار أزمة، رد بشكل داعم وشجعه على التواصل مع معالجه أو خدمات الطوارئ."),
            new OpenAI.Chat.UserChatMessage(prompt)
        };

        var result = await _client.CompleteChatAsync(messages);
        var response = result.Value.Content[0].Text;

        var startedAt = history.Count > 0 ? history.First().CreatedAt : DateTime.UtcNow;

        _context.AiChatLogs.Add(new Jalsa.Domain.Models.Chat.AiChatLog
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            PatientId = patientId,
            TokensUsed = result.Value.Usage?.OutputTokenCount,
            ModelUsed = _model,
            ResponseLatencyMs = (int?)(DateTime.UtcNow - startedAt).TotalMilliseconds,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return response;
    }
}
