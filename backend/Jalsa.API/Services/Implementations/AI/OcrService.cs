using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace Jalsa.API.Services.Implementations.AI;

public class OcrService : IOcrService
{
    private readonly ChatClient _client;
    private readonly string _model;
    private readonly Services.Interfaces.IPromptService _prompts;

    public OcrService(IOptions<OpenAiSettings> settings, Services.Interfaces.IPromptService prompts)
    {
        _prompts = prompts;
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
        var prompt = _prompts.Get("ocr-extract", "ar");

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(_prompts.Get("ocr-extract")),
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
