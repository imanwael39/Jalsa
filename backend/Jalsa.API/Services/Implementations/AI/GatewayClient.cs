using System.Net.Http.Json;
using System.Text.Json;
using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class GatewayClient : IGatewayClient
{
    private readonly HttpClient _httpClient;
    private readonly GatewaySettings _settings;

    public GatewayClient(HttpClient httpClient, IOptions<GatewaySettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<string> ChatAsync(string systemPrompt, string userPrompt, int maxTokens = 1500)
    {
        var payload = new
        {
            model_id = _settings.ChatModelId,
            messages = new[]
            {
                new { role = "user", content = userPrompt }
            },
            system_prompt = systemPrompt,
            max_tokens = maxTokens
        };

        using var response = await _httpClient.PostAsJsonAsync("student/chat", payload);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        var root = doc.RootElement;

        if (root.TryGetProperty("content", out var content) &&
            content.ValueKind == JsonValueKind.Array &&
            content.GetArrayLength() > 0 &&
            content[0].TryGetProperty("text", out var textEl))
        {
            return textEl.GetString() ?? string.Empty;
        }

        if (root.TryGetProperty("completion", out var completionEl))
            return completionEl.GetString() ?? string.Empty;

        if (root.TryGetProperty("message", out var messageEl) &&
            messageEl.TryGetProperty("content", out var messageContentEl))
        {
            return messageContentEl.GetString() ?? string.Empty;
        }

        if (root.TryGetProperty("text", out var plainTextEl))
            return plainTextEl.GetString() ?? string.Empty;

        throw new InvalidOperationException(
            $"Unrecognized gateway chat response shape: {root.GetRawText()}");
    }

    public async Task<float[]> EmbedAsync(string text, string inputType = "search_document")
    {
        var payload = new
        {
            model_id = _settings.EmbeddingModelId,
            texts = new[] { text },
            input_type = inputType
        };

        using var response = await _httpClient.PostAsJsonAsync("student/embed", payload);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        var root = doc.RootElement;

        if (root.TryGetProperty("embeddings", out var embeddingsEl) &&
            embeddingsEl.ValueKind == JsonValueKind.Array &&
            embeddingsEl.GetArrayLength() > 0)
        {
            return embeddingsEl[0].EnumerateArray().Select(e => e.GetSingle()).ToArray();
        }

        if (root.TryGetProperty("embedding", out var embeddingEl) &&
            embeddingEl.ValueKind == JsonValueKind.Array)
        {
            return embeddingEl.EnumerateArray().Select(e => e.GetSingle()).ToArray();
        }

        if (root.TryGetProperty("data", out var dataEl) &&
            dataEl.ValueKind == JsonValueKind.Array &&
            dataEl.GetArrayLength() > 0 &&
            dataEl[0].TryGetProperty("embedding", out var dataEmbeddingEl))
        {
            return dataEmbeddingEl.EnumerateArray().Select(e => e.GetSingle()).ToArray();
        }

        throw new InvalidOperationException(
            $"Unrecognized gateway embed response shape: {root.GetRawText()}");
    }
}
