namespace Jalsa.Domain.Models.Patient;

public class IntakeForm
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string? PresentingProblem { get; set; }
    public string? PsychiatricHistory { get; set; }
    public string? FamilyHistory { get; set; }
    public string? Medications { get; set; }
    public string? SocialHistory { get; set; }
    public string Status { get; set; } = "Draft";
    public DateTime CreatedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }

    public Patient Patient { get; set; } = null!;
    public ICollection<IntakeFormOcrExtraction> OcrExtractions { get; set; } = new List<IntakeFormOcrExtraction>();
    public ICollection<Session.Session> Sessions { get; set; } = new List<Session.Session>();
}
