namespace Jalsa.API.DTOs.Auth;

/// <summary>Lightweight, public-facing therapist entry for the registration page's therapist picker.</summary>
public class TherapistOptionDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Specialization { get; set; }
}
