using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Jalsa.API.Configurations;
using Jalsa.API.DTOs.Auth;
using Jalsa.API.Exceptions;
using Jalsa.API.Services.Interfaces;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Infrastructure.Data;
using Jalsa.Domain.Models.Identity;
using Microsoft.AspNetCore.Http;

public class AuthService : IAuthService
{
    private readonly JalsaDbContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly IEmailService _emailService;
    private readonly IAuditLogService _auditLogService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        JalsaDbContext context,
        IOptions<JwtSettings> jwt,
        IEmailService emailService,
        IAuditLogService auditLogService,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _jwtSettings = jwt.Value;
        _emailService = emailService;
        _auditLogService = auditLogService;
        _httpContextAccessor = httpContextAccessor;
    }

    private (string? Ip, string? UserAgent) GetRequestMetadata()
    {
        var http = _httpContextAccessor.HttpContext;
        var ip = http?.Connection.RemoteIpAddress?.ToString();
        var ua = http?.Request.Headers.UserAgent.ToString() is { Length: > 0 } value ? value : null;
        return (ip, ua);
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        // Validate everything up front so no row is written until all checks pass.
        var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (emailExists)
            throw new ApiException(409, "البريد الإلكتروني مستخدم بالفعل.");

        var roleName = string.IsNullOrWhiteSpace(dto.Role) ? "Therapist" : dto.Role;
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (role == null)
            throw new ApiException(400, $"نوع الحساب '{roleName}' غير موجود.");

        string? licenseNumber = null;
        if (roleName == "Therapist")
        {
            licenseNumber = string.IsNullOrWhiteSpace(dto.LicenseNumber)
                ? $"LIC-{Guid.NewGuid().ToString()[..8].ToUpper()}"
                : dto.LicenseNumber;

            var licenseExists = await _context.Therapists.AnyAsync(t => t.LicenseNumber == licenseNumber);
            if (licenseExists)
                throw new ApiException(409, "رقم الترخيص مستخدم بالفعل.");
        }

        // Self-registered patients still need a Patient record so they show up on a
        // therapist's dashboard exactly like a therapist-created patient does. The
        // patient picks which therapist they're signing up under at registration time.
        if (roleName == "Patient")
        {
            if (dto.TherapistId == null)
                throw new ApiException(400, "يجب اختيار المعالج عند التسجيل كمريض.");

            var chosenTherapistExists = await _context.Therapists.AnyAsync(t => t.Id == dto.TherapistId);
            if (!chosenTherapistExists)
                throw new ApiException(400, "المعالج المختار غير موجود.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        user.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            CreatedAt = DateTime.UtcNow
        });

        _context.Users.Add(user);

        if (roleName == "Therapist")
        {
            var therapist = new Jalsa.Domain.Models.Clinic.Therapist
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FullName = string.IsNullOrWhiteSpace(dto.FullName) ? dto.Email.Split('@')[0] : dto.FullName,
                LicenseNumber = licenseNumber!,
                Specialization = dto.Specialization,
                ApprovalStatus = Jalsa.Domain.Models.Clinic.TherapistApprovalStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
            _context.Therapists.Add(therapist);
        }
        else if (roleName == "Patient")
        {
            var patient = new Jalsa.Domain.Models.Patient.Patient
            {
                Id = Guid.NewGuid(),
                TherapistId = dto.TherapistId!.Value,
                UserId = user.Id,
                FullName = string.IsNullOrWhiteSpace(dto.FullName) ? dto.Email.Split('@')[0] : dto.FullName,
                Email = dto.Email,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.Patients.Add(patient);
        }

        // Single SaveChangesAsync => one transaction, so User/UserRole/Therapist/Patient commit or fail together.
        await _context.SaveChangesAsync();

        return await BuildAuthResponse(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        const int maxFailedAttempts = 5;
        const int lockoutMinutes = 15;

        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(u => u.Therapist)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        var (ip, userAgent) = GetRequestMetadata();

        if (user == null || user.IsDeleted)
        {
            await _auditLogService.LogAsync(null, "User", null, "User.LoginFailed", newValues: new { dto.Email, Reason = "NotFound" }, ipAddress: ip, userAgent: userAgent);
            throw new ApiException(401, "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
        }

        if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
        {
            await _auditLogService.LogAsync(user.Id, "User", user.Id.ToString(), "User.LoginFailed", newValues: new { Reason = "LockedOut" }, ipAddress: ip, userAgent: userAgent);
            throw new ApiException(403, "الحساب مقفل مؤقتًا، يرجى المحاولة لاحقًا.");
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            user.FailedLoginAttempts++;
            if (user.FailedLoginAttempts >= maxFailedAttempts)
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(lockoutMinutes);

            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            await _auditLogService.LogAsync(user.Id, "User", user.Id.ToString(), "User.LoginFailed", newValues: new { Reason = "WrongPassword" }, ipAddress: ip, userAgent: userAgent);
            throw new ApiException(401, "البريد الإلكتروني أو كلمة المرور غير صحيحة.");
        }

        if (!user.IsActive)
        {
            await _auditLogService.LogAsync(user.Id, "User", user.Id.ToString(), "User.LoginFailed", newValues: new { Reason = "Inactive" }, ipAddress: ip, userAgent: userAgent);
            throw new ApiException(403, "هذا الحساب غير مُفعّل.");
        }

        if (user.Therapist != null &&
            (user.Therapist.ApprovalStatus == Jalsa.Domain.Models.Clinic.TherapistApprovalStatus.Suspended ||
             user.Therapist.ApprovalStatus == Jalsa.Domain.Models.Clinic.TherapistApprovalStatus.Rejected))
        {
            await _auditLogService.LogAsync(user.Id, "User", user.Id.ToString(), "User.LoginFailed", newValues: new { Reason = "TherapistSuspended" }, ipAddress: ip, userAgent: userAgent);
            throw new ApiException(403, "تم تعليق حساب المعالج من قبل الإدارة.");
        }

        user.FailedLoginAttempts = 0;
        user.LockoutEnd = null;
        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        await _auditLogService.LogAsync(user.Id, "User", user.Id.ToString(), "User.LoginSucceeded", ipAddress: ip, userAgent: userAgent);
        return await BuildAuthResponse(user);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
    {
        var tokenHash = ComputeSha256(dto.RefreshToken);
        var storedToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(rt => rt.Token == tokenHash);

        if (storedToken == null)
            throw new ApiException(401, "رمز التحديث غير صالح.");

        if (storedToken.RevokedAt != null)
            throw new ApiException(401, "تم إلغاء رمز التحديث.");

        if (storedToken.ExpiresAt < DateTime.UtcNow)
            throw new ApiException(401, "انتهت صلاحية رمز التحديث.");

        var rawToken = GenerateRawToken();
        var newTokenHash = ComputeSha256(rawToken);

        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = storedToken.UserId,
            Token = newTokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        storedToken.RevokedAt = DateTime.UtcNow;
        storedToken.ReplacedByToken = newRefreshToken;

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();

        var response = await BuildAuthResponse(storedToken.User);
        response.RefreshToken = rawToken;
        response.RefreshTokenExpiresAt = newRefreshToken.ExpiresAt;
        return response;
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
            throw new ApiException(404, "هذا البريد الإلكتروني غير مسجل لدينا.");

        if (!user.IsActive)
            throw new ApiException(403, "هذا الحساب غير مفعل.");

        var existingTokens = await _context.PasswordResetTokens
            .Where(t => t.UserId == user.Id && t.UsedAt == null && t.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();

        foreach (var t in existingTokens)
            t.UsedAt = DateTime.UtcNow;

        var otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        var otpHash = ComputeSha256(otp);

        _context.PasswordResetTokens.Add(new PasswordResetToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = otpHash,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5),
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();

        await _emailService.SendOtpAsync(user.Email, otp);
    }

    public async Task VerifyOtpAsync(VerifyOtpDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
            throw new ApiException(404, "هذا البريد الإلكتروني غير مسجل لدينا.");

        await GetValidResetTokenAsync(user, dto.Otp);
    }

    public async Task ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
            throw new ApiException(404, "هذا البريد الإلكتروني غير مسجل لدينا.");

        var resetToken = await GetValidResetTokenAsync(user, dto.Otp);

        resetToken.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        resetToken.UsedAt = DateTime.UtcNow;
        resetToken.User.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    private async Task<PasswordResetToken> GetValidResetTokenAsync(User user, string otp)
    {
        var otpHash = ComputeSha256(otp);
        var resetToken = await _context.PasswordResetTokens
            .Include(pr => pr.User)
            .FirstOrDefaultAsync(pr => pr.UserId == user.Id && pr.TokenHash == otpHash);

        if (resetToken == null)
            throw new ApiException(400, "رمز التحقق غير صحيح.");

        if (resetToken.UsedAt != null)
            throw new ApiException(400, "رمز التحقق مستخدم بالفعل.");

        if (resetToken.ExpiresAt < DateTime.UtcNow)
            throw new ApiException(400, "انتهت صلاحية رمز التحقق.");

        return resetToken;
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new ApiException(404, "المستخدم غير موجود.");

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            throw new ApiException(400, "كلمة المرور الحالية غير صحيحة.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task RevokeTokenAsync(RevokeTokenRequestDto dto)
    {
        var tokenHash = ComputeSha256(dto.RefreshToken);
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == tokenHash);

        if (storedToken == null)
            throw new ApiException(401, "رمز التحديث غير صالح.");

        if (storedToken.RevokedAt != null)
            throw new ApiException(400, "تم إلغاء رمز التحديث بالفعل.");

        storedToken.RevokedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var (ip, userAgent) = GetRequestMetadata();
        await _auditLogService.LogAsync(storedToken.UserId, "User", storedToken.UserId.ToString(), "User.Logout", ipAddress: ip, userAgent: userAgent);
    }

    private async Task<AuthResponseDto> BuildAuthResponse(User user)
    {
        var roles = await _context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Select(ur => ur.Role.Name)
            .ToListAsync();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        if (string.IsNullOrWhiteSpace(_jwtSettings.Key))
            throw new ApiException(500, "حدث خطأ في إعدادات الخادم.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        var rawRefresh = GenerateRawToken();
        var refreshHash = ComputeSha256(rawRefresh);
        var refreshExpires = DateTime.UtcNow.AddDays(7);

        _context.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshHash,
            ExpiresAt = refreshExpires,
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expires,
            RefreshToken = rawRefresh,
            RefreshTokenExpiresAt = refreshExpires,
            Email = user.Email,
            Roles = roles
        };
    }

    private static string GenerateRawToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private static string ComputeSha256(string raw)
    {
        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(raw)));
    }
}