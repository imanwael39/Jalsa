namespace Jalsa.Domain.Models.Report;

public class ReferralReport
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid TherapistId { get; set; }
    public Guid GeneratedByTherapistId { get; set; }
    public string Status { get; set; } = "Draft";
    public Guid? CurrentVersionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Patient.Patient Patient { get; set; } = null!;
    public Clinic.Therapist Therapist { get; set; } = null!;
    public Clinic.Therapist GeneratedByTherapist { get; set; } = null!;
    public ReportVersion? CurrentVersion { get; set; }
    public ICollection<ReportVersion> Versions { get; set; } = new List<ReportVersion>();
    public ICollection<Ai.AiReportGenerationLog> AiReportGenerationLogs { get; set; } = new List<Ai.AiReportGenerationLog>();
}
