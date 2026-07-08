namespace Jalsa.Application.DTOs.PatientAssessment;

public class PatientAssessmentSummaryDto
{
    public Guid Id { get; set; }
    public string TemplateName { get; set; } = "";
    public string? Title { get; set; }
    public string Status { get; set; } = "";
    public DateOnly? AssessmentDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public decimal? TotalScore { get; set; }
    public string? Severity { get; set; }
    public int QuestionCount { get; set; }
    public int AnsweredCount { get; set; }
}
