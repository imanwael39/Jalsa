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
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
