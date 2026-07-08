namespace Jalsa.Application.DTOs.PatientSession;

public class PatientSessionDetailDto
{
    public Guid Id { get; set; }
    public int SessionNumber { get; set; }
    public DateOnly SessionDate { get; set; }
    public int? DurationMinutes { get; set; }
    public string? SessionType { get; set; }
    public string Status { get; set; } = "";
    public string TherapistName { get; set; } = "";
    public string? PatientRequestType { get; set; }
    public string? PatientRequestNote { get; set; }
    public string? PatientRequestStatus { get; set; }
    public DateTime? PatientRequestedAt { get; set; }
    public bool CanRequestChange { get; set; }
}
