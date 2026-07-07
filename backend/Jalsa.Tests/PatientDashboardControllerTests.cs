using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.Application.DTOs.PatientDashboard;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Jalsa.Tests;

public class PatientDashboardControllerTests
{
    private readonly Mock<IPatientDashboardService> _patientDashboardServiceMock;
    private readonly PatientDashboardController _sut;
    private readonly Guid _patientUserId;

    public PatientDashboardControllerTests()
    {
        _patientDashboardServiceMock = new Mock<IPatientDashboardService>();
        _sut = new PatientDashboardController(
            _patientDashboardServiceMock.Object,
            NullLogger<PatientDashboardController>.Instance);

        _patientUserId = Guid.NewGuid();
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _patientUserId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };
    }

    [Fact]
    public async Task GetDashboard_ReturnsOk_WithValidService()
    {
        var dto = new PatientDashboardDto { PatientFirstName = "سارة" };
        _patientDashboardServiceMock
            .Setup(x => x.GetDashboardAsync(It.IsAny<Guid>()))
            .ReturnsAsync(dto);

        var result = await _sut.GetDashboard();

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetDashboard_ServiceThrowsUnauthorized_Returns401()
    {
        _patientDashboardServiceMock
            .Setup(x => x.GetDashboardAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new UnauthorizedAccessException("Patient profile not found."));

        var result = await _sut.GetDashboard();

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task GetDashboard_ServiceThrowsGeneric_Returns500()
    {
        _patientDashboardServiceMock
            .Setup(x => x.GetDashboardAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        var result = await _sut.GetDashboard();

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(500);
    }
}
