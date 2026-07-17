namespace Jalsa.Application.DTOs.CrisisAlert;

public class CrisisAlertViewDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public Guid? ConversationId { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public double? Confidence { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? TriggeringMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
