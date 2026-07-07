namespace Jalsa.Application.DTOs.PatientDashboard;

public class UpcomingSessionDto
{
    public Guid Id { get; set; }
    public DateOnly SessionDate { get; set; }
    public string? SessionType { get; set; }
    public int? DurationMinutes { get; set; }
    public string Status { get; set; } = "";
}
