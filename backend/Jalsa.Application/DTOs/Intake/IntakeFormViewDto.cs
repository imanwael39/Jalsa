namespace Jalsa.Application.DTOs.Intake;

public class IntakeFormViewDto
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
}
