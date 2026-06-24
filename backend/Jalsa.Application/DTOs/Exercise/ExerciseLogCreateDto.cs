using System.ComponentModel.DataAnnotations;

namespace Jalsa.Application.DTOs.Exercise;

public class ExerciseLogCreateDto
{
    [Required]
    public Guid ExerciseId { get; set; }

    [Required]
    public Guid PatientId { get; set; }

    [Required]
    public string CompletionStatus { get; set; } = null!;

    [MaxLength(2000)]
    public string? ReflectionNote { get; set; }
}
