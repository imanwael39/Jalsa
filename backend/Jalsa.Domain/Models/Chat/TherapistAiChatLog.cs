namespace Jalsa.Domain.Models.Chat;

public class TherapistAiChatLog
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public Guid PatientId { get; set; }
    public int? TokensUsed { get; set; }
    public decimal? Cost { get; set; }
    public int? ResponseLatencyMs { get; set; }
    public string? ModelUsed { get; set; }
    public DateTime CreatedAt { get; set; }

    public TherapistAiConversation Conversation { get; set; } = null!;
    public Patient.Patient Patient { get; set; } = null!;
}
