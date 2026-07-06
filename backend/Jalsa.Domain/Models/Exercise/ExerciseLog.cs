namespace Jalsa.Domain.Models.Exercise;

public class ExerciseLog
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public Guid PatientId { get; set; }
    public string CompletionStatus { get; set; } = null!;
    public string? ReflectionNote { get; set; }
    public int? MoodBefore { get; set; }
    public int? MoodAfter { get; set; }
    public DateTime? LoggedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public Exercise Exercise { get; set; } = null!;
    public Patient.Patient Patient { get; set; } = null!;
}
