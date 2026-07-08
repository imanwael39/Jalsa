using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.Application.DTOs.PatientSession;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class PatientSessionControllerTests
{
    private readonly Mock<IPatientSessionService> _patientSessionServiceMock;
    private readonly PatientSessionController _sut;
    private readonly Guid _patientUserId;

    public PatientSessionControllerTests()
    {
        _patientSessionServiceMock = new Mock<IPatientSessionService>();
        _sut = new PatientSessionController(_patientSessionServiceMock.Object);

        _patientUserId = Guid.NewGuid();
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _patientUserId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };
    }

    private static PatientSessionSummaryDto MakeSummaryDto(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        SessionNumber = 1,
        SessionDate = DateOnly.FromDateTime(DateTime.UtcNow),
        DurationMinutes = 50,
        SessionType = "Individual",
        Status = "Scheduled"
    };

    private static PatientSessionDetailDto MakeDetailDto(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        SessionNumber = 1,
        SessionDate = DateOnly.FromDateTime(DateTime.UtcNow),
        DurationMinutes = 50,
        SessionType = "Individual",
        Status = "Scheduled",
        TherapistName = "د. أحمد سالم",
        CanRequestChange = true
    };

    [Fact]
    public async Task GetList_ReturnsOkWithSessions()
    {
        var sessions = new List<PatientSessionSummaryDto> { MakeSummaryDto(), MakeSummaryDto() };
        _patientSessionServiceMock
            .Setup(x => x.GetListAsync(_patientUserId))
            .ReturnsAsync(sessions);

        var result = await _sut.GetList();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = ok.Value.Should().BeAssignableTo<List<PatientSessionSummaryDto>>().Subject;
        value.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetDetail_ExistingId_ReturnsOk()
    {
        var sessionId = Guid.NewGuid();
        var dto = MakeDetailDto(sessionId);
        _patientSessionServiceMock
            .Setup(x => x.GetDetailAsync(_patientUserId, sessionId))
            .ReturnsAsync(dto);

        var result = await _sut.GetDetail(sessionId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task RequestReschedule_ValidDto_Returns204NoContent()
    {
        var sessionId = Guid.NewGuid();
        var dto = new SessionChangeRequestDto { Note = "أحتاج موعداً آخر" };
        _patientSessionServiceMock
            .Setup(x => x.RequestRescheduleAsync(_patientUserId, sessionId, dto))
            .Returns(Task.CompletedTask);

        var result = await _sut.RequestReschedule(sessionId, dto);

        result.Should().BeOfType<NoContentResult>();
        _patientSessionServiceMock.Verify(x => x.RequestRescheduleAsync(_patientUserId, sessionId, dto), Times.Once);
    }

    [Fact]
    public async Task RequestCancel_ValidDto_Returns204NoContent()
    {
        var sessionId = Guid.NewGuid();
        var dto = new SessionChangeRequestDto { Note = "لن أتمكن من الحضور" };
        _patientSessionServiceMock
            .Setup(x => x.RequestCancelAsync(_patientUserId, sessionId, dto))
            .Returns(Task.CompletedTask);

        var result = await _sut.RequestCancel(sessionId, dto);

        result.Should().BeOfType<NoContentResult>();
        _patientSessionServiceMock.Verify(x => x.RequestCancelAsync(_patientUserId, sessionId, dto), Times.Once);
    }
}
