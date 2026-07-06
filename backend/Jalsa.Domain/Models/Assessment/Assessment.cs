namespace Jalsa.Domain.Models.Assessment;

public class Assessment
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid? SessionId { get; set; }
    public Guid TemplateId { get; set; }
    public string? Title { get; set; }
    public DateOnly? AssessmentDate { get; set; }
    public decimal? TotalScore { get; set; }
    public string? Severity { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Patient.Patient Patient { get; set; } = null!;
    public Session.Session? Session { get; set; }
    public AssessmentTemplate Template { get; set; } = null!;
    public ICollection<AssessmentResponse> Responses { get; set; } = new List<AssessmentResponse>();
}
