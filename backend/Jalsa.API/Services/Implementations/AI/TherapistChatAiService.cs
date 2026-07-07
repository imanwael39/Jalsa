using System.Text;
using Jalsa.API.Configurations;
using Jalsa.API.DTOs.AI;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class TherapistChatAiService : ITherapistChatAiService
{
    private readonly IGeminiClient _client;
    private readonly JalsaDbContext _context;
    private readonly IPatientContextBuilder _contextBuilder;
    private readonly IConversationMemoryService _memory;
    private readonly string _model;

    private readonly ILlmObservabilityService _observability;
    private readonly Services.Interfaces.IPromptService _prompts;

    public TherapistChatAiService(
        IOptions<GeminiSettings> settings,
        IGeminiClient client,
        JalsaDbContext context,
        IPatientContextBuilder contextBuilder,
        IConversationMemoryService memory,
        ILlmObservabilityService observability,
        Services.Interfaces.IPromptService prompts)
    {
        _client = client;
        _model = settings.Value.ChatModelId;
        _context = context;
        _contextBuilder = contextBuilder;
        _memory = memory;
        _observability = observability;
        _prompts = prompts;
    }

    private record PromptContext(string SystemPrompt, string UserPrompt, PatientContextBundle Bundle);

    private async Task<PromptContext?> BuildPromptAsync(Guid conversationId, Guid patientId, string question, string language)
    {
        var bundle = await _contextBuilder.BuildAsync(
            patientId,
            embeddingQueryKey: "therapist-patient-qa",
            language: language,
            ragTopK: 5,
            recentSessionCount: 5,
            explicitEmbeddingQuery: question);

        if (bundle.Demographics is null)
            return null;

        var history = await _context.ChatMessages
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.CreatedAt)
            .Take(10)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        var historyText = string.Join("\n", history.Select(m =>
        {
            var sender = m.SenderType switch
            {
                "Patient" when language == "ar" => "المريض",
                "AI" when language == "ar" => "المساعد",
                "Therapist" when language == "ar" => "المعالج",
                _ => m.SenderType
            };

            return $"{sender}: {m.Content}";
        }));

        var memoryResults = await _memory.RetrieveSimilarMessagesAsync(patientId, question, topK: 3);
        var memoryContext = string.Join("\n\n", memoryResults);

        var contextText = string.Join("\n\n", bundle.RagChunks.Select(r => r.Text));
        var intakeText = ClinicalContextFormatter.BuildIntakeText(bundle.Intake, language);
        var assessmentsText = ClinicalContextFormatter.BuildAssessmentsText(bundle.Assessments, language);
        var sessionNotesText = ClinicalContextFormatter.BuildSessionNotesText(bundle.RecentSessionNotes, language);

        var userPrompt = _prompts.Get(
            "therapist-patient-qa",
            language,
            new Dictionary<string, string>
            {
                ["patientName"] = bundle.Demographics.FullName,
                ["intakeText"] = intakeText,
                ["assessmentsText"] = assessmentsText,
                ["sessionNotesText"] = sessionNotesText,
                ["contextText"] = contextText,
                ["memoryContext"] = memoryContext,
                ["historyText"] = historyText,
                ["question"] = question
            });

        var systemPrompt = _prompts.Get("therapist-patient-qa", language);

        return new PromptContext(systemPrompt, userPrompt, bundle);
    }

    public async Task<string> AnswerQuestionAsync(
        Guid conversationId,
        Guid patientId,
        string question,
        string language = "ar")
    {
        var diagnostics = await AnswerQuestionWithDiagnosticsAsync(conversationId, patientId, question, language);
        return diagnostics.Output;
    }

    public async Task<AiGenerationDiagnosticsDto> AnswerQuestionWithDiagnosticsAsync(
        Guid conversationId,
        Guid patientId,
        string question,
        string language = "ar")
    {
        var ctx = await BuildPromptAsync(conversationId, patientId, question, language);
        if (ctx is null)
        {
            var notFound = language == "ar" ? "المريض غير موجود." : "Patient not found.";
            return new AiGenerationDiagnosticsDto { Output = notFound, Model = _model };
        }

        var startTime = DateTime.UtcNow;

        GeminiChatResult generation;
        try
        {
            generation = await _client.ChatWithUsageAsync(ctx.SystemPrompt, ctx.UserPrompt);
        }
        catch (Exception ex)
        {
            await LogFailureAsync(ctx, conversationId, patientId, startTime, ex);
            throw;
        }

        var endTime = DateTime.UtcNow;
        var latencyMs = (int)(endTime - startTime).TotalMilliseconds;

        await LogAndPersistAsync(ctx, conversationId, patientId, startTime, endTime, generation);

        return BuildDiagnostics(ctx, generation, latencyMs);
    }

    public async Task<AiGenerationDiagnosticsDto> AnswerQuestionStreamingAsync(
        Guid conversationId,
        Guid patientId,
        string question,
        string language,
        Func<string, Task> onChunk,
        CancellationToken cancellationToken = default)
    {
        var ctx = await BuildPromptAsync(conversationId, patientId, question, language);
        if (ctx is null)
        {
            var notFound = language == "ar" ? "المريض غير موجود." : "Patient not found.";
            await onChunk(notFound);
            return new AiGenerationDiagnosticsDto { Output = notFound, Model = _model };
        }

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
            await LogFailureAsync(ctx, conversationId, patientId, startTime, ex);
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
        var latencyMs = (int)(endTime - startTime).TotalMilliseconds;

        await LogAndPersistAsync(ctx, conversationId, patientId, startTime, endTime, generation);

        return BuildDiagnostics(ctx, generation, latencyMs);
    }

    private async Task LogFailureAsync(PromptContext ctx, Guid conversationId, Guid patientId, DateTime startTime, Exception ex)
    {
        await _observability.LogGenerationAsync(
            new LlmGenerationLog
            {
                Name = "therapist-patient-qa",
                Model = _model,
                Input = ctx.UserPrompt,
                Output = string.Empty,
                StartTime = startTime,
                EndTime = DateTime.UtcNow,
                Error = ex.Message,
                Metadata = new Dictionary<string, object>
                {
                    ["patientId"] = patientId.ToString(),
                    ["conversationId"] = conversationId.ToString(),
                    ["embeddingCount"] = ctx.Bundle.EmbeddingCount,
                    ["ragChunkCount"] = ctx.Bundle.RagChunks.Count
                }
            });
    }

    private async Task LogAndPersistAsync(
        PromptContext ctx, Guid conversationId, Guid patientId, DateTime startTime, DateTime endTime, GeminiChatResult generation)
    {
        await _observability.LogGenerationAsync(
            new LlmGenerationLog
            {
                Name = "therapist-patient-qa",
                Model = _model,
                Input = ctx.UserPrompt,
                Output = generation.Text,
                StartTime = startTime,
                EndTime = endTime,
                InputTokens = generation.InputTokens,
                OutputTokens = generation.OutputTokens,
                Metadata = new Dictionary<string, object>
                {
                    ["patientId"] = patientId.ToString(),
                    ["conversationId"] = conversationId.ToString(),
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

    private AiGenerationDiagnosticsDto BuildDiagnostics(PromptContext ctx, GeminiChatResult generation, int latencyMs) => new()
    {
        Output = generation.Text,
        SystemPrompt = ctx.SystemPrompt,
        UserPrompt = ctx.UserPrompt,
        Model = _model,
        LatencyMs = latencyMs,
        EmbeddingCount = ctx.Bundle.EmbeddingCount,
        RagChunkCount = ctx.Bundle.RagChunks.Count,
        InputTokens = generation.InputTokens,
        OutputTokens = generation.OutputTokens,
        TotalTokens = generation.TotalTokens,
        RagSources = ctx.Bundle.RagChunks.Select(r => new RagSourceDto
        {
            SessionId = r.SessionId,
            Score = r.Score,
            Source = r.Source,
            TextPreview = r.Text.Length > 200 ? r.Text[..200] + "…" : r.Text
        }).ToList()
    };
}
