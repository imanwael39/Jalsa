using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.Application.DTOs.PatientProgress;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Jalsa.Tests;

public class PatientProgressControllerTests
{
    private readonly Mock<IPatientProgressService> _patientProgressServiceMock;
    private readonly PatientProgressController _sut;
    private readonly Guid _patientUserId;

    public PatientProgressControllerTests()
    {
        _patientProgressServiceMock = new Mock<IPatientProgressService>();
        _sut = new PatientProgressController(
            _patientProgressServiceMock.Object,
            NullLogger<PatientProgressController>.Instance);

        _patientUserId = Guid.NewGuid();
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _patientUserId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };
    }

    [Fact]
    public async Task GetProgress_ReturnsOk_WithValidService()
    {
        var dto = new PatientProgressDto();
        _patientProgressServiceMock
            .Setup(x => x.GetProgressAsync(It.IsAny<Guid>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>()))
            .ReturnsAsync(dto);

        var result = await _sut.GetProgress(null, null);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetProgress_PassesDateRangeThroughToService()
    {
        var from = new DateOnly(2026, 1, 1);
        var to = new DateOnly(2026, 6, 30);
        _patientProgressServiceMock
            .Setup(x => x.GetProgressAsync(_patientUserId, from, to))
            .ReturnsAsync(new PatientProgressDto());

        var result = await _sut.GetProgress(from, to);

        result.Should().BeOfType<OkObjectResult>();
        _patientProgressServiceMock.Verify(x => x.GetProgressAsync(_patientUserId, from, to), Times.Once);
    }

    [Fact]
    public async Task GetProgress_ServiceThrowsUnauthorized_Returns401()
    {
        _patientProgressServiceMock
            .Setup(x => x.GetProgressAsync(It.IsAny<Guid>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>()))
            .ThrowsAsync(new UnauthorizedAccessException("Patient profile not found."));

        var result = await _sut.GetProgress(null, null);

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task GetProgress_ServiceThrowsGeneric_Returns500()
    {
        _patientProgressServiceMock
            .Setup(x => x.GetProgressAsync(It.IsAny<Guid>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        var result = await _sut.GetProgress(null, null);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(500);
    }
}
