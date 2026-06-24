using System.ComponentModel.DataAnnotations;

namespace Jalsa.Application.DTOs.Exercise;

public class ExerciseUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public string? Frequency { get; set; }

    public DateOnly? DueDate { get; set; }

    public string? Status { get; set; }
}
