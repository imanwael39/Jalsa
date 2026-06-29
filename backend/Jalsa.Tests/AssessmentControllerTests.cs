using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.Application.DTOs.Assessment;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class AssessmentControllerTests
{
    private readonly Mock<IAssessmentService> _assessmentServiceMock;
    private readonly AssessmentController _sut;
    private readonly Guid _therapistUserId;

    public AssessmentControllerTests()
    {
        _assessmentServiceMock = new Mock<IAssessmentService>();

        _sut = new AssessmentController(_assessmentServiceMock.Object);

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
    public async Task GetByPatientId_ExistingPatient_ReturnsOkWithList()
    {
        var patientId = Guid.NewGuid();
        var assessments = new List<AssessmentViewDto>
        {
            new AssessmentViewDto
            {
                Id = Guid.NewGuid(),
                PatientId = patientId,
                TemplateId = Guid.NewGuid(),
                Title = "PHQ-9",
                TotalScore = 12,
                Status = "Active",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        _assessmentServiceMock
            .Setup(x => x.GetByPatientIdAsync(patientId, _therapistUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assessments);

        var result = await _sut.GetByPatientId(patientId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = ok.Value.Should().BeAssignableTo<IEnumerable<AssessmentViewDto>>().Subject;
        value.Should().HaveCount(1);
    }

    [Fact]
    public async Task Create_ValidDto_Returns201Created()
    {
        var patientId = Guid.NewGuid();
        var dto = new AssessmentCreateDto
        {
            TemplateId = "PHQ-9",
            Title = "PHQ-9",
            TotalScore = 15,
            AssessmentDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        var created = new AssessmentViewDto
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            TemplateId = Guid.NewGuid(),
            Title = "PHQ-9",
            TotalScore = 15,
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _assessmentServiceMock
            .Setup(x => x.CreateAsync(patientId, dto, _therapistUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(created);

        var result = await _sut.Create(patientId, dto);

        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(created);
    }
}
