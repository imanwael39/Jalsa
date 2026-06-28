namespace Jalsa.Application.DTOs.Patient;

public class PatientViewDto
{
    public Guid Id { get; set; }
    public Guid TherapistId { get; set; }
    public Guid? ClinicId { get; set; }
    public Guid? UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? ReferralSource { get; set; }
    public string? ChiefComplaint { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
