namespace Jalsa.Domain.Models.Patient;

public class Patient
{
    public Guid Id { get; set; }
    public Guid TherapistId { get; set; }
    public Guid? ClinicId { get; set; }
    public Guid? UserId { get; set; }
    public string FullName { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? ReferralSource { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? ProfileImageUrl { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Clinic.Therapist Therapist { get; set; } = null!;
    public Clinic.Clinic? Clinic { get; set; }
    public Identity.User? User { get; set; }
    public ICollection<PatientInvitation> PatientInvitations { get; set; } = new List<PatientInvitation>();
    public ICollection<IntakeForm> IntakeForms { get; set; } = new List<IntakeForm>();
    public ICollection<Session.Session> Sessions { get; set; } = new List<Session.Session>();
    public ICollection<Assessment.Assessment> Assessments { get; set; } = new List<Assessment.Assessment>();
    public ICollection<Exercise.Exercise> Exercises { get; set; } = new List<Exercise.Exercise>();
    public ICollection<Exercise.ExerciseLog> ExerciseLogs { get; set; } = new List<Exercise.ExerciseLog>();
    public ICollection<Chat.ChatConversation> ChatConversations { get; set; } = new List<Chat.ChatConversation>();
    public ICollection<Chat.AiChatLog> AiChatLogs { get; set; } = new List<Chat.AiChatLog>();
    public ICollection<Crisis.CrisisAlert> CrisisAlerts { get; set; } = new List<Crisis.CrisisAlert>();
    public ICollection<Report.ReferralReport> ReferralReports { get; set; } = new List<Report.ReferralReport>();
    public ICollection<Ai.AiArtifact> AiArtifacts { get; set; } = new List<Ai.AiArtifact>();
    public ICollection<Ai.AiReportGenerationLog> AiReportGenerationLogs { get; set; } = new List<Ai.AiReportGenerationLog>();
}
