namespace Jalsa.Domain.Models.Crisis;

public class CrisisAlert
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid? TherapistId { get; set; }
    public Guid? ConversationId { get; set; }
    public string Severity { get; set; } = null!;
    public string? Reason { get; set; }
    public double? Confidence { get; set; }
    public Guid? ChatMessageId { get; set; }
    public string Status { get; set; } = "New";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Patient.Patient Patient { get; set; } = null!;
    public Chat.PatientSupportMessage? ChatMessage { get; set; }
}
