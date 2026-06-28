using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace Jalsa.API.Services.Implementations.AI;

public class ReportGenerationService : IReportGenerationService
{
    private readonly ChatClient _client;
    private readonly JalsaDbContext _context;
    private readonly IVectorStore _vectorStore;
    private readonly IEmbeddingService _embeddingService;
    private readonly string _model;

    public ReportGenerationService(
        IOptions<OpenAiSettings> settings,
        JalsaDbContext context,
        IVectorStore vectorStore,
        IEmbeddingService embeddingService)
    {
        var config = settings.Value;
        _model = config.ChatModel;
        _context = context;
        _vectorStore = vectorStore;
        _embeddingService = embeddingService;

        OpenAI.OpenAIClient openAi = string.IsNullOrWhiteSpace(config.Endpoint)
            ? new OpenAI.OpenAIClient(config.ApiKey)
            : new Azure.AI.OpenAI.AzureOpenAIClient(
                new Uri(config.Endpoint),
                new System.ClientModel.ApiKeyCredential(config.ApiKey));

        _client = openAi.GetChatClient(_model);
    }

    public async Task<string> GenerateDraftAsync(Guid patientId, string? therapistInstructions = null, string language = "ar")
    {
        var patient = await _context.Patients
            .Include(p => p.IntakeForms)
            .Include(p => p.Assessments).ThenInclude(a => a.Template)
            .FirstOrDefaultAsync(p => p.Id == patientId);

        if (patient == null)
            return "Patient not found.";

        var queryVec = await _embeddingService.GenerateEmbeddingAsync(
            $"تقرير إحالة للمريض {patient.FullName}");
        var ragContext = await _vectorStore.SearchAsync(queryVec, topK: 5);

        var contextText = string.Join("\n\n", ragContext.Select(r => r.Text));
        var isArabic = language == "ar";
        var instructions = !string.IsNullOrWhiteSpace(therapistInstructions)
            ? (isArabic ? $"\nتعليمات المعالج: {therapistInstructions}" : $"\nTherapist instructions: {therapistInstructions}")
            : "";

        var prompt = isArabic
            ? $"""
            قم بإنشاء تقرير إحالة منظم للمريض {patient.FullName}.
            
            البيانات الديموغرافية: {patient.FullName}, تاريخ الميلاد: {patient.DateOfBirth}, الجنس: {patient.Gender}
            
            ملاحظات الجلسات ذات الصلة:
            {contextText}
            {instructions}
            
            قم بتضمين الأقسام التالية:
            1. معلومات المريض
            2. التاريخ السريري
            3. ملخص التقييم
            4. تقدم الجلسات
            5. التوصيات
            
            بتنسيق نص عادي مع عناوين أقسام واضحة.
            """
            : $"""
            Generate a structured referral report for patient {patient.FullName}.
            
            Demographics: {patient.FullName}, DOB: {patient.DateOfBirth}, Gender: {patient.Gender}
            
            Relevant session notes:
            {contextText}
            {instructions}
            
            Include these sections:
            1. Patient Information
            2. Clinical History
            3. Assessment Summary
            4. Session Progress
            5. Recommendations
            
            Format as plain text with clear section headers.
            """;

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(isArabic
                ? "أنت كاتب تقارير إكلينيكية. قم بإنشاء تقارير إحالة منظمة باللغة العربية."
                : "You are a clinical report writer. Generate structured referral reports in English."),
            new UserChatMessage(prompt)
        };

        var result = await _client.CompleteChatAsync(messages);
        return result.Value.Content[0].Text;
    }
}
