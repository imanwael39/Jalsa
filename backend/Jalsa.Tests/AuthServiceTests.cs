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

    // --- Helper ---

    private async Task<AuthResponseDto> RegisterTestUser(string email, string password)
    {
        return await _sut.RegisterAsync(new RegisterDto
        {
            Email = email,
            Password = password
        });
    }
}
