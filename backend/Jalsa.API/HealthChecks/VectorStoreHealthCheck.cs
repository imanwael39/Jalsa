using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Jalsa.API.HealthChecks;

public class VectorStoreHealthCheck : IHealthCheck
{
    private readonly IVectorStore _vectorStore;

    public VectorStoreHealthCheck(IVectorStore vectorStore)
    {
        _vectorStore = vectorStore;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken token = default)
    {
        try
        {
            var results = await _vectorStore.SearchAsync(
                new float[] { 0 }, topK: 1);
            return HealthCheckResult.Healthy($"VectorStore reachable. {results.Count} results.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("VectorStore is unreachable", ex);
        }
    }
}
