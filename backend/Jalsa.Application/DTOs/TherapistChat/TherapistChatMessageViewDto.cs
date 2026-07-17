namespace Jalsa.Application.DTOs.TherapistChat;

public class TherapistChatMessageViewDto
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public string SenderType { get; set; } = string.Empty;
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
}
