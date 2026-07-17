namespace Jalsa.Domain.Models.Ai;

public class TherapistAiMemory
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public Guid PatientId { get; set; }
    public Guid TriggerMessageId { get; set; }
    public string? ContentText { get; set; }
    public string? EmbeddingVector { get; set; }
    public DateTime CreatedAt { get; set; }

    public Chat.TherapistAiConversation Conversation { get; set; } = null!;
    public Patient.Patient Patient { get; set; } = null!;
    public Chat.TherapistAiMessage TriggerMessage { get; set; } = null!;
}
