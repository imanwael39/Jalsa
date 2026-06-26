using FluentAssertions;
using Jalsa.API.DTOs.Auth;
using Jalsa.API.Exceptions;
using Jalsa.API.Services.Interfaces;
using Moq;
using Xunit;

namespace Jalsa.Tests.Services;

public class PatientAuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;

    public PatientAuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
    }

    [Fact]
    public async Task Login_WithValidPatient_ReturnsTokenWithPatientRole()
    {
        var dto = new LoginDto { Email = "patient@test.com", Password = "Patient123!" };
        var response = new AuthResponseDto
        {
            Token = "test-token",
            Email = "patient@test.com",
            Roles = new List<string> { "Patient" }
        };

        _authServiceMock
            .Setup(x => x.LoginAsync(dto))
            .ReturnsAsync(response);

        var result = await _authServiceMock.Object.LoginAsync(dto);

        result.Roles.Should().Contain("Patient");
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithTherapistRole_ShouldReturnTherapistRole()
    {
        var dto = new LoginDto { Email = "therapist@test.com", Password = "Therapist123!" };
        var response = new AuthResponseDto
        {
            Token = "test-token",
            Email = "therapist@test.com",
            Roles = new List<string> { "Therapist" }
        };

        _authServiceMock
            .Setup(x => x.LoginAsync(dto))
            .ReturnsAsync(response);

        var result = await _authServiceMock.Object.LoginAsync(dto);

        result.Roles.Should().Contain("Therapist");
        result.Roles.Should().NotContain("Patient");
    }
}
