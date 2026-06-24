using System.ComponentModel.DataAnnotations;

namespace Jalsa.Application.DTOs.Exercise;

public class ExerciseCreateDto
{
    [Required]
    public Guid PatientId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = null!;

    public string? Frequency { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? DueDate { get; set; }
}
