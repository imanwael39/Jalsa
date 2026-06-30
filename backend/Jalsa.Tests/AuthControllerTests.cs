using System.Linq.Expressions;
using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.API.DTOs.Auth;
using Jalsa.API.Services.Interfaces;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AuthController _sut;
    private readonly Guid _userId = Guid.NewGuid();

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _sut = new AuthController(_authServiceMock.Object, _unitOfWorkMock.Object);

        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _userId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };
    }

    [Fact]
    public async Task Register_ValidDto_ReturnsOk()
    {
        var dto = new RegisterDto { Email = "test@test.com", Password = "Pass123!", FullName = "Test User", Role = "Therapist" };
        var response = new AuthResponseDto { Token = "jwt", Email = dto.Email, Roles = ["Therapist"] };
        _authServiceMock.Setup(x => x.RegisterAsync(dto)).ReturnsAsync(response);

        var result = await _sut.Register(dto);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        var dto = new LoginDto { Email = "test@test.com", Password = "Pass123!" };
        var response = new AuthResponseDto { Token = "jwt", Email = dto.Email };
        _authServiceMock.Setup(x => x.LoginAsync(dto)).ReturnsAsync(response);

        var result = await _sut.Login(dto);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Refresh_ValidToken_ReturnsOk()
    {
        var dto = new RefreshTokenRequestDto { RefreshToken = "token" };
        var response = new AuthResponseDto { Token = "newjwt", RefreshToken = "newrefresh" };
        _authServiceMock.Setup(x => x.RefreshTokenAsync(dto)).ReturnsAsync(response);

        var result = await _sut.Refresh(dto);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Revoke_ValidToken_ReturnsNoContent()
    {
        var dto = new RevokeTokenRequestDto { RefreshToken = "token" };
        _authServiceMock.Setup(x => x.RevokeTokenAsync(dto)).Returns(Task.CompletedTask);

        var result = await _sut.Revoke(dto);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task ForgotPassword_AnyEmail_ReturnsOkWithMessage()
    {
        var dto = new ForgotPasswordDto { Email = "test@test.com" };
        _authServiceMock.Setup(x => x.ForgotPasswordAsync(dto)).Returns(Task.CompletedTask);

        var result = await _sut.ForgotPassword(dto);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ResetPassword_ValidDto_ReturnsOkWithMessage()
    {
        var dto = new ResetPasswordDto { Email = "test@test.com", Otp = "123456", NewPassword = "NewPass1!" };
        _authServiceMock.Setup(x => x.ResetPasswordAsync(dto)).Returns(Task.CompletedTask);

        var result = await _sut.ResetPassword(dto);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetProfile_AuthenticatedUser_ReturnsOk()
    {
        var user = new User { Id = _userId, Email = "test@test.com", PasswordHash = "hash", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var therapist = new Therapist { Id = Guid.NewGuid(), UserId = _userId, FullName = "Ahmed Ali", LicenseNumber = "LIC001" };
        var roleId = Guid.NewGuid();
        var userRoles = new List<UserRole> { new() { UserId = _userId, RoleId = roleId } };
        var roles = new List<Role> { new() { Id = roleId, Name = "Therapist" } };

        var userRepoMock = new Mock<IGenericRepository<User>>();
        userRepoMock.Setup(r => r.GetByIdAsync(_userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var therapistRepoMock = new Mock<IGenericRepository<Therapist>>();
        therapistRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(therapist);

        var userRoleRepoMock = new Mock<IGenericRepository<UserRole>>();
        userRoleRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<UserRole, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(userRoles);

        var roleRepoMock = new Mock<IGenericRepository<Role>>();
        roleRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(roles);

        _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Therapist>()).Returns(therapistRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<UserRole>()).Returns(userRoleRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Role>()).Returns(roleRepoMock.Object);

        var result = await _sut.GetProfile();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var profile = ok.Value.Should().BeOfType<ProfileViewDto>().Subject;
        profile.Email.Should().Be("test@test.com");
        profile.FirstName.Should().Be("Ahmed");
        profile.LastName.Should().Be("Ali");
        profile.Roles.Should().Contain("Therapist");
    }

    [Fact]
    public async Task UpdateProfile_ValidDto_ReturnsUpdatedProfile()
    {
        var user = new User { Id = _userId, Email = "old@test.com", PasswordHash = "hash", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
        var therapist = new Therapist { Id = Guid.NewGuid(), UserId = _userId, FullName = "Old Name", LicenseNumber = "LIC001" };
        var roleId = Guid.NewGuid();

        var userRepoMock = new Mock<IGenericRepository<User>>();
        userRepoMock.Setup(r => r.GetByIdAsync(_userId, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var therapistRepoMock = new Mock<IGenericRepository<Therapist>>();
        therapistRepoMock.Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(therapist);

        var userRoleRepoMock = new Mock<IGenericRepository<UserRole>>();
        userRoleRepoMock.Setup(r => r.FindAsync(It.IsAny<Expression<Func<UserRole, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<UserRole> { new() { UserId = _userId, RoleId = roleId } });

        var roleRepoMock = new Mock<IGenericRepository<Role>>();
        roleRepoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Role> { new() { Id = roleId, Name = "Therapist" } });

        _unitOfWorkMock.Setup(u => u.Repository<User>()).Returns(userRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Therapist>()).Returns(therapistRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<UserRole>()).Returns(userRoleRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Role>()).Returns(roleRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var dto = new UpdateProfileDto { FirstName = "New", LastName = "Name", Email = "new@test.com" };

        var result = await _sut.UpdateProfile(dto);

        result.Should().BeOfType<OkObjectResult>();
        user.Email.Should().Be("new@test.com");
        therapist.FullName.Should().Be("New Name");
    }
}
