namespace Jalsa.Domain.Models.Report;

public class ReportVersion
{
    public Guid Id { get; set; }
    public Guid ReportId { get; set; }
    public int VersionNumber { get; set; }
    public string? Content { get; set; }
    public Guid CreatedByTherapistId { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ChangeNote { get; set; }
    public DateTime CreatedAt { get; set; }

    public ReferralReport Report { get; set; } = null!;
    public Clinic.Therapist CreatedByTherapist { get; set; } = null!;
}
