namespace Jalsa.Application.DTOs.Exercise;

public class ExerciseLogViewDto
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public Guid PatientId { get; set; }
    public string CompletionStatus { get; set; } = null!;
    public string? ReflectionNote { get; set; }
    public DateTime? LoggedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
