namespace Jalsa.Domain.Models.Patient;

public class PatientInvitation
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string InvitedEmail { get; set; } = null!;
    public string InviteTokenHash { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public Patient Patient { get; set; } = null!;
}
