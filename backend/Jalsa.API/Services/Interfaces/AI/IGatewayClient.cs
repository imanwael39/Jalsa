namespace Jalsa.API.Services.Interfaces.AI;

public interface IGatewayClient
{
    Task<string> ChatAsync(string systemPrompt, string userPrompt, int maxTokens = 1500);

    Task<float[]> EmbedAsync(string text, string inputType = "search_document");
}
