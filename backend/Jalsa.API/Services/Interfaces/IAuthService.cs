using Jalsa.API.DTOs.Auth;
namespace Jalsa.API.Services.Interfaces;
public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto);
    Task RevokeTokenAsync(RevokeTokenRequestDto dto);
    Task ForgotPasswordAsync(ForgotPasswordDto dto);
    Task VerifyOtpAsync(VerifyOtpDto dto);
    Task ResetPasswordAsync(ResetPasswordDto dto);
    Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto);
}
