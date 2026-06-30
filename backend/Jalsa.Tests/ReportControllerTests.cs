using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.DTOs.Report;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class ReportControllerTests
{
    private readonly Mock<IReportService> _reportServiceMock;
    private readonly Mock<IReportGenerationService> _aiServiceMock;
    private readonly ReportController _sut;
    private readonly Guid _therapistUserId;

    public ReportControllerTests()
    {
        _reportServiceMock = new Mock<IReportService>();
        _aiServiceMock = new Mock<IReportGenerationService>();

        _sut = new ReportController(_reportServiceMock.Object, _aiServiceMock.Object);

        _therapistUserId = Guid.NewGuid();
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, _therapistUserId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    private static ReportViewDto MakeReportDto(Guid? id = null, Guid? patientId = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        PatientId = patientId ?? Guid.NewGuid(),
        TherapistId = Guid.NewGuid(),
        GeneratedByTherapistId = Guid.NewGuid(),
        Status = "Draft",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        CurrentVersion = new ReportVersionViewDto
        {
            Id = Guid.NewGuid(),
            ReportId = Guid.NewGuid(),
            VersionNumber = 1,
            Content = "محتوى التقرير",
            CreatedAt = DateTime.UtcNow
        },
        Versions = new List<ReportVersionViewDto>()
    };

    [Fact]
    public async Task Generate_ValidDto_Returns201Created()
    {
        var dto = new ReportGenerateDto { PatientId = Guid.NewGuid(), Language = "ar" };
        var aiContent = "محتوى مولد بالذكاء الاصطناعي";
        var created = MakeReportDto(patientId: dto.PatientId);

        _aiServiceMock
            .Setup(x => x.GenerateDraftAsync(dto.PatientId, dto.TherapistInstructions, dto.Language))
            .ReturnsAsync(aiContent);

        _reportServiceMock
            .Setup(x => x.CreateWithAiContentAsync(dto.PatientId, aiContent, _therapistUserId))
            .ReturnsAsync(created);

        var result = await _sut.Generate(dto);

        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(created);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsOk()
    {
        var reportId = Guid.NewGuid();
        var dto = MakeReportDto(id: reportId);

        _reportServiceMock
            .Setup(x => x.GetByIdAsync(reportId, _therapistUserId))
            .ReturnsAsync(dto);

        var result = await _sut.GetById(reportId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetByPatientId_ExistingPatient_ReturnsOkWithList()
    {
        var patientId = Guid.NewGuid();
        var reports = new List<ReportViewDto> { MakeReportDto(patientId: patientId) };

        _reportServiceMock
            .Setup(x => x.GetByPatientIdAsync(patientId, _therapistUserId))
            .ReturnsAsync(reports);

        var result = await _sut.GetByPatientId(patientId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = ok.Value.Should().BeAssignableTo<IEnumerable<ReportViewDto>>().Subject;
        value.Should().HaveCount(1);
    }

    [Fact]
    public async Task Update_ValidDto_ReturnsOk()
    {
        var reportId = Guid.NewGuid();
        var dto = new ReportUpdateDto { Content = "محتوى محدث" };
        var updated = MakeReportDto(id: reportId);

        _reportServiceMock
            .Setup(x => x.UpdateAsync(reportId, dto, _therapistUserId))
            .ReturnsAsync(updated);

        var result = await _sut.Update(reportId, dto);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(updated);
    }

    [Fact]
    public async Task Approve_ExistingId_ReturnsOk()
    {
        var reportId = Guid.NewGuid();
        var approved = MakeReportDto(id: reportId);
        approved.Status = "Approved";

        _reportServiceMock
            .Setup(x => x.ApproveAsync(reportId, _therapistUserId))
            .ReturnsAsync(approved);

        var result = await _sut.Approve(reportId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ((ReportViewDto)ok.Value!).Status.Should().Be("Approved");
    }

    [Fact]
    public async Task Reject_ExistingId_ReturnsOk()
    {
        var reportId = Guid.NewGuid();
        var rejected = MakeReportDto(id: reportId);
        rejected.Status = "Rejected";

        _reportServiceMock
            .Setup(x => x.RejectAsync(reportId, _therapistUserId))
            .ReturnsAsync(rejected);

        var result = await _sut.Reject(reportId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ((ReportViewDto)ok.Value!).Status.Should().Be("Rejected");
    }

    [Fact]
    public async Task Export_ExistingId_ReturnsFileResult()
    {
        var reportId = Guid.NewGuid();
        var report = MakeReportDto(id: reportId);

        _reportServiceMock
            .Setup(x => x.GetByIdAsync(reportId, _therapistUserId))
            .ReturnsAsync(report);

        var result = await _sut.Export(reportId);

        var fileResult = result.Should().BeOfType<FileContentResult>().Subject;
        fileResult.ContentType.Should().Be("text/html; charset=utf-8");
        fileResult.FileDownloadName.Should().Be($"report-{reportId}.html");
    }

    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        var reportId = Guid.NewGuid();
        _reportServiceMock
            .Setup(x => x.DeleteAsync(reportId, _therapistUserId))
            .Returns(Task.CompletedTask);

        var result = await _sut.Delete(reportId);

        result.Should().BeOfType<NoContentResult>();
        _reportServiceMock.Verify(x => x.DeleteAsync(reportId, _therapistUserId), Times.Once);
    }
}
