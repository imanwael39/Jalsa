namespace Jalsa.API.Configurations;

public class GatewaySettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ChatModelId { get; set; } = "anthropic.claude-3-haiku-20240307-v1:0";
    public string EmbeddingModelId { get; set; } = "amazon.titan-embed-text-v2:0";
}
