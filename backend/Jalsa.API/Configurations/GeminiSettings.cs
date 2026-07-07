namespace Jalsa.API.Configurations;

public class GeminiSettings
{
    public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta/";
    public string ApiKey { get; set; } = string.Empty;
    public string ChatModelId { get; set; } = "gemini-2.5-flash";
    public string EmbeddingModelId { get; set; } = "gemini-embedding-001";
    public int EmbeddingDimensions { get; set; } = 768;
}
