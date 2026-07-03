using System.Security.Claims;
using FluentAssertions;
using Jalsa.Application.DTOs.Dashboard;
using Jalsa.Application.Interfaces.Services;
using Jalsa.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class ProgressControllerTests
{
    private readonly Mock<IProgressService> _progressServiceMock;
    private readonly ProgressController _sut;
    private readonly Guid _therapistUserId;

    public ProgressControllerTests()
    {
        _progressServiceMock = new Mock<IProgressService>();
        _sut = new ProgressController(_progressServiceMock.Object);

        _therapistUserId = Guid.NewGuid();
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, _therapistUserId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [Fact]
    public async Task GetDashboard_ReturnsOk_WithValidService()
    {
        // Arrange
        var dto = new DashboardSummaryDto
        {
            TotalPatients = 10,
            ActivePatients = 8,
            ArchivedPatients = 2,
            TotalSessions = 25,
            SessionsThisMonth = 5,
            ExerciseCompletionRate = 75.5,
            AverageAssessmentScore = 68.3,
            RecentAlerts = new List<string>(),
            Analytics = new AnalyticsDto()
        };

        _progressServiceMock
            .Setup(x => x.GetDashboardAsync(It.IsAny<Guid>()))
            .ReturnsAsync(dto);

        // Act
        var result = await _sut.GetDashboard();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetDashboard_ServiceThrows_Returns500()
    {
        // Arrange
        _progressServiceMock
            .Setup(x => x.GetDashboardAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await _sut.GetDashboard();

        // Assert
        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(500);
    }
}
