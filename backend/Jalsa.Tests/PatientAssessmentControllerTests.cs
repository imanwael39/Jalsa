using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.Application.DTOs.PatientAssessment;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class PatientAssessmentControllerTests
{
    private readonly Mock<IPatientAssessmentService> _patientAssessmentServiceMock;
    private readonly PatientAssessmentController _sut;
    private readonly Guid _patientUserId;

    public PatientAssessmentControllerTests()
    {
        _patientAssessmentServiceMock = new Mock<IPatientAssessmentService>();
        _sut = new PatientAssessmentController(_patientAssessmentServiceMock.Object);

        _patientUserId = Guid.NewGuid();
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _patientUserId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };
    }

    [Fact]
    public async Task GetList_ReturnsOk_WithList()
    {
        var list = new List<PatientAssessmentSummaryDto> { new() { Id = Guid.NewGuid(), TemplateName = "phq-9" } };
        _patientAssessmentServiceMock.Setup(x => x.GetListAsync(_patientUserId)).ReturnsAsync(list);

        var result = await _sut.GetList();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(list);
    }

    [Fact]
    public async Task GetDetail_ReturnsOk_WithDetail()
    {
        var id = Guid.NewGuid();
        var detail = new PatientAssessmentDetailDto { Id = id, TemplateName = "phq-9" };
        _patientAssessmentServiceMock.Setup(x => x.GetDetailAsync(_patientUserId, id)).ReturnsAsync(detail);

        var result = await _sut.GetDetail(id);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(detail);
    }

    [Fact]
    public async Task SaveAnswer_ReturnsNoContent()
    {
        var id = Guid.NewGuid();
        var questionId = Guid.NewGuid();
        var dto = new SaveAnswerRequestDto { AnswerNumber = 2 };
        _patientAssessmentServiceMock.Setup(x => x.SaveAnswerAsync(_patientUserId, id, questionId, dto)).Returns(Task.CompletedTask);

        var result = await _sut.SaveAnswer(id, questionId, dto);

        result.Should().BeOfType<NoContentResult>();
        _patientAssessmentServiceMock.Verify(x => x.SaveAnswerAsync(_patientUserId, id, questionId, dto), Times.Once);
    }

    [Fact]
    public async Task Submit_ReturnsOk_WithResult()
    {
        var id = Guid.NewGuid();
        var detail = new PatientAssessmentDetailDto { Id = id, Status = "Completed", TotalScore = 8, Severity = "Mild" };
        _patientAssessmentServiceMock.Setup(x => x.SubmitAsync(_patientUserId, id)).ReturnsAsync(detail);

        var result = await _sut.Submit(id);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(detail);
    }
}
