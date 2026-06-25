using Jalsa.API.Configurations;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace Jalsa.API.HealthChecks;

public class OpenAIHealthCheck : IHealthCheck
{
    private readonly ChatClient _client;

    public OpenAIHealthCheck(IOptions<OpenAiSettings> settings)
    {
        var config = settings.Value;
        OpenAI.OpenAIClient openAi = string.IsNullOrWhiteSpace(config.Endpoint)
            ? new OpenAI.OpenAIClient(config.ApiKey)
            : new Azure.AI.OpenAI.AzureOpenAIClient(
                new Uri(config.Endpoint),
                new System.ClientModel.ApiKeyCredential(config.ApiKey));
        _client = openAi.GetChatClient(config.ChatModel);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken token = default)
    {
        try
        {
            var result = await _client.CompleteChatAsync(
                [new UserChatMessage("Respond with OK")], cancellationToken: token);
            return result.Value.Content[0].Text.Contains("OK", StringComparison.OrdinalIgnoreCase)
                ? HealthCheckResult.Healthy("OpenAI API is reachable")
                : HealthCheckResult.Degraded("OpenAI API returned unexpected response");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("OpenAI API is unreachable", ex);
        }
    }
}
