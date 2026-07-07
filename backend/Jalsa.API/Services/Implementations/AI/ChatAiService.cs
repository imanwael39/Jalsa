using System.Text;
using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class ChatAiService : IChatAiService
{
    private readonly IGeminiClient _client;
    private readonly JalsaDbContext _context;
    private readonly IPatientContextBuilder _contextBuilder;
    private readonly IConversationMemoryService _memory;
    private readonly string _model;

    private readonly ILlmObservabilityService _observability;
    private readonly Services.Interfaces.IPromptService _prompts;

    public ChatAiService(
        IOptions<GeminiSettings> settings,
        IGeminiClient client,
        JalsaDbContext context,
        IPatientContextBuilder contextBuilder,
        IConversationMemoryService memory,
        ILlmObservabilityService observability,
        Services.Interfaces.IPromptService prompts)
    {
        _observability = observability;
        _prompts = prompts;

        _client = client;
        _model = settings.Value.ChatModelId;
        _context = context;
        _contextBuilder = contextBuilder;
        _memory = memory;
    }

    private record PromptContext(string SystemPrompt, string UserPrompt, PatientContextBundle Bundle);

    private async Task<PromptContext> BuildPromptAsync(Guid conversationId, Guid patientId, string message)
    {
        var lang =
            message.Any(c => c >= 0x0600 && c <= 0x06FF)
                ? "ar"
                : "en";

        var history = await _context.ChatMessages
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.CreatedAt)
            .Take(10)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        var bundle = await _contextBuilder.BuildAsync(
            patientId,
            embeddingQueryKey: "chat-response",
            language: lang,
            ragTopK: 3,
            recentSessionCount: 3,
            explicitEmbeddingQuery: message);

        var memoryResults =
            await _memory.RetrieveSimilarMessagesAsync(
                patientId,
                message,
                topK: 3);

        var ragContext = string.Join("\n\n", bundle.RagChunks.Select(r => r.Text));
        var memoryContext = string.Join("\n\n", memoryResults);
        var intakeText = ClinicalContextFormatter.BuildIntakeText(bundle.Intake, lang);
        var assessmentsText = ClinicalContextFormatter.BuildAssessmentsText(bundle.Assessments, lang);
        var sessionNotesText = ClinicalContextFormatter.BuildSessionNotesText(bundle.RecentSessionNotes, lang);

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
                ["intakeText"] = intakeText,
                ["assessmentsText"] = assessmentsText,
                ["ragContext"] = ragContext,
                ["sessionNotesText"] = sessionNotesText,
                ["memoryContext"] = memoryContext,
                ["historyText"] = historyText
            });

        var systemPrompt = _prompts.Get("chat-response");

        return new PromptContext(systemPrompt, userPrompt, bundle);
    }

    public async Task<string> GenerateResponseAsync(
        Guid conversationId,
        Guid patientId,
        string message)
    {
        var ctx = await BuildPromptAsync(conversationId, patientId, message);
        var startTime = DateTime.UtcNow;

        GeminiChatResult generation;
        try
        {
            generation = await _client.ChatWithUsageAsync(ctx.SystemPrompt, ctx.UserPrompt);
        }
        catch (Exception ex)
        {
            await LogFailureAsync(ctx, conversationId, patientId, message, startTime, ex);
            throw;
        }

        var endTime = DateTime.UtcNow;
        await LogAndPersistAsync(ctx, conversationId, patientId, message, startTime, endTime, generation);

        return generation.Text;
    }

    public async Task<string> GenerateResponseStreamingAsync(
        Guid conversationId,
        Guid patientId,
        string message,
        Func<string, Task> onChunk,
        CancellationToken cancellationToken = default)
    {
        var ctx = await BuildPromptAsync(conversationId, patientId, message);
        var startTime = DateTime.UtcNow;
        var text = new StringBuilder();
        int? inputTokens = null, outputTokens = null, totalTokens = null;

        try
        {
            await foreach (var chunk in _client.ChatStreamAsync(ctx.SystemPrompt, ctx.UserPrompt, cancellationToken: cancellationToken))
            {
                if (!string.IsNullOrEmpty(chunk.TextDelta))
                {
                    text.Append(chunk.TextDelta);
                    await onChunk(chunk.TextDelta);
                }

                if (chunk.IsFinal)
                {
                    inputTokens = chunk.InputTokens;
                    outputTokens = chunk.OutputTokens;
                    totalTokens = chunk.TotalTokens;
                }
            }
        }
        catch (Exception ex)
        {
            await LogFailureAsync(ctx, conversationId, patientId, message, startTime, ex);
            throw;
        }

        var generation = new GeminiChatResult
        {
            Text = text.ToString(),
            InputTokens = inputTokens,
            OutputTokens = outputTokens,
            TotalTokens = totalTokens
        };

        var endTime = DateTime.UtcNow;
        await LogAndPersistAsync(ctx, conversationId, patientId, message, startTime, endTime, generation);

        return generation.Text;
    }

    private async Task LogFailureAsync(
        PromptContext ctx, Guid conversationId, Guid patientId, string message, DateTime startTime, Exception ex)
    {
        await _observability.LogGenerationAsync(
            new LlmGenerationLog
            {
                Name = "chat-response",
                Model = _model,
                Input = message,
                Output = string.Empty,
                StartTime = startTime,
                EndTime = DateTime.UtcNow,
                Error = ex.Message,
                Metadata = new Dictionary<string, object>
                {
                    ["conversationId"] = conversationId.ToString(),
                    ["patientId"] = patientId.ToString(),
                    ["embeddingCount"] = ctx.Bundle.EmbeddingCount,
                    ["ragChunkCount"] = ctx.Bundle.RagChunks.Count
                }
            });
    }

    private async Task LogAndPersistAsync(
        PromptContext ctx, Guid conversationId, Guid patientId, string message,
        DateTime startTime, DateTime endTime, GeminiChatResult generation)
    {
        await _observability.LogGenerationAsync(
            new LlmGenerationLog
            {
                Name = "chat-response",
                Model = _model,
                Input = message,
                Output = generation.Text,
                StartTime = startTime,
                EndTime = endTime,
                InputTokens = generation.InputTokens,
                OutputTokens = generation.OutputTokens,

                Metadata = new Dictionary<string, object>
                {
                    ["conversationId"] = conversationId.ToString(),
                    ["patientId"] = patientId.ToString(),
                    ["embeddingCount"] = ctx.Bundle.EmbeddingCount,
                    ["ragChunkCount"] = ctx.Bundle.RagChunks.Count
                }
            });

        _context.AiChatLogs.Add(
            new Jalsa.Domain.Models.Chat.AiChatLog
            {
                Id = Guid.NewGuid(),
                ConversationId = conversationId,
                PatientId = patientId,
                ModelUsed = _model,
                ResponseLatencyMs = (int)(endTime - startTime).TotalMilliseconds,
                TokensUsed = generation.TotalTokens,
                CreatedAt = DateTime.UtcNow
            });

        await _context.SaveChangesAsync();
    }
}
