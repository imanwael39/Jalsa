namespace Jalsa.Application.DTOs.Chat;

public class ConversationViewDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? LastActivityAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public int MessageCount { get; set; }
}
