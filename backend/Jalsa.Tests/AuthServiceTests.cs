using FluentAssertions;
using Jalsa.API.Configurations;
using Jalsa.API.DTOs.Auth;
using Jalsa.API.Exceptions;
using Jalsa.API.Services.Interfaces;
using Jalsa.Domain.Models.Identity;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace Jalsa.Tests;

public class AuthServiceTests : IDisposable
{
    private readonly JalsaDbContext _context;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly AuthService _sut;
    private readonly JwtSettings _jwtSettings;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<JalsaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new JalsaDbContext(options);
        _emailServiceMock = new Mock<IEmailService>();

        _jwtSettings = new JwtSettings
        {
            Key = "a-test-key-that-is-at-least-32-bytes-long-for-hmac-sha256",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiryMinutes = 60
        };

        _sut = new AuthService(
            _context,
            Options.Create(_jwtSettings),
            _emailServiceMock.Object);

        SeedRoles();
    }

    private void SeedRoles()
    {
        _context.Roles.AddRange(
            new Role { Id = Guid.NewGuid(), Name = "Therapist" },
            new Role { Id = Guid.NewGuid(), Name = "Patient" },
            new Role { Id = Guid.NewGuid(), Name = "Admin" }
        );
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    // --- Register ---

    [Fact]
    public async Task RegisterAsync_ValidTherapist_ReturnsTokenAndCreatesTherapist()
    {
        var dto = new RegisterDto
        {
            Email = "therapist@test.com",
            Password = "Test123!",
            FullName = "Dr. Test",
            LicenseNumber = "LIC-001"
        };

        var result = await _sut.RegisterAsync(dto);

        result.Token.Should().NotBeNullOrEmpty();
        result.Email.Should().Be(dto.Email);
        result.Roles.Should().Contain("Therapist");
        result.RefreshToken.Should().NotBeNullOrEmpty();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        user.Should().NotBeNull();
        BCrypt.Net.BCrypt.Verify(dto.Password, user!.PasswordHash).Should().BeTrue();

        var therapist = await _context.Therapists.FirstOrDefaultAsync(t => t.UserId == user.Id);
        therapist.Should().NotBeNull();
        therapist!.FullName.Should().Be("Dr. Test");
    }

    [Fact]
    public async Task RegisterAsync_PatientRole_DoesNotCreateTherapist()
    {
        var dto = new RegisterDto
        {
            Email = "patient@test.com",
            Password = "Test123!",
            Role = "Patient"
        };

        var result = await _sut.RegisterAsync(dto);

        result.Roles.Should().Contain("Patient");
        var therapists = await _context.Therapists.ToListAsync();
        therapists.Should().BeEmpty();
    }

    [Fact]
    public async Task RegisterAsync_DuplicateEmail_Throws409()
    {
        var dto = new RegisterDto { Email = "dup@test.com", Password = "Test123!" };
        await _sut.RegisterAsync(dto);

        var act = () => _sut.RegisterAsync(dto);

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(409);

        var users = await _context.Users.Where(u => u.Email == dto.Email).ToListAsync();
        users.Should().HaveCount(1);
    }

    [Fact]
    public async Task RegisterAsync_DuplicateLicenseNumber_Throws409AndDoesNotCreateUser()
    {
        await _sut.RegisterAsync(new RegisterDto
        {
            Email = "first-therapist@test.com",
            Password = "Test123!",
            LicenseNumber = "LIC-SHARED"
        });

        var secondDto = new RegisterDto
        {
            Email = "second-therapist@test.com",
            Password = "Test123!",
            LicenseNumber = "LIC-SHARED"
        };

        var act = () => _sut.RegisterAsync(secondDto);

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(409);

        var secondUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == secondDto.Email);
        secondUser.Should().BeNull("registration must be atomic — no user row should exist when the license check fails");

        var therapists = await _context.Therapists.Where(t => t.LicenseNumber == "LIC-SHARED").ToListAsync();
        therapists.Should().HaveCount(1);
    }

    [Fact]
    public async Task RegisterAsync_InvalidRole_Throws400()
    {
        var dto = new RegisterDto
        {
            Email = "bad@test.com",
            Password = "Test123!",
            Role = "NonExistentRole"
        };

        var act = () => _sut.RegisterAsync(dto);

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(400);
    }

    // --- Login ---

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        await RegisterTestUser("login@test.com", "Correct1!");

        var result = await _sut.LoginAsync(new LoginDto
        {
            Email = "login@test.com",
            Password = "Correct1!"
        });

        result.Token.Should().NotBeNullOrEmpty();
        result.Email.Should().Be("login@test.com");
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_Throws401()
    {
        await RegisterTestUser("wrong@test.com", "Correct1!");

        var act = () => _sut.LoginAsync(new LoginDto
        {
            Email = "wrong@test.com",
            Password = "WrongPassword"
        });

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task LoginAsync_NonExistentUser_Throws401()
    {
        var act = () => _sut.LoginAsync(new LoginDto
        {
            Email = "nobody@test.com",
            Password = "whatever"
        });

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task LoginAsync_InactiveUser_Throws403()
    {
        await RegisterTestUser("inactive@test.com", "Test123!");
        var user = await _context.Users.FirstAsync(u => u.Email == "inactive@test.com");
        user.IsActive = false;
        await _context.SaveChangesAsync();

        var act = () => _sut.LoginAsync(new LoginDto
        {
            Email = "inactive@test.com",
            Password = "Test123!"
        });

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(403);
    }

    // --- Refresh Token ---

    [Fact]
    public async Task RefreshTokenAsync_ValidToken_RotatesAndReturnsNew()
    {
        var authResponse = await RegisterTestUser("refresh@test.com", "Test123!");
        var originalRefresh = authResponse.RefreshToken;

        var result = await _sut.RefreshTokenAsync(new RefreshTokenRequestDto
        {
            RefreshToken = originalRefresh
        });

        result.Token.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBe(originalRefresh);

        var revokedTokens = await _context.RefreshTokens
            .Where(rt => rt.RevokedAt != null)
            .ToListAsync();
        revokedTokens.Should().NotBeEmpty();
    }

    [Fact]
    public async Task RefreshTokenAsync_InvalidToken_Throws401()
    {
        var act = () => _sut.RefreshTokenAsync(new RefreshTokenRequestDto
        {
            RefreshToken = "invalid-token"
        });

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task RefreshTokenAsync_RevokedToken_Throws401()
    {
        var authResponse = await RegisterTestUser("revoked-refresh@test.com", "Test123!");
        await _sut.RevokeTokenAsync(new RevokeTokenRequestDto
        {
            RefreshToken = authResponse.RefreshToken
        });

        var act = () => _sut.RefreshTokenAsync(new RefreshTokenRequestDto
        {
            RefreshToken = authResponse.RefreshToken
        });

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(401);
    }

    // --- Revoke Token ---

    [Fact]
    public async Task RevokeTokenAsync_ValidToken_RevokesSuccessfully()
    {
        var authResponse = await RegisterTestUser("revoke@test.com", "Test123!");

        await _sut.RevokeTokenAsync(new RevokeTokenRequestDto
        {
            RefreshToken = authResponse.RefreshToken
        });

        var allTokens = await _context.RefreshTokens.ToListAsync();
        allTokens.Should().AllSatisfy(t => t.RevokedAt.Should().NotBeNull());
    }

    [Fact]
    public async Task RevokeTokenAsync_AlreadyRevoked_Throws400()
    {
        var authResponse = await RegisterTestUser("double-revoke@test.com", "Test123!");
        await _sut.RevokeTokenAsync(new RevokeTokenRequestDto
        {
            RefreshToken = authResponse.RefreshToken
        });

        var act = () => _sut.RevokeTokenAsync(new RevokeTokenRequestDto
        {
            RefreshToken = authResponse.RefreshToken
        });

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(400);
    }

    // --- JWT Token Validation ---

    [Fact]
    public async Task RegisterAsync_JwtToken_ContainsExpectedClaims()
    {
        var dto = new RegisterDto
        {
            Email = "claims@test.com",
            Password = "Test123!"
        };

        var result = await _sut.RegisterAsync(dto);

        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(result.Token);

        jwt.Issuer.Should().Be(_jwtSettings.Issuer);
        jwt.Audiences.Should().Contain(_jwtSettings.Audience);
        jwt.Claims.Should().Contain(c => c.Type == "email" && c.Value == dto.Email);
    }

    // --- Password Hashing ---

    [Fact]
    public async Task RegisterAsync_PasswordIsHashedWithBCrypt()
    {
        var dto = new RegisterDto
        {
            Email = "hash@test.com",
            Password = "PlainText123!"
        };

        await _sut.RegisterAsync(dto);

        var user = await _context.Users.FirstAsync(u => u.Email == dto.Email);
        user.PasswordHash.Should().NotBe(dto.Password);
        user.PasswordHash.Should().StartWith("$2");
    }

    // --- Account Lockout ---

    [Fact]
    public async Task LoginAsync_WrongPassword_IncrementsFailedAttempts()
    {
        await RegisterTestUser("lockout@test.com", "Test123!");

        var act = () => _sut.LoginAsync(new LoginDto
        {
            Email = "lockout@test.com",
            Password = "WrongPassword"
        });

        await act.Should().ThrowAsync<ApiException>();

        var user = await _context.Users.FirstAsync(u => u.Email == "lockout@test.com");
        user.FailedLoginAttempts.Should().Be(1);
        user.LockoutEnd.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_FiveFailedAttempts_LocksAccount()
    {
        await RegisterTestUser("fivefail@test.com", "Test123!");

        for (int i = 0; i < 4; i++)
        {
            var act = () => _sut.LoginAsync(new LoginDto
            {
                Email = "fivefail@test.com",
                Password = "WrongPassword"
            });
            await act.Should().ThrowAsync<ApiException>();
        }

        var user = await _context.Users.FirstAsync(u => u.Email == "fivefail@test.com");
        user.FailedLoginAttempts.Should().Be(4);
        user.LockoutEnd.Should().BeNull();

        var act5 = () => _sut.LoginAsync(new LoginDto
        {
            Email = "fivefail@test.com",
            Password = "WrongPassword"
        });
        await act5.Should().ThrowAsync<ApiException>();

        user = await _context.Users.FirstAsync(u => u.Email == "fivefail@test.com");
        user.FailedLoginAttempts.Should().Be(5);
        user.LockoutEnd.Should().NotBeNull();
        user.LockoutEnd!.Value.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task LoginAsync_LockedAccount_Throws403()
    {
        await RegisterTestUser("locked@test.com", "Test123!");
        var user = await _context.Users.FirstAsync(u => u.Email == "locked@test.com");
        user.FailedLoginAttempts = 5;
        user.LockoutEnd = DateTime.UtcNow.AddMinutes(10);
        await _context.SaveChangesAsync();

        var act = () => _sut.LoginAsync(new LoginDto
        {
            Email = "locked@test.com",
            Password = "Test123!"
        });

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(403);
    }

    [Fact]
    public async Task LoginAsync_SuccessfulLogin_ResetsFailedAttempts()
    {
        await RegisterTestUser("reset@test.com", "Test123!");
        var user = await _context.Users.FirstAsync(u => u.Email == "reset@test.com");
        user.FailedLoginAttempts = 3;
        await _context.SaveChangesAsync();

        await _sut.LoginAsync(new LoginDto
        {
            Email = "reset@test.com",
            Password = "Test123!"
        });

        user = await _context.Users.FirstAsync(u => u.Email == "reset@test.com");
        user.FailedLoginAttempts.Should().Be(0);
        user.LockoutEnd.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_ExpiredLockout_AllowsLogin()
    {
        await RegisterTestUser("expired@test.com", "Test123!");
        var user = await _context.Users.FirstAsync(u => u.Email == "expired@test.com");
        user.FailedLoginAttempts = 5;
        user.LockoutEnd = DateTime.UtcNow.AddMinutes(-1);
        await _context.SaveChangesAsync();

        var result = await _sut.LoginAsync(new LoginDto
        {
            Email = "expired@test.com",
            Password = "Test123!"
        });

        result.Token.Should().NotBeNullOrEmpty();
    }

    // --- Forgot Password / OTP ---

    [Fact]
    public async Task ForgotPasswordAsync_NonExistentEmail_Throws404AndDoesNotSendEmail()
    {
        var act = () => _sut.ForgotPasswordAsync(new ForgotPasswordDto { Email = "nobody@test.com" });

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(404);

        _emailServiceMock.Verify(x => x.SendOtpAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ForgotPasswordAsync_ExistingEmail_SendsOtpAndSavesToken()
    {
        await RegisterTestUser("forgot@test.com", "Test123!");

        await _sut.ForgotPasswordAsync(new ForgotPasswordDto { Email = "forgot@test.com" });

        _emailServiceMock.Verify(x => x.SendOtpAsync("forgot@test.com", It.IsAny<string>()), Times.Once);
        var tokens = await _context.PasswordResetTokens.ToListAsync();
        tokens.Should().ContainSingle();
    }

    [Fact]
    public async Task VerifyOtpAsync_ValidOtp_DoesNotThrowAndDoesNotConsumeToken()
    {
        var otp = await RegisterAndForgotPassword("verify@test.com");

        var act = () => _sut.VerifyOtpAsync(new VerifyOtpDto { Email = "verify@test.com", Otp = otp });

        await act.Should().NotThrowAsync();

        var token = await _context.PasswordResetTokens.FirstAsync();
        token.UsedAt.Should().BeNull();
    }

    [Fact]
    public async Task VerifyOtpAsync_InvalidOtp_Throws400()
    {
        await RegisterAndForgotPassword("verify-invalid@test.com");

        var act = () => _sut.VerifyOtpAsync(new VerifyOtpDto { Email = "verify-invalid@test.com", Otp = "000000" });

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task VerifyOtpAsync_ExpiredOtp_Throws400()
    {
        var otp = await RegisterAndForgotPassword("verify-expired@test.com");
        var token = await _context.PasswordResetTokens.FirstAsync();
        token.ExpiresAt = DateTime.UtcNow.AddMinutes(-1);
        await _context.SaveChangesAsync();

        var act = () => _sut.VerifyOtpAsync(new VerifyOtpDto { Email = "verify-expired@test.com", Otp = otp });

        var ex = await act.Should().ThrowAsync<ApiException>();
        ex.Which.StatusCode.Should().Be(400);
    }

    // --- Helper ---

    private async Task<string> RegisterAndForgotPassword(string email)
    {
        await RegisterTestUser(email, "Test123!");

        string? capturedOtp = null;
        _emailServiceMock
            .Setup(x => x.SendOtpAsync(email, It.IsAny<string>()))
            .Callback<string, string>((_, otp) => capturedOtp = otp)
            .Returns(Task.CompletedTask);

        await _sut.ForgotPasswordAsync(new ForgotPasswordDto { Email = email });

        return capturedOtp!;
    }

    private async Task<AuthResponseDto> RegisterTestUser(string email, string password)
    {
        return await _sut.RegisterAsync(new RegisterDto
        {
            Email = email,
            Password = password
        });
    }
}
