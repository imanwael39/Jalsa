namespace Jalsa.Application.DTOs.Report;

public class ReportViewDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid TherapistId { get; set; }
    public Guid GeneratedByTherapistId { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid? CurrentVersionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ReportVersionViewDto? CurrentVersion { get; set; }
    public List<ReportVersionViewDto> Versions { get; set; } = new();
}
