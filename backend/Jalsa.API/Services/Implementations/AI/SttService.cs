using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;
using OpenAI.Audio;

namespace Jalsa.API.Services.Implementations.AI;

public class SttService : ISttService
{
    private readonly AudioClient _client;

    public SttService(IOptions<OpenAiSettings> settings)
    {
        var config = settings.Value;
        OpenAI.OpenAIClient openAi = string.IsNullOrWhiteSpace(config.Endpoint)
            ? new OpenAI.OpenAIClient(config.ApiKey)
            : new Azure.AI.OpenAI.AzureOpenAIClient(
                new Uri(config.Endpoint),
                new System.ClientModel.ApiKeyCredential(config.ApiKey));

        _client = openAi.GetAudioClient(config.SttModel);
    }

    public async Task<string> TranscribeAsync(Stream audioStream, string fileName, string language = "ar")
    {
        var options = new AudioTranscriptionOptions
        {
            Language = language,
            ResponseFormat = AudioTranscriptionFormat.Text
        };

        var result = await _client.TranscribeAudioAsync(audioStream, fileName, options);
        return result.Value.Text ?? string.Empty;
    }
}
