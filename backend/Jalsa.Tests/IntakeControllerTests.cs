using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.Application.DTOs.Intake;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class IntakeControllerTests
{
    private readonly Mock<IIntakeService> _intakeServiceMock;
    private readonly IntakeController _sut;
    private readonly Guid _therapistUserId;

    public IntakeControllerTests()
    {
        _intakeServiceMock = new Mock<IIntakeService>();

        _sut = new IntakeController(_intakeServiceMock.Object);

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
    public async Task GetByPatientId_ExistingPatient_ReturnsOk()
    {
        var patientId = Guid.NewGuid();
        var dto = new IntakeFormViewDto
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            PresentingProblem = "قلق",
            Status = "Draft",
            CreatedAt = DateTime.UtcNow
        };

        _intakeServiceMock
            .Setup(x => x.GetByPatientIdAsync(patientId, _therapistUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var result = await _sut.GetByPatientId(patientId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task Save_ValidDto_ReturnsOk()
    {
        var patientId = Guid.NewGuid();
        var dto = new IntakeFormSaveDto
        {
            PresentingProblem = "اكتئاب",
            PsychiatricHistory = "لا يوجد",
            Medications = "لا يوجد"
        };
        var saved = new IntakeFormViewDto
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            PresentingProblem = "اكتئاب",
            Status = "Draft",
            CreatedAt = DateTime.UtcNow
        };

        _intakeServiceMock
            .Setup(x => x.SaveAsync(patientId, dto, _therapistUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(saved);

        var result = await _sut.Save(patientId, dto);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(saved);
    }

    [Fact]
    public async Task Submit_ExistingPatient_ReturnsOk()
    {
        var patientId = Guid.NewGuid();
        var submitted = new IntakeFormViewDto
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            Status = "Submitted",
            SubmittedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _intakeServiceMock
            .Setup(x => x.SubmitAsync(patientId, _therapistUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(submitted);

        var result = await _sut.Submit(patientId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ((IntakeFormViewDto)ok.Value!).Status.Should().Be("Submitted");
    }
}
