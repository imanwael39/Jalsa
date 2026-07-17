namespace Jalsa.Domain.Models.Chat;

public class TherapistAiConversation
{
    public Guid Id { get; set; }
    public Guid TherapistId { get; set; }
    public Guid PatientId { get; set; }
    public string Status { get; set; } = "Open";
    public DateTime? LastActivityAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Clinic.Therapist Therapist { get; set; } = null!;
    public Patient.Patient Patient { get; set; } = null!;
    public ICollection<TherapistAiMessage> Messages { get; set; } = new List<TherapistAiMessage>();
    public ICollection<TherapistAiChatLog> ChatLogs { get; set; } = new List<TherapistAiChatLog>();
}
