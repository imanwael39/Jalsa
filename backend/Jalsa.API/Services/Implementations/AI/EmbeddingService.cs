using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class EmbeddingService : IEmbeddingService
{
    private readonly OpenAI.Embeddings.EmbeddingClient _client;

    public EmbeddingService(IOptions<OpenAiSettings> settings)
    {
        var config = settings.Value;

        if (string.IsNullOrWhiteSpace(config.Endpoint))
        {
            var openAi = new OpenAI.OpenAIClient(config.ApiKey);
            _client = openAi.GetEmbeddingClient(config.EmbeddingModel);
        }
        else
        {
            var openAi = new Azure.AI.OpenAI.AzureOpenAIClient(
                new Uri(config.Endpoint),
                new System.ClientModel.ApiKeyCredential(config.ApiKey));
            _client = openAi.GetEmbeddingClient(config.EmbeddingModel);
        }
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        var result = await _client.GenerateEmbeddingAsync(text);
        return result.Value.ToFloats().ToArray();
    }
}
