namespace Jalsa.Application.DTOs.PatientDashboard;

public class AssignedExerciseSummaryDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public string? Frequency { get; set; }
    public DateOnly? DueDate { get; set; }
    public string Status { get; set; } = "";
    public bool IsOverdue { get; set; }
}
