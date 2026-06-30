using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class LangfuseObservabilityService : ILlmObservabilityService
{
    private readonly HttpClient _httpClient;
    private readonly LangfuseSettings _settings;
    private readonly ILogger<LangfuseObservabilityService> _logger;

    public LangfuseObservabilityService(
        HttpClient httpClient,
        IOptions<LangfuseSettings> settings,
        ILogger<LangfuseObservabilityService> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;

        if (_settings.Enabled)
        {
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{_settings.PublicKey}:{_settings.SecretKey}"));
            _httpClient.BaseAddress = new Uri(_settings.BaseUrl.TrimEnd('/'));
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", credentials);
        }
    }

    public async Task LogGenerationAsync(LlmGenerationLog log)
    {
        if (!_settings.Enabled)
            return;

        var traceId = Guid.NewGuid().ToString();
        var generationId = Guid.NewGuid().ToString();

        var payload = new
        {
            batch = new object[]
            {
                new
                {
                    id = traceId,
                    type = "trace-create",
                    timestamp = log.StartTime.ToString("o"),
                    body = new
                    {
                        id = traceId,
                        name = log.Name,
                        input = log.Input,
                        output = log.Output,
                        metadata = log.Metadata
                    }
                },
                new
                {
                    id = generationId,
                    type = "generation-create",
                    timestamp = log.StartTime.ToString("o"),
                    body = new
                    {
                        id = generationId,
                        traceId,
                        name = log.Name,
                        model = log.Model,
                        input = log.Input,
                        output = log.Output,
                        startTime = log.StartTime.ToString("o"),
                        endTime = log.EndTime.ToString("o"),
                        usage = new
                        {
                            input = log.InputTokens,
                            output = log.OutputTokens
                        },
                        metadata = log.Metadata
                    }
                }
            }
        };

        try
        {
            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/public/ingestion", content);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Langfuse ingestion failed: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send trace to Langfuse");
        }
    }
}
