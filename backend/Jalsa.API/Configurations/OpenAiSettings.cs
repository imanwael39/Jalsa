namespace Jalsa.API.Configurations;

public class OpenAiSettings
{
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string EmbeddingModel { get; set; } = "text-embedding-3-small";
    public string ChatModel { get; set; } = "gpt-4o";
    public string VisionModel { get; set; } = "gpt-4o";
}
