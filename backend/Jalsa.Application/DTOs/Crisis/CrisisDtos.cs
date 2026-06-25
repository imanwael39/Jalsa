namespace Jalsa.Application.DTOs.Crisis;

public class CrisisAlertViewDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public Guid? TherapistId { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public string? MessageSnippet { get; set; }
    public Guid? ChatMessageId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
}
