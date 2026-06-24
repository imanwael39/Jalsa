namespace Jalsa.Application.DTOs.Exercise;

public class ExerciseViewDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string? Description { get; set; }
    public string? Frequency { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? DueDate { get; set; }
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
