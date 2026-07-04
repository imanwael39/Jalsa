using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.DTOs.Auth;
using Jalsa.API.Exceptions;
using Jalsa.API.Services.Interfaces;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Identity;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthController(IAuthService authService, IUnitOfWork unitOfWork)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
        var result = await _authService.RefreshTokenAsync(dto);
        return Ok(result);
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RevokeTokenRequestDto dto)
    {
        await _authService.RevokeTokenAsync(dto);
        return NoContent();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
    {
        await _authService.ForgotPasswordAsync(dto);
        return Ok(new { message = "If the email exists, an OTP has been sent." });
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
    {
        await _authService.VerifyOtpAsync(dto);
        return Ok(new { message = "رمز التحقق صحيح." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        await _authService.ResetPasswordAsync(dto);
        return Ok(new { message = "Password has been reset successfully." });
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetCurrentUserId();
        var userRepo = _unitOfWork.Repository<User>();
        var user = await userRepo.GetByIdAsync(userId)
            ?? throw new ApiException(404, "User not found");

        var therapistRepo = _unitOfWork.Repository<Therapist>();
        var therapist = await therapistRepo.FindSingleAsync(t => t.UserId == userId);

        var roleRepo = _unitOfWork.Repository<UserRole>();
        var userRoles = await roleRepo.FindAsync(ur => ur.UserId == userId);
        var roleIdRepo = _unitOfWork.Repository<Role>();
        var allRoles = await roleIdRepo.GetAllAsync();
        var roleNames = userRoles
            .Join(allRoles, ur => ur.RoleId, r => r.Id, (_, r) => r.Name)
            .ToList();

        var (firstName, lastName) = SplitFullName(therapist?.FullName);

        return Ok(new ProfileViewDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = firstName,
            LastName = lastName,
            Roles = roleNames,
            IsActive = user.IsActive,
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        });
    }

    [HttpPost("profile/change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = GetCurrentUserId();
        await _authService.ChangePasswordAsync(userId, dto);
        return Ok(new { message = "Password changed successfully." });
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userId = GetCurrentUserId();
        var userRepo = _unitOfWork.Repository<User>();
        var user = await userRepo.GetByIdAsync(userId)
            ?? throw new ApiException(404, "User not found");

        if (!string.IsNullOrWhiteSpace(dto.Email))
            user.Email = dto.Email;

        user.UpdatedAt = DateTime.UtcNow;
        userRepo.Update(user);

        var therapistRepo = _unitOfWork.Repository<Therapist>();
        var therapist = await therapistRepo.FindSingleAsync(t => t.UserId == userId);
        if (therapist != null)
        {
            var first = dto.FirstName ?? "";
            var last = dto.LastName ?? "";
            var fullName = $"{first} {last}".Trim();
            if (!string.IsNullOrEmpty(fullName))
                therapist.FullName = fullName;
            therapistRepo.Update(therapist);
        }

        await _unitOfWork.SaveChangesAsync();

        return await GetProfile();
    }

    private static (string firstName, string lastName) SplitFullName(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return ("", "");

        var parts = fullName.Trim().Split(' ', 2);
        return (parts[0], parts.Length > 1 ? parts[1] : "");
    }
}
