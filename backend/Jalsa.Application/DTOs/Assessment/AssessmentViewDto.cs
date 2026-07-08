namespace Jalsa.Application.DTOs.Assessment;

public class AssessmentViewDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid? SessionId { get; set; }
    public Guid TemplateId { get; set; }
    public string? Title { get; set; }
    public DateOnly? AssessmentDate { get; set; }
    public decimal? TotalScore { get; set; }
    public string? Severity { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
