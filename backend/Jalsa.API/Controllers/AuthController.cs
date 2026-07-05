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
    private readonly IWebHostEnvironment _environment;

    public AuthController(IAuthService authService, IUnitOfWork unitOfWork, IWebHostEnvironment environment)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
        _environment = environment;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return Ok(result);
    }

    [HttpGet("therapists")]
    public async Task<IActionResult> GetTherapists()
    {
        var therapists = await _unitOfWork.Repository<Therapist>().GetAllAsync();
        var result = therapists
            .OrderBy(t => t.FullName)
            .Select(t => new TherapistOptionDto
            {
                Id = t.Id,
                FullName = t.FullName,
                Specialization = t.Specialization,
            });
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
        return Ok(new { message = "تم إرسال رمز التحقق إلى بريدك الإلكتروني." });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        await _authService.ResetPasswordAsync(dto);
        return Ok(new { message = "تم إعادة تعيين كلمة المرور بنجاح." });
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

        var patientRepo = _unitOfWork.Repository<Jalsa.Domain.Models.Patient.Patient>();
        var patient = therapist == null
            ? await patientRepo.FindSingleAsync(p => p.UserId == userId)
            : null;

        var roleRepo = _unitOfWork.Repository<UserRole>();
        var userRoles = await roleRepo.FindAsync(ur => ur.UserId == userId);
        var roleIdRepo = _unitOfWork.Repository<Role>();
        var allRoles = await roleIdRepo.GetAllAsync();
        var roleNames = userRoles
            .Join(allRoles, ur => ur.RoleId, r => r.Id, (_, r) => r.Name)
            .ToList();

        var (firstName, lastName) = SplitFullName(therapist?.FullName ?? patient?.FullName);

        return Ok(new ProfileViewDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = firstName,
            LastName = lastName,
            ProfileImageUrl = therapist?.ProfileImageUrl ?? patient?.ProfileImageUrl,
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

        var first = dto.FirstName ?? "";
        var last = dto.LastName ?? "";
        var fullName = $"{first} {last}".Trim();

        var therapistRepo = _unitOfWork.Repository<Therapist>();
        var therapist = await therapistRepo.FindSingleAsync(t => t.UserId == userId);
        if (therapist != null)
        {
            if (!string.IsNullOrEmpty(fullName))
                therapist.FullName = fullName;
            therapistRepo.Update(therapist);
        }
        else
        {
            var patientRepo = _unitOfWork.Repository<Jalsa.Domain.Models.Patient.Patient>();
            var patient = await patientRepo.FindSingleAsync(p => p.UserId == userId);
            if (patient != null)
            {
                if (!string.IsNullOrEmpty(fullName))
                    patient.FullName = fullName;
                if (!string.IsNullOrWhiteSpace(dto.Email))
                    patient.Email = dto.Email;
                patient.UpdatedAt = DateTime.UtcNow;
                patientRepo.Update(patient);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        return await GetProfile();
    }

    private static readonly HashSet<string> AllowedAvatarContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg", "image/png", "image/webp"
    };

    private const long MaxAvatarFileSizeBytes = 5 * 1024 * 1024; // 5 MB

    [HttpPost("profile/photo")]
    [Authorize]
    public async Task<IActionResult> UploadProfilePhoto(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ApiException(400, "لم يتم اختيار أي صورة.");

        if (file.Length > MaxAvatarFileSizeBytes)
            throw new ApiException(400, "حجم الصورة يجب ألا يتجاوز 5 ميجابايت.");

        if (string.IsNullOrWhiteSpace(file.ContentType) || !AllowedAvatarContentTypes.Contains(file.ContentType))
            throw new ApiException(400, "صيغة الصورة غير مدعومة. الصيغ المسموحة: JPG, PNG, WEBP.");

        var userId = GetCurrentUserId();

        var extension = file.ContentType.ToLowerInvariant() switch
        {
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => ".jpg",
        };

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{userId}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativeUrl = $"/uploads/avatars/{fileName}";

        var therapistRepo = _unitOfWork.Repository<Therapist>();
        var therapist = await therapistRepo.FindSingleAsync(t => t.UserId == userId);
        if (therapist != null)
        {
            therapist.ProfileImageUrl = relativeUrl;
            therapistRepo.Update(therapist);
        }
        else
        {
            var patientRepo = _unitOfWork.Repository<Jalsa.Domain.Models.Patient.Patient>();
            var patient = await patientRepo.FindSingleAsync(p => p.UserId == userId)
                ?? throw new ApiException(404, "لم يتم العثور على الملف الشخصي.");
            patient.ProfileImageUrl = relativeUrl;
            patient.UpdatedAt = DateTime.UtcNow;
            patientRepo.Update(patient);
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
