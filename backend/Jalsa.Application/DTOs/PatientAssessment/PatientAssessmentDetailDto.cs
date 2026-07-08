namespace Jalsa.Application.DTOs.PatientAssessment;

public class PatientAssessmentDetailDto
{
    public Guid Id { get; set; }
    public string TemplateName { get; set; } = "";
    public string? Title { get; set; }
    public string Status { get; set; } = "";
    public DateOnly? AssessmentDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public decimal? TotalScore { get; set; }
    public string? Severity { get; set; }
    public List<AssessmentQuestionDto> Questions { get; set; } = new();
}
