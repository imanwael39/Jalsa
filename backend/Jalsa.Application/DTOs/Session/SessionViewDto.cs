namespace Jalsa.Application.DTOs.Session;

public class SessionViewDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string? PatientName { get; set; }
    public int SessionNumber { get; set; }
    public DateOnly SessionDate { get; set; }
    public int? DurationMinutes { get; set; }
    public string? SessionType { get; set; }
    public string Status { get; set; } = "Draft";
    public SessionNoteViewDto? SessionNote { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class SessionNoteViewDto
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public string? Observations { get; set; }
    public string? Interventions { get; set; }
    public string? PatientResponse { get; set; }
    public string? HomeworkAssigned { get; set; }
    public string? NextGoals { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
