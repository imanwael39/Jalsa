using System.ComponentModel.DataAnnotations;

namespace Jalsa.API.DTOs.Auth;

public class RevokeTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
