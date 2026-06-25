using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace Jalsa.API.Services.Implementations.AI;

public class OcrService : IOcrService
{
    private readonly ChatClient _client;
    private readonly string _model;

    public OcrService(IOptions<OpenAiSettings> settings)
    {
        var config = settings.Value;
        _model = config.VisionModel;

        OpenAI.OpenAIClient openAi = string.IsNullOrWhiteSpace(config.Endpoint)
            ? new OpenAI.OpenAIClient(config.ApiKey)
            : new Azure.AI.OpenAI.AzureOpenAIClient(
                new Uri(config.Endpoint),
                new System.ClientModel.ApiKeyCredential(config.ApiKey));

        _client = openAi.GetChatClient(_model);
    }

    public async Task<OcrResult> ExtractFromImageAsync(string imageUrl)
    {
        var prompt = """
            استخرج الحقول التالية من صورة نموذج القبول وأعدها ككائن JSON.
            Extract the following fields from this intake form image and return them as a JSON object:
            - FullName (الاسم الكامل)
            - DateOfBirth (تاريخ الميلاد)
            - Gender (الجنس)
            - Phone (رقم الهاتف)
            - Email (البريد الإلكتروني)
            - Address (العنوان)
            - EmergencyContact (جهة اتصال الطوارئ)
            - ReasonForVisit (سبب الزيارة)
            - MedicalHistory (التاريخ الطبي)
            - Medications (الأدوية)
            - Allergies (الحساسية)
            - InsuranceProvider (شركة التأمين)
            - InsuranceId (رقم التأمين)
            Return field names in English. Return ONLY valid JSON, no markdown, no explanation.
            أعد أسماء الحقول بالإنجليزية. أعد ONLY JSON صالحًا، بدون markdown أو شرح.
            """;

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are a medical intake form OCR assistant. Extract structured data from form images in any language. / أنت مساعد استخراج بيانات من نماذج القبول الطبية. استخرج البيانات المنظمة من صور النماذج بأي لغة."),
            new UserChatMessage(
                ChatMessageContentPart.CreateTextPart(prompt),
                ChatMessageContentPart.CreateImagePart(new Uri(imageUrl), ChatImageDetailLevel.High)
            )
        };

        var result = await _client.CompleteChatAsync(messages);
        var text = result.Value.Content[0].Text;

        return new OcrResult
        {
            ExtractedJson = text,
            RawText = text,
            ModelUsed = _model,
            Confidence = 0.85m
        };
    }
}
