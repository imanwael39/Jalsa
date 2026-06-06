namespace Jalsa.Domain.Models.Session;

public class SessionNote
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

    public Session Session { get; set; } = null!;
}
