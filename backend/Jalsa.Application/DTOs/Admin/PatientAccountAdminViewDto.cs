namespace Jalsa.Application.DTOs.Admin;

public class PatientAccountAdminViewDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string TherapistName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
