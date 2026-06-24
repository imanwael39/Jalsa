namespace Jalsa.Domain.Models.Chat;

public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public string SenderType { get; set; } = null!;
    public string? Content { get; set; }
    public int? TokensUsed { get; set; }
    public int? LatencyMs { get; set; }
    public DateTime CreatedAt { get; set; }

    public ChatConversation Conversation { get; set; } = null!;
    public Ai.AiArtifact? TriggeredArtifact { get; set; }
    public Crisis.CrisisAlert? TriggeredCrisisAlert { get; set; }
}
