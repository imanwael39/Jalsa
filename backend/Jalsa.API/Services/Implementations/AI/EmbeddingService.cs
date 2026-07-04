using Jalsa.API.Services.Interfaces.AI;

namespace Jalsa.API.Services.Implementations.AI;

public class EmbeddingService : IEmbeddingService
{
    private readonly IGatewayClient _gatewayClient;

    public EmbeddingService(IGatewayClient gatewayClient)
    {
        _gatewayClient = gatewayClient;
    }

    public Task<float[]> GenerateEmbeddingAsync(string text)
    {
        return _gatewayClient.EmbedAsync(text);
    }
}
