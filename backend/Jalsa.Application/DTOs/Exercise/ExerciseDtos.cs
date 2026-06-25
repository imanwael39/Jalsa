namespace Jalsa.Application.DTOs.Exercise;

public class PatientExerciseViewDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public string? Frequency { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? DueDate { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
}

public class ExerciseLogViewDto
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public string? CompletionStatus { get; set; }
    public string? ReflectionNote { get; set; }
    public DateTime LoggedAt { get; set; }
}

public class ExerciseStatusUpdateDto
{
    public string Status { get; set; } = string.Empty;
    public string? ReflectionNote { get; set; }
}
