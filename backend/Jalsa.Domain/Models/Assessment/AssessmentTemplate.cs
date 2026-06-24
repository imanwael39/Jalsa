namespace Jalsa.Domain.Models.Assessment;

public class AssessmentTemplate
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Version { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<AssessmentQuestion> Questions { get; set; } = new List<AssessmentQuestion>();
    public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
}
