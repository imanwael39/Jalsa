namespace Jalsa.API.Services.Interfaces.AI;

public interface ILlmObservabilityService
{
    Task LogGenerationAsync(LlmGenerationLog log);
}

public class LlmGenerationLog
{
    public string Name { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Input { get; set; } = string.Empty;
    public string Output { get; set; } = string.Empty;
    public int? InputTokens { get; set; }
    public int? OutputTokens { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Error { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}
