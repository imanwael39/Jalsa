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

    private readonly ILlmObservabilityService _observability;
    private readonly Services.Interfaces.IPromptService _prompts;

    public ChatAiService(
        IOptions<OpenAiSettings> settings,
        JalsaDbContext context,
        IVectorStore vectorStore,
        IEmbeddingService embeddingService,
        IConversationMemoryService memory,
        ILlmObservabilityService observability,
        Services.Interfaces.IPromptService prompts)
    {
        _observability = observability;
        _prompts = prompts;

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

        var messages = new List<OpenAI.Chat.ChatMessage>
        {
            new OpenAI.Chat.SystemChatMessage(
                _prompts.Get("chat-response")),

            new OpenAI.Chat.UserChatMessage(
                userPrompt)
        };

        var startTime = DateTime.UtcNow;

        var result = await _client.CompleteChatAsync(messages);

        var response =
            result.Value.Content[0].Text;

        await _observability.LogGenerationAsync(
            new LlmGenerationLog
            {
                Name = "chat-response",
                Model = _model,
                Input = message,
                Output = response,
                InputTokens = result.Value.Usage?.InputTokenCount,
                OutputTokens = result.Value.Usage?.OutputTokenCount,
                StartTime = startTime,
                EndTime = DateTime.UtcNow,

                Metadata = new Dictionary<string, object>
                {
                    ["conversationId"] = conversationId.ToString(),
                    ["patientId"] = patientId.ToString()
                }
            });

        var startedAt =
            history.Count > 0
                ? history.First().CreatedAt
                : DateTime.UtcNow;

        _context.AiChatLogs.Add(
            new Jalsa.Domain.Models.Chat.AiChatLog
            {
                Id = Guid.NewGuid(),
                ConversationId = conversationId,
                PatientId = patientId,
                TokensUsed = result.Value.Usage?.OutputTokenCount,
                ModelUsed = _model,
                ResponseLatencyMs =
                    (int?)(DateTime.UtcNow - startedAt)
                    .TotalMilliseconds,
                CreatedAt = DateTime.UtcNow
            });

        await _context.SaveChangesAsync();

        return response;
    }
}