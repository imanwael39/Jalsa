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
using Jalsa.Infrastructure.Data;
using Jalsa.Domain.Models.Identity;

public class AuthService : IAuthService
{
    private readonly Galsa_DBDbContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly IEmailService _emailService;

    public AuthService(Galsa_DBDbContext context, IOptions<JwtSettings> jwt, IEmailService emailService)
    {
        _context = context;
        _jwtSettings = jwt.Value;
        _emailService = emailService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (existingUser != null)
            throw new ApiException(409, "Email already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var roleName = string.IsNullOrWhiteSpace(dto.Role) ? "Therapist" : dto.Role;
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (role == null)
            throw new ApiException(400, $"Role '{roleName}' does not exist.");

        user.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            CreatedAt = DateTime.UtcNow
        });

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        if (roleName == "Therapist")
        {
            var therapist = new Jalsa.Domain.Models.Clinic.Therapist
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FullName = dto.Email.Split('@')[0],
                LicenseNumber = $"LIC-{Guid.NewGuid().ToString()[..8].ToUpper()}",
                CreatedAt = DateTime.UtcNow
            };
            _context.Therapists.Add(therapist);
            await _context.SaveChangesAsync();
        }

        return await BuildAuthResponse(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            throw new ApiException(401, "Invalid email or password!");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new ApiException(401, "Invalid email or password!");

        if (!user.IsActive)
            throw new ApiException(403, "Account is inactive!");

        user.LastLoginAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
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
            throw new ApiException(401, "Invalid refresh token!");

        if (storedToken.RevokedAt != null)
            throw new ApiException(401, "Refresh token has been revoked!");

        if (storedToken.ExpiresAt < DateTime.UtcNow)
            throw new ApiException(401, "Refresh token has expired!");

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
        if (user == null || !user.IsActive)
            return;

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

    public async Task ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
            throw new ApiException(400, "Invalid reset request!");

        var otpHash = ComputeSha256(dto.Otp);
        var resetToken = await _context.PasswordResetTokens
            .Include(pr => pr.User)
            .FirstOrDefaultAsync(pr => pr.UserId == user.Id && pr.TokenHash == otpHash);

        if (resetToken == null)
            throw new ApiException(400, "Invalid OTP!");

        if (resetToken.UsedAt != null)
            throw new ApiException(400, "OTP has already been used!");

        if (resetToken.ExpiresAt < DateTime.UtcNow)
            throw new ApiException(400, "OTP has expired!");

        resetToken.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        resetToken.UsedAt = DateTime.UtcNow;
        resetToken.User.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task RevokeTokenAsync(RevokeTokenRequestDto dto)
    {
        var tokenHash = ComputeSha256(dto.RefreshToken);
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == tokenHash);

        if (storedToken == null)
            throw new ApiException(401, "Invalid refresh token!");

        if (storedToken.RevokedAt != null)
            throw new ApiException(400, "Refresh token is already revoked!");

        storedToken.RevokedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
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
            throw new ApiException(500, "JWT signing key is not configured.");

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