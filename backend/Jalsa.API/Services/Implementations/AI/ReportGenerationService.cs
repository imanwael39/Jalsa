using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class ReportGenerationService : IReportGenerationService
{
    private readonly IGeminiClient _client;
    private readonly JalsaDbContext _context;
    private readonly IVectorStore _vectorStore;
    private readonly IEmbeddingService _embeddingService;
    private readonly string _model;

    private readonly ILlmObservabilityService _observability;
    private readonly Services.Interfaces.IPromptService _prompts;

    public ReportGenerationService(
        IOptions<GeminiSettings> settings,
        IGeminiClient client,
        JalsaDbContext context,
        IVectorStore vectorStore,
        IEmbeddingService embeddingService,
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
    }

    public async Task<string> GenerateDraftAsync(
        Guid patientId,
        string? therapistInstructions = null,
        string language = "ar")
    {
        var patient = await _context.Patients
            .Include(p => p.IntakeForms)
            .Include(p => p.Assessments)
            .ThenInclude(a => a.Template)
            .FirstOrDefaultAsync(p => p.Id == patientId);

        if (patient == null)
            return "Patient not found.";

        var queryVec =
            await _embeddingService.GenerateEmbeddingAsync(
                _prompts.GetEmbeddingQuery(
                    "generate-report-draft",
                    "ar")
                .Replace("{patientName}", patient.FullName));

        var ragContext =
            await _vectorStore.SearchAsync(queryVec, topK: 5);

        var contextText =
            string.Join("\n\n", ragContext.Select(r => r.Text));

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
                    ["patientName"] = patient.FullName,
                    ["dateOfBirth"] =
                        patient.DateOfBirth?.ToString("d") ?? "",
                    ["gender"] = patient.Gender ?? "",
                    ["contextText"] = contextText,
                    ["instructions"] = instructions
                });

        var systemPrompt =
            _prompts.Get(
                "generate-report-draft",
                language);

        var startTime = DateTime.UtcNow;

        var output =
            await _client.ChatAsync(systemPrompt, userPrompt);

        await _observability.LogGenerationAsync(
            new LlmGenerationLog
            {
                Name = "generate-report-draft",
                Model = _model,
                Input = userPrompt,
                Output = output,
                StartTime = startTime,
                EndTime = DateTime.UtcNow,

                Metadata = new Dictionary<string, object>
                {
                    ["patientId"] = patientId.ToString()
                }
            });

        return output;
    }
}