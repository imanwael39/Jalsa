namespace Jalsa.Application.DTOs.PatientSupportChat;

public class PatientSupportChatHistoryDto
{
    public Guid ConversationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<PatientSupportMessageViewDto> Messages { get; set; } = new();
}
