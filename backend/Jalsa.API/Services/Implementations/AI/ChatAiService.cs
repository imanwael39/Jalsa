using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class ChatAiService : IChatAiService
{
    private readonly IGatewayClient _client;
    private readonly JalsaDbContext _context;
    private readonly IVectorStore _vectorStore;
    private readonly IEmbeddingService _embeddingService;
    private readonly IConversationMemoryService _memory;
    private readonly string _model;

    private readonly ILlmObservabilityService _observability;
    private readonly Services.Interfaces.IPromptService _prompts;

    public ChatAiService(
        IOptions<GatewaySettings> settings,
        IGatewayClient client,
        JalsaDbContext context,
        IVectorStore vectorStore,
        IEmbeddingService embeddingService,
        IConversationMemoryService memory,
        ILlmObservabilityService observability,
        Services.Interfaces.IPromptService prompts)
    {
        _observability = observability;
        _prompts = prompts;

        _client = client;
        _model = settings.Value.ChatModelId;
        _context = context;
        _vectorStore = vectorStore;
        _embeddingService = embeddingService;
        _memory = memory;
    }

    public async Task<string> GenerateResponseAsync(
        Guid conversationId,
        Guid patientId,
        string message)
    {
        var history = await _context.ChatMessages
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.CreatedAt)
            .Take(10)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        var ragResults = await _vectorStore.SearchAsync(
            await _embeddingService.GenerateEmbeddingAsync(message),
            topK: 3);

        var memoryResults =
            await _memory.RetrieveSimilarMessagesAsync(
                patientId,
                message,
                topK: 3);

        var ragContext =
            string.Join("\n\n", ragResults.Select(r => r.Text));

        var memoryContext =
            string.Join("\n\n", memoryResults);

        var lang =
            message.Any(c => c >= 0x0600 && c <= 0x06FF)
                ? "ar"
                : "en";

        var historyText =
            string.Join("\n", history.Select(m =>
            {
                var sender = m.SenderType switch
                {
                    "Patient" when lang == "ar" => "المريض",
                    "AI" when lang == "ar" => "المساعد",
                    _ => m.SenderType
                };

                return $"{sender}: {m.Content}";
            }));

        var userPrompt = _prompts.Get(
            "chat-response",
            lang,
            new Dictionary<string, string>
            {
                ["message"] = message,
                ["ragContext"] = ragContext,
                ["memoryContext"] = memoryContext,
                ["historyText"] = historyText
            });

        var systemPrompt = _prompts.Get("chat-response");

        var startTime = DateTime.UtcNow;

        var response = await _client.ChatAsync(systemPrompt, userPrompt);

        await _observability.LogGenerationAsync(
            new LlmGenerationLog
            {
                Name = "chat-response",
                Model = _model,
                Input = message,
                Output = response,
                StartTime = startTime,
                EndTime = DateTime.UtcNow,

                Metadata = new Dictionary<string, object>
                {
                    ["conversationId"] = conversationId.ToString(),
                    ["patientId"] = patientId.ToString()
                }
            });

        _context.AiChatLogs.Add(
            new Jalsa.Domain.Models.Chat.AiChatLog
            {
                Id = Guid.NewGuid(),
                ConversationId = conversationId,
                PatientId = patientId,
                ModelUsed = _model,
                ResponseLatencyMs =
                    (int?)(DateTime.UtcNow - startTime)
                    .TotalMilliseconds,
                CreatedAt = DateTime.UtcNow
            });

        await _context.SaveChangesAsync();

        return response;
    }
}