namespace Jalsa.Domain.Models.Assessment;

public class AssessmentResponse
{
    public Guid Id { get; set; }
    public Guid AssessmentId { get; set; }
    public Guid QuestionId { get; set; }
    public string? AnswerText { get; set; }
    public decimal? AnswerNumber { get; set; }
    public DateTime CreatedAt { get; set; }

    public Assessment Assessment { get; set; } = null!;
    public AssessmentQuestion Question { get; set; } = null!;
}
