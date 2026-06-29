namespace Jalsa.Application.DTOs.Chat;

public class ChatHistoryDto
{
    public Guid ConversationId { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public List<ChatMessageViewDto> Messages { get; set; } = new();
}
