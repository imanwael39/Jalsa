namespace Jalsa.Domain.Models.Clinic;

public class Therapist
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string LicenseNumber { get; set; } = null!;
    public string? Specialization { get; set; }
    public string? Phone { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string? Bio { get; set; }
    public string ApprovalStatus { get; set; } = TherapistApprovalStatus.Pending;
    public DateTime? ApprovalStatusUpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public Identity.User User { get; set; } = null!;
    public ICollection<TherapistClinic> TherapistClinics { get; set; } = new List<TherapistClinic>();
    public ICollection<Patient.Patient> Patients { get; set; } = new List<Patient.Patient>();
    public ICollection<Chat.TherapistAiConversation> TherapistAiConversations { get; set; } = new List<Chat.TherapistAiConversation>();
    public ICollection<Report.ReferralReport> ReferralReports { get; set; } = new List<Report.ReferralReport>();
    public ICollection<Report.ReportVersion> ReportVersions { get; set; } = new List<Report.ReportVersion>();
    public ICollection<Ai.AiReportGenerationLog> AiReportGenerationLogs { get; set; } = new List<Ai.AiReportGenerationLog>();
}
