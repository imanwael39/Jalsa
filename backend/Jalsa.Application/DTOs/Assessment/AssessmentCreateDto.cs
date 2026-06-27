namespace Jalsa.Application.DTOs.Assessment;

public class AssessmentCreateDto
{
    public Guid? SessionId { get; set; }
    public string TemplateId { get; set; } = string.Empty;
    public string? Title { get; set; }
    public DateOnly? AssessmentDate { get; set; }
    public decimal? TotalScore { get; set; }
}
