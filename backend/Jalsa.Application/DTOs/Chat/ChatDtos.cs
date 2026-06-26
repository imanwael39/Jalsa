namespace Jalsa.Application.DTOs.Chat;

public class ChatSessionViewDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = "Open";
    public DateTime StartedAt { get; set; }
    public DateTime? LastActivityAt { get; set; }
}

public class ChatMessageViewDto
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public string SenderType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class ChatSendMessageDto
{
    public Guid SessionId { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ChatSendResponseDto
{
    public ChatMessageViewDto PatientMessage { get; set; } = null!;
    public ChatMessageViewDto AiMessage { get; set; } = null!;
}
