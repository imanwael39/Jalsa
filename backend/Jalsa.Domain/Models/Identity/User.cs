using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Audit;
using Jalsa.Domain.Models.File;

namespace Jalsa.Domain.Models.Identity;

using PatientEntity = Patient.Patient;
using NotificationEntity = Notification.Notification;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
    public Therapist? Therapist { get; set; }
    public PatientEntity? Patient { get; set; }
    public ICollection<NotificationEntity> Notifications { get; set; } = new List<NotificationEntity>();
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    public ICollection<UploadedFile> UploadedFiles { get; set; } = new List<UploadedFile>();
}
