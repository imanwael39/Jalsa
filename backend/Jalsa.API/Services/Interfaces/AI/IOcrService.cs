namespace Jalsa.API.Services.Interfaces.AI;

public class OcrResult
{
    public string? ExtractedJson { get; set; }
    public string? RawText { get; set; }
    public decimal? Confidence { get; set; }
    public string? ModelUsed { get; set; }
}

public interface IOcrService
{
    Task<OcrResult> ExtractFromImageAsync(string imageUrl);
}
