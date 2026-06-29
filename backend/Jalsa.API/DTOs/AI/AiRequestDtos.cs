namespace Jalsa.API.DTOs.AI;

public class SummarizeRequest
{
    public string Language { get; set; } = "ar";
}

public class ReportDraftRequest
{
    public string? TherapistInstructions { get; set; }
    public string Language { get; set; } = "ar";
}
