namespace Jalsa.Domain.Models.Crisis;

public class CrisisAlert
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid? TherapistId { get; set; }
    public string Severity { get; set; } = null!;
    public Guid? ChatMessageId { get; set; }
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }

    public Patient.Patient Patient { get; set; } = null!;
    public Clinic.Therapist? Therapist { get; set; }
    public Chat.ChatMessage? ChatMessage { get; set; }
}
