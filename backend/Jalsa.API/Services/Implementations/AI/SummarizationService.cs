using Jalsa.API.Configurations;
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

    public SummarizationService(
        IOptions<OpenAiSettings> settings,
        JalsaDbContext context,
        IVectorStore vectorStore,
        IEmbeddingService embeddingService)
    {
        var config = settings.Value;
        _context = context;
        _vectorStore = vectorStore;
        _embeddingService = embeddingService;

        OpenAI.OpenAIClient openAi = string.IsNullOrWhiteSpace(config.Endpoint)
            ? new OpenAI.OpenAIClient(config.ApiKey)
            : new Azure.AI.OpenAI.AzureOpenAIClient(
                new Uri(config.Endpoint),
                new System.ClientModel.ApiKeyCredential(config.ApiKey));

        _client = openAi.GetChatClient(config.ChatModel);
    }

    public async Task<string> SummarizePatientAsync(Guid patientId, string language = "ar")
    {
        var patient = await _context.Patients
            .Include(p => p.IntakeForms)
            .Include(p => p.Assessments).ThenInclude(a => a.Template)
            .FirstOrDefaultAsync(p => p.Id == patientId);

        if (patient == null)
            return "Patient not found.";

        var queryVec = await _embeddingService.GenerateEmbeddingAsync(
            $"ملخص التاريخ السريري للمريض {patient.FullName}");
        var ragContext = await _vectorStore.SearchAsync(queryVec, topK: 5, metadataFilter: null);

        var contextText = string.Join("\n\n", ragContext.Select(r => r.Text));

        var isArabic = language == "ar";

        var prompt = isArabic
            ? $"""
            قدم ملخصًا سريريًا للمريض {patient.FullName}.
            
            البيانات الديموغرافية: {patient.FullName}, تاريخ الميلاد: {patient.DateOfBirth}, الجنس: {patient.Gender}
            
            ملاحظات الجلسات ذات الصلة:
            {contextText}
            
            قم بتضمين: ملخص ديموغرافي، الموضوعات السريرية الرئيسية، اتجاهات التقييم، وملاحظات التقدم.
            """
            : $"""
            Provide a clinical summary of patient {patient.FullName}.
            
            Demographics: {patient.FullName}, DOB: {patient.DateOfBirth}, Gender: {patient.Gender}
            
            Relevant session notes:
            {contextText}
            
            Include: demographic summary, key clinical themes, assessment trends, and progress notes.
            """;

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(isArabic
                ? "أنت مساعد تلخيص إكلينيكي. قدم ملخصات منظمة وموجزة باللغة العربية."
                : "You are a clinical summarization assistant. Provide concise, structured summaries in English."),
            new UserChatMessage(prompt)
        };

        var result = await _client.CompleteChatAsync(messages);
        return result.Value.Content[0].Text;
    }

    public async Task<string> SummarizeSessionAsync(Guid sessionId, string language = "ar")
    {
        var session = await _context.Sessions
            .Include(s => s.SessionNote)
            .Include(s => s.VoiceMemos)
            .FirstOrDefaultAsync(s => s.Id == sessionId);

        if (session is null)
            return language == "ar" ? "الجلسة غير موجودة." : "Session not found.";

        var n = session.SessionNote;
        var note = n is null ? string.Empty : string.Join("\n",
            new[] {
                n.Observations is not null ? $"الملاحظات: {n.Observations}" : null,
                n.Interventions is not null ? $"التدخلات: {n.Interventions}" : null,
                n.PatientResponse is not null ? $"استجابة المريض: {n.PatientResponse}" : null,
                n.HomeworkAssigned is not null ? $"الواجبات المنزلية: {n.HomeworkAssigned}" : null,
                n.NextGoals is not null ? $"الأهداف التالية: {n.NextGoals}" : null
            }.Where(x => x is not null));
        var voiceTranscripts = string.Join("\n", session.VoiceMemos.Select(v => v.Transcript ?? string.Empty));

        var isArabic = language == "ar";

        var prompt = isArabic
            ? $"""
            قدم ملخصًا للجلسة العلاجية التالية:

            التاريخ: {session.SessionDate:dd/MM/yyyy}
            المدة: {session.DurationMinutes} دقيقة
            النوع: {session.SessionType}

            ملاحظات الجلسة:
            {note}

            {(string.IsNullOrWhiteSpace(voiceTranscripts) ? "" : $"نصوص التسجيلات الصوتية:\n{voiceTranscripts}")}

            قدم ملخصًا موجزًا يشمل: النقاط الرئيسية، التدخلات المستخدمة، والخطوات التالية المقترحة.
            """
            : $"""
            Summarize the following therapy session:

            Date: {session.SessionDate:dd/MM/yyyy}
            Duration: {session.DurationMinutes} minutes
            Type: {session.SessionType}

            Session Notes:
            {note}

            {(string.IsNullOrWhiteSpace(voiceTranscripts) ? "" : $"Voice Transcripts:\n{voiceTranscripts}")}

            Provide a concise summary covering: key points discussed, interventions used, and suggested next steps.
            """;

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(isArabic
                ? "أنت مساعد تلخيص إكلينيكي. قدم ملخصات موجزة ومنظمة باللغة العربية."
                : "You are a clinical summarization assistant. Provide concise, structured session summaries."),
            new UserChatMessage(prompt)
        };

        var result = await _client.CompleteChatAsync(messages);
        return result.Value.Content[0].Text;
    }
}
