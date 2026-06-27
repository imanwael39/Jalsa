namespace Jalsa.Application.DTOs.Report;

public class ReportGenerateDto
{
    public Guid PatientId { get; set; }
    public string? TherapistInstructions { get; set; }
    public string Language { get; set; } = "ar";
}
