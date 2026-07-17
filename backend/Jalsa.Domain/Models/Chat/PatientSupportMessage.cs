namespace Jalsa.Domain.Models.Chat;

public class PatientSupportMessage
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public string SenderType { get; set; } = null!;
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public PatientSupportConversation Conversation { get; set; } = null!;
    public Ai.PatientSupportMemory? TriggeredMemory { get; set; }
    public Crisis.CrisisAlert? TriggeredCrisisAlert { get; set; }
}
