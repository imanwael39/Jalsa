namespace Jalsa.Application.DTOs.PatientDashboard;

public class PendingAssessmentDto
{
    public Guid Id { get; set; }
    public string TemplateName { get; set; } = "";
    public DateOnly? AssignedDate { get; set; }
}
