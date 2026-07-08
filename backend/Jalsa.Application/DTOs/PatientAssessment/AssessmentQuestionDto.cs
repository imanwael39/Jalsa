namespace Jalsa.Application.DTOs.PatientAssessment;

public class AssessmentQuestionDto
{
    public Guid Id { get; set; }
    public string QuestionText { get; set; } = "";
    public string? QuestionType { get; set; }
    public int SortOrder { get; set; }
    public decimal? AnswerNumber { get; set; }
}
