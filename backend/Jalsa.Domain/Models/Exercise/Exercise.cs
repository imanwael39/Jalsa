namespace Jalsa.Domain.Models.Exercise;

public class Exercise
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string? Description { get; set; }
    public string? Frequency { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? DueDate { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Patient.Patient Patient { get; set; } = null!;
    public ICollection<ExerciseLog> ExerciseLogs { get; set; } = new List<ExerciseLog>();
}
