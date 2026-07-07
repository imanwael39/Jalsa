using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Jalsa.API.Configurations;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.Extensions.Options;

namespace Jalsa.API.Services.Implementations.AI;

public class GeminiClient : IGeminiClient
{
    private const int MaxAttempts = 3;
    private static readonly TimeSpan[] RetryDelays =
    {
        TimeSpan.FromMilliseconds(500),
        TimeSpan.FromMilliseconds(1500)
    };

    private readonly HttpClient _httpClient;
    private readonly GeminiSettings _settings;
    private readonly ILogger<GeminiClient> _logger;

    public GeminiClient(HttpClient httpClient, IOptions<GeminiSettings> settings, ILogger<GeminiClient> logger)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<string> ChatAsync(string systemPrompt, string userPrompt, int maxTokens = 1500)
    {
        var result = await ChatWithUsageAsync(systemPrompt, userPrompt, maxTokens);
        return result.Text;
    }

    public async Task<GeminiChatResult> ChatWithUsageAsync(string systemPrompt, string userPrompt, int maxTokens = 1500)
    {
        var url = $"models/{_settings.ChatModelId}:generateContent?key={_settings.ApiKey}";
        var payload = BuildChatPayload(systemPrompt, userPrompt, maxTokens);

        var raw = await SendWithRetryAsync(url, payload, "Gemini chat");

        using var doc = JsonDocument.Parse(raw);
        var root = doc.RootElement;

        var (inputTokens, outputTokens, totalTokens) = ParseUsage(root);

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
                return new GeminiChatResult
                {
                    Text = textEl.GetString() ?? string.Empty,
                    InputTokens = inputTokens,
                    OutputTokens = outputTokens,
                    TotalTokens = totalTokens ?? (inputTokens.HasValue && outputTokens.HasValue ? inputTokens + outputTokens : null)
                };
            }
        }

        throw new InvalidOperationException(
            $"Unrecognized Gemini chat response shape: {raw}");
    }

    public async IAsyncEnumerable<GeminiStreamChunk> ChatStreamAsync(
        string systemPrompt,
        string userPrompt,
        int maxTokens = 1500,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var url = $"models/{_settings.ChatModelId}:streamGenerateContent?alt=sse&key={_settings.ApiKey}";
        var payload = BuildChatPayload(systemPrompt, userPrompt, maxTokens);

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(payload)
        };

        using var response = await _httpClient.SendAsync(
            request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException(
                $"Gemini chat stream failed ({(int)response.StatusCode}): {errorBody}");
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        int? inputTokens = null;
        int? outputTokens = null;
        int? totalTokens = null;

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data: ", StringComparison.Ordinal))
                continue;

            var jsonPayload = line["data: ".Length..].Trim();
            if (jsonPayload is "[DONE]" or "") continue;

            using var doc = JsonDocument.Parse(jsonPayload);
            var root = doc.RootElement;

            var usage = ParseUsage(root);
            if (usage.InputTokens.HasValue) inputTokens = usage.InputTokens;
            if (usage.OutputTokens.HasValue) outputTokens = usage.OutputTokens;
            if (usage.TotalTokens.HasValue) totalTokens = usage.TotalTokens;

            string? delta = null;
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
                    delta = textEl.GetString();
                }
            }

            if (!string.IsNullOrEmpty(delta))
            {
                yield return new GeminiStreamChunk { TextDelta = delta };
            }
        }

        yield return new GeminiStreamChunk
        {
            IsFinal = true,
            InputTokens = inputTokens,
            OutputTokens = outputTokens,
            TotalTokens = totalTokens ?? (inputTokens.HasValue && outputTokens.HasValue ? inputTokens + outputTokens : null)
        };
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

        var raw = await SendWithRetryAsync(url, payload, "Gemini embed");

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

    private static object BuildChatPayload(string systemPrompt, string userPrompt, int maxTokens) => new
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

    private static (int? InputTokens, int? OutputTokens, int? TotalTokens) ParseUsage(JsonElement root)
    {
        if (!root.TryGetProperty("usageMetadata", out var usage))
            return (null, null, null);

        int? inputTokens = usage.TryGetProperty("promptTokenCount", out var promptTokens)
            ? promptTokens.GetInt32() : null;
        int? outputTokens = usage.TryGetProperty("candidatesTokenCount", out var candidateTokens)
            ? candidateTokens.GetInt32() : null;
        int? totalTokens = usage.TryGetProperty("totalTokenCount", out var total)
            ? total.GetInt32() : null;

        return (inputTokens, outputTokens, totalTokens);
    }

    /// <summary>
    /// Retries on transient failures (429, 5xx, timeouts, network errors) with exponential
    /// backoff. Non-transient failures (4xx other than 429) fail immediately since retrying
    /// a malformed request or bad API key will never succeed.
    /// </summary>
    private async Task<string> SendWithRetryAsync(string url, object payload, string operationName)
    {
        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsJsonAsync(url, payload);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or SocketException)
            {
                if (attempt == MaxAttempts) throw;
                _logger.LogWarning(ex, "{Operation} attempt {Attempt}/{Max} failed with a network error; retrying.",
                    operationName, attempt, MaxAttempts);
                await Task.Delay(RetryDelays[attempt - 1]);
                continue;
            }

            using (response)
            {
                var raw = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return raw;

                var isTransient = response.StatusCode == HttpStatusCode.TooManyRequests ||
                                   (int)response.StatusCode >= 500;

                if (!isTransient || attempt == MaxAttempts)
                {
                    throw new InvalidOperationException(
                        $"{operationName} failed ({(int)response.StatusCode}): {raw}");
                }

                _logger.LogWarning(
                    "{Operation} attempt {Attempt}/{Max} failed with {StatusCode}; retrying.",
                    operationName, attempt, MaxAttempts, (int)response.StatusCode);
                await Task.Delay(RetryDelays[attempt - 1]);
            }
        }

        throw new InvalidOperationException($"{operationName} failed after {MaxAttempts} attempts.");
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
