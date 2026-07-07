namespace Jalsa.Application.DTOs.Admin;

public class TherapistAdminViewDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string? Specialization { get; set; }
    public string ApprovalStatus { get; set; } = string.Empty;
    public DateTime? ApprovalStatusUpdatedAt { get; set; }
    public int PatientCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
