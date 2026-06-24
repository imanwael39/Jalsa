namespace Jalsa.Domain.Models.Assessment;

public class AssessmentQuestion
{
    public Guid Id { get; set; }
    public Guid TemplateId { get; set; }
    public string QuestionText { get; set; } = null!;
    public string? QuestionType { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }

    public AssessmentTemplate Template { get; set; } = null!;
    public ICollection<AssessmentResponse> Responses { get; set; } = new List<AssessmentResponse>();
}
