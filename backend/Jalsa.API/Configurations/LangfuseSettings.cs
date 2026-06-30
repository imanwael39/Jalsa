namespace Jalsa.API.Configurations;

public class LangfuseSettings
{
    public bool Enabled { get; set; }
    public string BaseUrl { get; set; } = "https://cloud.langfuse.com";
    public string PublicKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}
