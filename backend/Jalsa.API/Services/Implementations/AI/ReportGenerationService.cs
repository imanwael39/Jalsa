using Jalsa.API.Configurations;
using Jalsa.API.DTOs.AI;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class ReportGenerationService : IReportGenerationService
{
    private readonly IGeminiClient _client;
    private readonly IPatientContextBuilder _contextBuilder;
    private readonly JalsaDbContext _context;
    private readonly string _model;

    private readonly ILlmObservabilityService _observability;
    private readonly Services.Interfaces.IPromptService _prompts;

    public ReportGenerationService(
        IOptions<GeminiSettings> settings,
        IGeminiClient client,
        IPatientContextBuilder contextBuilder,
        JalsaDbContext context,
        ILlmObservabilityService observability,
        Services.Interfaces.IPromptService prompts)
    {
        _observability = observability;
        _prompts = prompts;

        _client = client;
        _model = settings.Value.ChatModelId;
        _contextBuilder = contextBuilder;
        _context = context;
    }

    public async Task<string> GenerateDraftAsync(
        Guid patientId,
        string? therapistInstructions = null,
        string language = "ar",
        Guid? requestedByUserId = null)
    {
        var diagnostics = await GenerateDraftWithDiagnosticsAsync(patientId, therapistInstructions, language, requestedByUserId);
        return diagnostics.Output;
    }

    public async Task<AiGenerationDiagnosticsDto> GenerateDraftWithDiagnosticsAsync(
        Guid patientId,
        string? therapistInstructions = null,
        string language = "ar",
        Guid? requestedByUserId = null)
    {
        var bundle = await _contextBuilder.BuildAsync(
            patientId,
            embeddingQueryKey: "generate-report-draft",
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

        var sessionNotesText = ClinicalContextFormatter.BuildSessionNotesText(bundle.RecentSessionNotes, language);
        var voiceTranscriptsText = ClinicalContextFormatter.BuildVoiceTranscriptsText(bundle.VoiceTranscripts, language);
        var intakeText = ClinicalContextFormatter.BuildIntakeText(bundle.Intake, language);
        var assessmentsText = ClinicalContextFormatter.BuildAssessmentsText(bundle.Assessments, language);

        var instructions =
            !string.IsNullOrWhiteSpace(therapistInstructions)
                ? (language == "ar"
                    ? $"\nتعليمات المعالج: {therapistInstructions}"
                    : $"\nTherapist instructions: {therapistInstructions}")
                : "";

        var userPrompt =
            _prompts.Get(
                "generate-report-draft",
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
                    ["sessionNotesText"] = sessionNotesText,
                    ["voiceTranscriptsText"] = voiceTranscriptsText,
                    ["contextText"] = contextText,
                    ["instructions"] = instructions
                });

        var systemPrompt =
            _prompts.Get(
                "generate-report-draft",
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
                    Name = "generate-report-draft",
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
                Name = "generate-report-draft",
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

        if (requestedByUserId.HasValue)
        {
            var therapistId = await _context.Therapists
                .Where(t => t.UserId == requestedByUserId.Value)
                .Select(t => (Guid?)t.Id)
                .FirstOrDefaultAsync();

            if (therapistId.HasValue)
            {
                _context.AiReportGenerationLogs.Add(new Jalsa.Domain.Models.Ai.AiReportGenerationLog
                {
                    Id = Guid.NewGuid(),
                    PatientId = patientId,
                    SourceType = "ReportDraft",
                    ContentText = output.Length > 4000 ? output[..4000] : output,
                    GeneratedByTherapistId = therapistId.Value,
                    TokensUsed = generation.TotalTokens,
                    ProcessingTimeMs = latencyMs,
                    ModelUsed = _model,
                    CreatedAt = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();
            }
        }

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
}
