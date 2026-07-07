namespace Jalsa.Application.DTOs.PatientDashboard;

public class RecentConversationDto
{
    public Guid ConversationId { get; set; }
    public DateTime? LastActivityAt { get; set; }
    public string Status { get; set; } = "";
}
