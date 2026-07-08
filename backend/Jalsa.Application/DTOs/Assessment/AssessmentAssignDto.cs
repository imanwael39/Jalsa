namespace Jalsa.Application.DTOs.Assessment;

public class AssessmentAssignDto
{
    public Guid? SessionId { get; set; }
    public string TemplateId { get; set; } = string.Empty;
    public string? Title { get; set; }
}
