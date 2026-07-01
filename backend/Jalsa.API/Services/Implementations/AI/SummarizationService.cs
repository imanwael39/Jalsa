using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace Jalsa.API.Services.Implementations.AI;

public class SummarizationService : ISummarizationService
{
    private readonly ChatClient _client;
    private readonly JalsaDbContext _context;
    private readonly IVectorStore _vectorStore;
    private readonly IEmbeddingService _embeddingService;
    private readonly ILlmObservabilityService _observability;
    private readonly IPromptService _prompts;
    private readonly string _model;

    public SummarizationService(
        IOptions<OpenAiSettings> settings,
        JalsaDbContext context,
        IVectorStore vectorStore,
        IEmbeddingService embeddingService,
        ILlmObservabilityService observability,
        IPromptService prompts)
    {
        var config = settings.Value;

        _context = context;
        _vectorStore = vectorStore;
        _embeddingService = embeddingService;
        _observability = observability;
        _prompts = prompts;
        _model = config.ChatModel;

        OpenAI.OpenAIClient openAi =
            string.IsNullOrWhiteSpace(config.Endpoint)
                ? new OpenAI.OpenAIClient(config.ApiKey)
                : new Azure.AI.OpenAI.AzureOpenAIClient(
                    new Uri(config.Endpoint),
                    new System.ClientModel.ApiKeyCredential(config.ApiKey));

        _client = openAi.GetChatClient(config.ChatModel);
    }

    public async Task<string> SummarizePatientAsync(
        Guid patientId,
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
                    "summarize-patient",
                    "ar")
                .Replace("{patientName}", patient.FullName));

        var ragContext =
            await _vectorStore.SearchAsync(
                queryVec,
                topK: 5,
                metadataFilter: null);

        var contextText =
            string.Join("\n\n", ragContext.Select(r => r.Text));

        var userPrompt =
            _prompts.Get(
                "summarize-patient",
                language,
                new Dictionary<string, string>
                {
                    ["patientName"] = patient.FullName,
                    ["dateOfBirth"] =
                        patient.DateOfBirth?.ToString("d") ?? "",
                    ["gender"] = patient.Gender ?? "",
                    ["contextText"] = contextText
                });

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(
                _prompts.Get(
                    "summarize-patient",
                    language)),

            new UserChatMessage(userPrompt)
        };

        var startTime = DateTime.UtcNow;

        var result =
            await _client.CompleteChatAsync(messages);

        var output =
            result.Value.Content[0].Text;

        await _observability.LogGenerationAsync(
            new LlmGenerationLog
            {
                Name = "summarize-patient",
                Model = _model,
                Input = userPrompt,
                Output = output,
                InputTokens = result.Value.Usage?.InputTokenCount,
                OutputTokens = result.Value.Usage?.OutputTokenCount,
                StartTime = startTime,
                EndTime = DateTime.UtcNow,
                Metadata = new Dictionary<string, object>
                {
                    ["patientId"] = patientId.ToString()
                }
            });

        return output;
    }

    public async Task<string> SummarizeSessionAsync(
        Guid sessionId,
        string language = "ar")
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

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(
                _prompts.Get(
                    "summarize-session",
                    language)),

            new UserChatMessage(userPrompt)
        };

        var startTime = DateTime.UtcNow;

        var result =
            await _client.CompleteChatAsync(messages);

        var output =
            result.Value.Content[0].Text;

        await _observability.LogGenerationAsync(
            new LlmGenerationLog
            {
                Name = "summarize-session",
                Model = _model,
                Input = userPrompt,
                Output = output,
                InputTokens = result.Value.Usage?.InputTokenCount,
                OutputTokens = result.Value.Usage?.OutputTokenCount,
                StartTime = startTime,
                EndTime = DateTime.UtcNow,
                Metadata = new Dictionary<string, object>
                {
                    ["sessionId"] = sessionId.ToString()
                }
            });

        return output;
    }
}