namespace Jalsa.Application.DTOs.PatientSupportChat;

public class PatientSupportConversationViewDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? LastActivityAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public int MessageCount { get; set; }
}
