using Jalsa.API.Services.Interfaces;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.API.Services.Implementations.AI;

/// <summary>
/// Assembles a full clinical context bundle (demographics, intake, assessments,
/// recent session notes, voice transcripts, patient-scoped RAG chunks) for a
/// single patient. Shared by summary, report, and chat generation so none of
/// them depend solely on RAG for context.
/// </summary>
public class PatientContextBuilder : IPatientContextBuilder
{
    private readonly JalsaDbContext _context;
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorStore _vectorStore;
    private readonly IPromptService _prompts;
    private readonly ILogger<PatientContextBuilder> _logger;

    public PatientContextBuilder(
        JalsaDbContext context,
        IEmbeddingService embeddingService,
        IVectorStore vectorStore,
        IPromptService prompts,
        ILogger<PatientContextBuilder> logger)
    {
        _context = context;
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
        _prompts = prompts;
        _logger = logger;
    }

    public async Task<PatientContextBundle> BuildAsync(
        Guid patientId,
        string embeddingQueryKey,
        string language,
        int ragTopK = 5,
        int recentSessionCount = 5,
        string? explicitEmbeddingQuery = null,
        CancellationToken ct = default)
    {
        var patient = await _context.Patients
            .AsNoTracking()
            .Include(p => p.IntakeForms)
            .Include(p => p.Assessments).ThenInclude(a => a.Template)
            .FirstOrDefaultAsync(p => p.Id == patientId, ct);

        if (patient is null)
            return new PatientContextBundle();

        var recentSessions = await _context.Sessions
            .AsNoTracking()
            .Where(s => s.PatientId == patientId)
            .Include(s => s.SessionNote)
            .Include(s => s.VoiceMemos)
            .OrderByDescending(s => s.SessionDate)
            .ThenByDescending(s => s.SessionNumber)
            .Take(recentSessionCount)
            .ToListAsync(ct);

        var embeddingCount = await _vectorStore.CountAsync(patientId);

        var ragChunks = await BuildRagChunksAsync(patient.Id, patient.FullName, embeddingQueryKey, language, ragTopK, embeddingCount, explicitEmbeddingQuery, ct);

        var latestIntake = patient.IntakeForms
            .OrderByDescending(i => i.SubmittedAt ?? i.CreatedAt)
            .FirstOrDefault();

        return new PatientContextBundle
        {
            Demographics = new PatientDemographics
            {
                Id = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Phone = patient.Phone,
                Email = patient.Email,
                Address = patient.Address,
                ReferralSource = patient.ReferralSource,
                ChiefComplaint = patient.ChiefComplaint,
                MedicalHistory = patient.MedicalHistory,
                EmergencyContact = patient.EmergencyContact,
                Status = patient.Status,
                CreatedAt = patient.CreatedAt
            },
            Intake = latestIntake is null ? null : new IntakeContext
            {
                PresentingProblem = latestIntake.PresentingProblem,
                PsychiatricHistory = latestIntake.PsychiatricHistory,
                FamilyHistory = latestIntake.FamilyHistory,
                Medications = latestIntake.Medications,
                SocialHistory = latestIntake.SocialHistory,
                Status = latestIntake.Status,
                SubmittedAt = latestIntake.SubmittedAt
            },
            Assessments = patient.Assessments
                .OrderByDescending(a => a.AssessmentDate)
                .Select(a => new AssessmentContext
                {
                    Title = a.Title,
                    TemplateName = a.Template?.Name ?? string.Empty,
                    AssessmentDate = a.AssessmentDate,
                    TotalScore = a.TotalScore,
                    Severity = a.Severity,
                    Notes = a.Notes,
                    Status = a.Status
                }).ToList(),
            RecentSessionNotes = recentSessions
                .Where(s => s.SessionNote is not null)
                .Select(s => new SessionNoteContext
                {
                    SessionId = s.Id,
                    SessionNumber = s.SessionNumber,
                    SessionDate = s.SessionDate,
                    SessionType = s.SessionType,
                    DurationMinutes = s.DurationMinutes,
                    Observations = s.SessionNote!.Observations,
                    Interventions = s.SessionNote!.Interventions,
                    PatientResponse = s.SessionNote!.PatientResponse,
                    HomeworkAssigned = s.SessionNote!.HomeworkAssigned,
                    NextGoals = s.SessionNote!.NextGoals,
                    UpdatedAt = s.SessionNote!.UpdatedAt
                }).ToList(),
            VoiceTranscripts = recentSessions
                .SelectMany(s => s.VoiceMemos
                    .Where(v => !string.IsNullOrWhiteSpace(v.Transcript))
                    .Select(v => new VoiceTranscriptContext
                    {
                        SessionId = s.Id,
                        SessionNumber = s.SessionNumber,
                        Transcript = v.Transcript!
                    }))
                .ToList(),
            RagChunks = ragChunks,
            EmbeddingCount = embeddingCount
        };
    }

    private async Task<IReadOnlyList<RagContextChunk>> BuildRagChunksAsync(
        Guid patientId,
        string patientName,
        string embeddingQueryKey,
        string language,
        int topK,
        int embeddingCount,
        string? explicitEmbeddingQuery,
        CancellationToken ct)
    {
        if (embeddingCount == 0)
            return Array.Empty<RagContextChunk>();

        string embeddingQuery;
        if (!string.IsNullOrWhiteSpace(explicitEmbeddingQuery))
        {
            embeddingQuery = explicitEmbeddingQuery;
        }
        else
        {
            var embeddingQueryTemplate = _prompts.GetEmbeddingQuery(embeddingQueryKey, language);
            if (string.IsNullOrWhiteSpace(embeddingQueryTemplate))
                return Array.Empty<RagContextChunk>();

            embeddingQuery = embeddingQueryTemplate.Replace("{patientName}", patientName);
        }

        float[] queryVector;
        try
        {
            queryVector = await _embeddingService.GenerateEmbeddingAsync(embeddingQuery);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to embed RAG query for patient {PatientId}.", patientId);
            return Array.Empty<RagContextChunk>();
        }

        if (queryVector.Length == 0)
            return Array.Empty<RagContextChunk>();

        var results = await _vectorStore.SearchAsync(new VectorSearchQuery
        {
            QueryVector = queryVector,
            TopK = topK,
            PatientId = patientId
        });

        return results.Select(r => new RagContextChunk
        {
            Id = r.Id,
            SessionId = r.SessionId ?? Guid.Empty,
            Text = r.Text,
            Score = r.Score,
            Source = r.Source
        }).ToList();
    }
}
