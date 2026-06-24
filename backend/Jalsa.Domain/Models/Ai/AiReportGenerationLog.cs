namespace Jalsa.Domain.Models.Ai;

public class AiReportGenerationLog
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid? ReportId { get; set; }
    public string? SourceType { get; set; }
    public Guid? SourceId { get; set; }
    public string? ContentText { get; set; }
    public Guid GeneratedByTherapistId { get; set; }
    public int? TokensUsed { get; set; }
    public int? ProcessingTimeMs { get; set; }
    public decimal? CostEstimate { get; set; }
    public string? ModelUsed { get; set; }
    public DateTime CreatedAt { get; set; }

    public Patient.Patient Patient { get; set; } = null!;
    public Report.ReferralReport? Report { get; set; }
    public Clinic.Therapist GeneratedByTherapist { get; set; } = null!;
}
