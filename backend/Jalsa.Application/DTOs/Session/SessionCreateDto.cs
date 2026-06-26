using System.ComponentModel.DataAnnotations;

namespace Jalsa.Application.DTOs.Session;

public class SessionCreateDto
{
    [Required]
    public Guid PatientId { get; set; }

    [Required]
    public DateOnly SessionDate { get; set; }

    public int? DurationMinutes { get; set; }

    [MaxLength(100)]
    public string? SessionType { get; set; }

    [MaxLength(50)]
    public string? Status { get; set; }

    public SessionNoteCreateDto? SessionNote { get; set; }
}

public class SessionNoteCreateDto
{
    [MaxLength(10000)]
    public string? Observations { get; set; }

    [MaxLength(10000)]
    public string? Interventions { get; set; }

    [MaxLength(10000)]
    public string? PatientResponse { get; set; }

    [MaxLength(10000)]
    public string? HomeworkAssigned { get; set; }

    [MaxLength(10000)]
    public string? NextGoals { get; set; }
}
