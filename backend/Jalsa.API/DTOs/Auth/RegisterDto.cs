using System.ComponentModel.DataAnnotations;

namespace Jalsa.API.DTOs.Auth;
public class RegisterDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8), RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "كلمة المرور يجب أن تحتوي على 8 أحرف على الأقل وحرف كبير وحرف صغير ورقم")]
    public string Password { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string? LicenseNumber { get; set; }

    public string? Specialization { get; set; }

    public string? Role { get; set; }
}