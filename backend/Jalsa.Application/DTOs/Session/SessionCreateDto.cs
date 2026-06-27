namespace Jalsa.Application.DTOs.Session;

public class SessionCreateDto
{
    public Guid PatientId { get; set; }
    public Guid? IntakeFormId { get; set; }
    public DateOnly SessionDate { get; set; }
    public int? DurationMinutes { get; set; }
    public string? SessionType { get; set; }
}
