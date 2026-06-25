namespace Jalsa.API.Services.Interfaces.AI;

public class CrisisDetectionResult
{
    public bool IsCrisis { get; set; }
    public string? Reason { get; set; }
    public string? SuggestedMessage { get; set; }
}

public interface ICrisisDetectionService
{
    Task<CrisisDetectionResult> AnalyzeAsync(string message);
}
