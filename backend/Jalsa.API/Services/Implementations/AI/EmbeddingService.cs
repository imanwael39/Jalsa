using Jalsa.API.Services.Interfaces.AI;

namespace Jalsa.API.Services.Implementations.AI;

public class EmbeddingService : IEmbeddingService
{
    private readonly IGeminiClient _geminiClient;
    private readonly ILogger<EmbeddingService> _logger;

    public EmbeddingService(IGeminiClient geminiClient, ILogger<EmbeddingService> logger)
    {
        _geminiClient = geminiClient;
        _logger = logger;
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text, string inputType = "search_document")
    {
        if (string.IsNullOrWhiteSpace(text))
            return Array.Empty<float>();

        try
        {
            return await _geminiClient.EmbedAsync(text, inputType);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Embedding generation failed; returning empty vector.");
            return Array.Empty<float>();
        }
    }
}
