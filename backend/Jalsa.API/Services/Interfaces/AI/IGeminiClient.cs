namespace Jalsa.API.Services.Interfaces.AI;

public class GeminiChatResult
{
    public string Text { get; set; } = string.Empty;
    public int? InputTokens { get; set; }
    public int? OutputTokens { get; set; }
    public int? TotalTokens { get; set; }
}

public class GeminiStreamChunk
{
    public string? TextDelta { get; set; }
    public bool IsFinal { get; set; }
    public int? InputTokens { get; set; }
    public int? OutputTokens { get; set; }
    public int? TotalTokens { get; set; }
}

public interface IGeminiClient
{
    Task<string> ChatAsync(string systemPrompt, string userPrompt, int maxTokens = 1500);

    Task<GeminiChatResult> ChatWithUsageAsync(string systemPrompt, string userPrompt, int maxTokens = 1500);

    IAsyncEnumerable<GeminiStreamChunk> ChatStreamAsync(
        string systemPrompt, string userPrompt, int maxTokens = 1500, CancellationToken cancellationToken = default);

    Task<float[]> EmbedAsync(string text, string inputType = "search_document");
}
