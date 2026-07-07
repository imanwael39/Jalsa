using Jalsa.API.Configurations;
using Jalsa.API.DTOs.AI;
using Jalsa.API.Services.Interfaces;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class SummarizationService : ISummarizationService
{
    private readonly IGeminiClient _client;
    private readonly JalsaDbContext _context;
    private readonly IPatientContextBuilder _contextBuilder;
    private readonly ILlmObservabilityService _observability;
    private readonly IPromptService _prompts;
    private readonly string _model;

    public SummarizationService(
        IOptions<GeminiSettings> settings,
        IGeminiClient client,
        JalsaDbContext context,
        IPatientContextBuilder contextBuilder,
        ILlmObservabilityService observability,
        IPromptService prompts)
    {
        _client = client;
        _context = context;
        _contextBuilder = contextBuilder;
        _observability = observability;
        _prompts = prompts;
        _model = settings.Value.ChatModelId;
    }

    public async Task<string> SummarizePatientAsync(
        Guid patientId,
        string language = "ar",
        Guid? requestedByUserId = null)
    {
        var diagnostics = await SummarizePatientWithDiagnosticsAsync(patientId, language, requestedByUserId);
        return diagnostics.Output;
    }

    public async Task<AiGenerationDiagnosticsDto> SummarizePatientWithDiagnosticsAsync(
        Guid patientId,
        string language = "ar",
        Guid? requestedByUserId = null)
    {
        var bundle = await _contextBuilder.BuildAsync(
            patientId,
            embeddingQueryKey: "summarize-patient",
            language: language,
            ragTopK: 5,
            recentSessionCount: 5);

        if (bundle.Demographics is null)
        {
            var notFound = language == "ar" ? "المريض غير موجود." : "Patient not found.";
            return new AiGenerationDiagnosticsDto { Output = notFound, Model = _model };
        }

        var demographics = bundle.Demographics;

        var contextText = string.Join("\n\n", bundle.RagChunks.Select(r => r.Text));

        var recentNotesText = ClinicalContextFormatter.BuildSessionNotesText(bundle.RecentSessionNotes, language);

        var intakeText = ClinicalContextFormatter.BuildIntakeText(bundle.Intake, language);

        var assessmentsText = ClinicalContextFormatter.BuildAssessmentsText(bundle.Assessments, language);

        var userPrompt =
            _prompts.Get(
                "summarize-patient",
                language,
                new Dictionary<string, string>
                {
                    ["patientName"] = demographics.FullName,
                    ["dateOfBirth"] =
                        demographics.DateOfBirth?.ToString("d") ?? "",
                    ["gender"] = demographics.Gender ?? "",
                    ["chiefComplaint"] = demographics.ChiefComplaint ?? "",
                    ["medicalHistory"] = demographics.MedicalHistory ?? "",
                    ["intakeText"] = intakeText,
                    ["assessmentsText"] = assessmentsText,
                    ["contextText"] = contextText,
                    ["recentNotesText"] = recentNotesText
                });

        var systemPrompt =
            _prompts.Get(
                "summarize-patient",
                language);

        var startTime = DateTime.UtcNow;

        GeminiChatResult generation;
        try
        {
            generation = await _client.ChatWithUsageAsync(systemPrompt, userPrompt);
        }
        catch (Exception ex)
        {
            await _observability.LogGenerationAsync(
                new LlmGenerationLog
                {
                    Name = "summarize-patient",
                    Model = _model,
                    Input = userPrompt,
                    Output = string.Empty,
                    StartTime = startTime,
                    EndTime = DateTime.UtcNow,
                    Error = ex.Message,
                    Metadata = new Dictionary<string, object>
                    {
                        ["patientId"] = patientId.ToString(),
                        ["embeddingCount"] = bundle.EmbeddingCount,
                        ["ragChunkCount"] = bundle.RagChunks.Count
                    }
                });
            throw;
        }

        var output = generation.Text;

        var endTime = DateTime.UtcNow;
        var latencyMs = (int)(endTime - startTime).TotalMilliseconds;

        await _observability.LogGenerationAsync(
            new LlmGenerationLog
            {
                Name = "summarize-patient",
                Model = _model,
                Input = userPrompt,
                Output = output,
                StartTime = startTime,
                EndTime = endTime,
                InputTokens = generation.InputTokens,
                OutputTokens = generation.OutputTokens,
                Metadata = new Dictionary<string, object>
                {
                    ["patientId"] = patientId.ToString(),
                    ["embeddingCount"] = bundle.EmbeddingCount,
                    ["ragChunkCount"] = bundle.RagChunks.Count
                }
            });

        await PersistGenerationLogAsync(
            patientId, "PatientSummary", null, output, requestedByUserId, generation.TotalTokens, latencyMs);

        return new AiGenerationDiagnosticsDto
        {
            Output = output,
            SystemPrompt = systemPrompt,
            UserPrompt = userPrompt,
            Model = _model,
            LatencyMs = latencyMs,
            EmbeddingCount = bundle.EmbeddingCount,
            RagChunkCount = bundle.RagChunks.Count,
            InputTokens = generation.InputTokens,
            OutputTokens = generation.OutputTokens,
            TotalTokens = generation.TotalTokens,
            RagSources = bundle.RagChunks.Select(r => new RagSourceDto
            {
                SessionId = r.SessionId,
                Score = r.Score,
                Source = r.Source,
                TextPreview = r.Text.Length > 200 ? r.Text[..200] + "…" : r.Text
            }).ToList()
        };
    }

    private async Task PersistGenerationLogAsync(
        Guid patientId,
        string sourceType,
        Guid? sourceId,
        string output,
        Guid? requestedByUserId,
        int? tokensUsed,
        int latencyMs)
    {
        if (!requestedByUserId.HasValue) return;

        var therapistId = await _context.Therapists
            .Where(t => t.UserId == requestedByUserId.Value)
            .Select(t => (Guid?)t.Id)
            .FirstOrDefaultAsync();

        if (!therapistId.HasValue) return;

        _context.AiReportGenerationLogs.Add(new Jalsa.Domain.Models.Ai.AiReportGenerationLog
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            SourceType = sourceType,
            SourceId = sourceId,
            ContentText = output.Length > 4000 ? output[..4000] : output,
            GeneratedByTherapistId = therapistId.Value,
            TokensUsed = tokensUsed,
            ProcessingTimeMs = latencyMs,
            ModelUsed = _model,
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
    }

    public async Task<string> SummarizeSessionAsync(
        Guid sessionId,
        string language = "ar",
        Guid? requestedByUserId = null)
    {
        var session = await _context.Sessions
            .Include(s => s.SessionNote)
            .Include(s => s.VoiceMemos)
            .FirstOrDefaultAsync(s => s.Id == sessionId);

        if (session is null)
            return language == "ar"
                ? "الجلسة غير موجودة."
                : "Session not found.";

        var n = session.SessionNote;

        var note = n is null
            ? string.Empty
            : string.Join("\n",
            new[]
            {
                n.Observations is not null ? $"الملاحظات: {n.Observations}" : null,
                n.Interventions is not null ? $"التدخلات: {n.Interventions}" : null,
                n.PatientResponse is not null ? $"استجابة المريض: {n.PatientResponse}" : null,
                n.HomeworkAssigned is not null ? $"الواجبات المنزلية: {n.HomeworkAssigned}" : null,
                n.NextGoals is not null ? $"الأهداف التالية: {n.NextGoals}" : null
            }.Where(x => x is not null));

        var voiceTranscripts =
            string.Join("\n",
                session.VoiceMemos.Select(
                    v => v.Transcript ?? string.Empty));

        var voiceSection =
            string.IsNullOrWhiteSpace(voiceTranscripts)
                ? ""
                : (language == "ar"
                    ? $"نصوص التسجيلات الصوتية:\n{voiceTranscripts}"
                    : $"Voice Transcripts:\n{voiceTranscripts}");

        var userPrompt =
            _prompts.Get(
                "summarize-session",
                language,
                new Dictionary<string, string>
                {
                    ["sessionDate"] =
                        session.SessionDate.ToString("dd/MM/yyyy"),

                    ["durationMinutes"] =
                        session.DurationMinutes?.ToString() ?? "",

                    ["sessionType"] =
                        session.SessionType ?? "",

                    ["note"] = note,
                    ["voiceTranscripts"] = voiceSection
                });

        var systemPrompt =
            _prompts.Get(
                "summarize-session",
                language);

        var startTime = DateTime.UtcNow;

        GeminiChatResult generation;
        try
        {
            generation = await _client.ChatWithUsageAsync(systemPrompt, userPrompt);
        }
        catch (Exception ex)
        {
            await _observability.LogGenerationAsync(
                new LlmGenerationLog
                {
                    Name = "summarize-session",
                    Model = _model,
                    Input = userPrompt,
                    Output = string.Empty,
                    StartTime = startTime,
                    EndTime = DateTime.UtcNow,
                    Error = ex.Message,
                    Metadata = new Dictionary<string, object>
                    {
                        ["sessionId"] = sessionId.ToString()
                    }
                });
            throw;
        }

        var output = generation.Text;
        var endTime = DateTime.UtcNow;
        var latencyMs = (int)(endTime - startTime).TotalMilliseconds;

        await _observability.LogGenerationAsync(
            new LlmGenerationLog
            {
                Name = "summarize-session",
                Model = _model,
                Input = userPrompt,
                Output = output,
                StartTime = startTime,
                EndTime = endTime,
                InputTokens = generation.InputTokens,
                OutputTokens = generation.OutputTokens,
                Metadata = new Dictionary<string, object>
                {
                    ["sessionId"] = sessionId.ToString()
                }
            });

        await PersistGenerationLogAsync(
            session.PatientId, "SessionSummary", sessionId, output, requestedByUserId, generation.TotalTokens, latencyMs);

        return output;
    }
}