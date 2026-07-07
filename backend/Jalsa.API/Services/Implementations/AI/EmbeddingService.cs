using Jalsa.API.Services.Interfaces.AI;

namespace Jalsa.API.Services.Implementations.AI;

public class EmbeddingService : IEmbeddingService
{
    private readonly IGeminiClient _geminiClient;

    public EmbeddingService(IGeminiClient geminiClient)
    {
        _geminiClient = geminiClient;
    }

    public Task<float[]> GenerateEmbeddingAsync(string text)
    {
        return _geminiClient.EmbedAsync(text);
    }
}
