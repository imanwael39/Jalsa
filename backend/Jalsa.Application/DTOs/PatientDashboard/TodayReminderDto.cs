namespace Jalsa.Application.DTOs.PatientDashboard;

public class TodayReminderDto
{
    public string Type { get; set; } = ""; // "Session" | "Exercise"
    public string Title { get; set; } = "";
    public Guid ReferenceId { get; set; }
}
