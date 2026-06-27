namespace Jalsa.Application.DTOs.Session;

public class SessionViewDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid? IntakeFormId { get; set; }
    public int SessionNumber { get; set; }
    public DateOnly SessionDate { get; set; }
    public int? DurationMinutes { get; set; }
    public string? SessionType { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public SessionNoteViewDto? Note { get; set; }
}
