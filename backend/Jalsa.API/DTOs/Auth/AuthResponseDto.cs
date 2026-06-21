namespace Jalsa.API.DTOs.Auth;
public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiresAt { get; set; }
    public string Email { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];
}