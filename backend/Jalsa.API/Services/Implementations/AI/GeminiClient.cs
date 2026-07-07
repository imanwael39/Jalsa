using System.Net.Http.Json;
using System.Text.Json;
using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class GeminiClient : IGeminiClient
{
    private readonly HttpClient _httpClient;
    private readonly GeminiSettings _settings;

    public GeminiClient(HttpClient httpClient, IOptions<GeminiSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }

    public async Task<string> ChatAsync(string systemPrompt, string userPrompt, int maxTokens = 1500)
    {
        var url = $"models/{_settings.ChatModelId}:generateContent?key={_settings.ApiKey}";

        var payload = new
        {
            contents = new[]
            {
                new { role = "user", parts = new[] { new { text = userPrompt } } }
            },
            systemInstruction = new
            {
                parts = new[] { new { text = systemPrompt } }
            },
            generationConfig = new
            {
                maxOutputTokens = maxTokens
            }
        };

        using var response = await _httpClient.PostAsJsonAsync(url, payload);
        var raw = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Gemini chat failed ({(int)response.StatusCode}): {raw}");
        }

        using var doc = JsonDocument.Parse(raw);
        var root = doc.RootElement;

        if (root.TryGetProperty("candidates", out var candidates) &&
            candidates.ValueKind == JsonValueKind.Array &&
            candidates.GetArrayLength() > 0)
        {
            var first = candidates[0];
            if (first.TryGetProperty("content", out var content) &&
                content.TryGetProperty("parts", out var parts) &&
                parts.ValueKind == JsonValueKind.Array &&
                parts.GetArrayLength() > 0 &&
                parts[0].TryGetProperty("text", out var textEl))
            {
                return textEl.GetString() ?? string.Empty;
            }
        }

        throw new InvalidOperationException(
            $"Unrecognized Gemini chat response shape: {raw}");
    }

    public async Task<float[]> EmbedAsync(string text, string inputType = "search_document")
    {
        var url = $"models/{_settings.EmbeddingModelId}:embedContent?key={_settings.ApiKey}";

        var payload = new
        {
            model = $"models/{_settings.EmbeddingModelId}",
            content = new
            {
                parts = new[] { new { text } }
            },
            taskType = MapTaskType(inputType),
            outputDimensionality = _settings.EmbeddingDimensions
        };

        using var response = await _httpClient.PostAsJsonAsync(url, payload);
        var raw = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Gemini embed failed ({(int)response.StatusCode}): {raw}");
        }

        using var doc = JsonDocument.Parse(raw);
        var root = doc.RootElement;

        if (root.TryGetProperty("embedding", out var embedding) &&
            embedding.TryGetProperty("values", out var values) &&
            values.ValueKind == JsonValueKind.Array)
        {
            return values.EnumerateArray().Select(e => e.GetSingle()).ToArray();
        }

        throw new InvalidOperationException(
            $"Unrecognized Gemini embed response shape: {raw}");
    }

    private static string MapTaskType(string inputType) => inputType switch
    {
        "search_query" or "query" => "RETRIEVAL_QUERY",
        "search_document" or "document" => "RETRIEVAL_DOCUMENT",
        "semantic_similarity" => "SEMANTIC_SIMILARITY",
        "classification" => "CLASSIFICATION",
        "clustering" => "CLUSTERING",
        "question_answering" => "QUESTION_ANSWERING",
        _ => "RETRIEVAL_DOCUMENT"
    };
}
