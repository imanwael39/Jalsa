namespace Jalsa.Application.DTOs.TherapistChat;

public class TherapistChatHistoryDto
{
    public Guid ConversationId { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public List<TherapistChatMessageViewDto> Messages { get; set; } = new();
}
