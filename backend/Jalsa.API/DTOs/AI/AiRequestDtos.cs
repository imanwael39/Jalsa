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

public class ChatDiagnosticsRequest
{
    public Guid ConversationId { get; set; }
    public Guid PatientId { get; set; }
    public string Question { get; set; } = string.Empty;
    public string? Language { get; set; }
}
