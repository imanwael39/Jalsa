namespace Jalsa.Domain.Models.Chat;

public class TherapistAiMessage
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public string SenderType { get; set; } = null!;
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public TherapistAiConversation Conversation { get; set; } = null!;
    public Ai.TherapistAiMemory? TriggeredMemory { get; set; }
}
