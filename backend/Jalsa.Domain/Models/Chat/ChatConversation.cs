namespace Jalsa.Domain.Models.Chat;

public class ChatConversation
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string Status { get; set; } = "Open";
    public DateTime? LastActivityAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Patient.Patient Patient { get; set; } = null!;
    public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    public ICollection<AiChatLog> AiChatLogs { get; set; } = new List<AiChatLog>();
    public ICollection<Ai.AiArtifact> AiArtifacts { get; set; } = new List<Ai.AiArtifact>();
}
