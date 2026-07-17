namespace Jalsa.Domain.Models.Chat;

public class PatientSupportConversation
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string Status { get; set; } = "Open";
    public DateTime? LastActivityAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Patient.Patient Patient { get; set; } = null!;
    public ICollection<PatientSupportMessage> Messages { get; set; } = new List<PatientSupportMessage>();
    public ICollection<PatientSupportAiChatLog> ChatLogs { get; set; } = new List<PatientSupportAiChatLog>();
}
