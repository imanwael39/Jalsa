using System.ComponentModel.DataAnnotations;

namespace Jalsa.Application.DTOs.Patient;

public class PatientCreateDto
{
    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    public DateOnly? DateOfBirth { get; set; }

    [MaxLength(20)]
    public string? Gender { get; set; }
    [MaxLength(20)]
    public string? Phone { get; set; }

    [EmailAddress, MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(200)]
    public string? ReferralSource { get; set; }

    [MaxLength(1000)]
    public string? ChiefComplaint { get; set; }
}
