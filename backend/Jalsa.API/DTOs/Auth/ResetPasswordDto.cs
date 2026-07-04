using System.ComponentModel.DataAnnotations;

namespace Jalsa.API.DTOs.Auth;
public class ResetPasswordDto
{
   [Required, EmailAddress]
   public string Email { get; set; } = string.Empty;

   [Required]
   public string Otp { get; set; } = string.Empty;

   [Required, MinLength(8), RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
       ErrorMessage = "كلمة المرور يجب أن تحتوي على 8 أحرف على الأقل وحرف كبير وحرف صغير ورقم")]
   public string NewPassword { get; set; } = string.Empty;
}
