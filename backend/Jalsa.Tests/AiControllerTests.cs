using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.API.DTOs.AI;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class AiControllerTests
{
    private readonly Mock<ISummarizationService> _summarizationServiceMock;
    private readonly Mock<IReportGenerationService> _reportGenerationServiceMock;
    private readonly AiController _sut;

    public AiControllerTests()
    {
        _summarizationServiceMock = new Mock<ISummarizationService>();
        _reportGenerationServiceMock = new Mock<IReportGenerationService>();

        _sut = new AiController(
            _summarizationServiceMock.Object,
            _reportGenerationServiceMock.Object);
    }

    [Fact]
    public async Task SummarizePatient_ValidPatientId_ReturnsOkWithSummary()
    {
        var patientId = Guid.NewGuid();
        var request = new SummarizeRequest { Language = "ar" };
        var summary = "ملخص المريض: يعاني من اضطراب القلق";

        _summarizationServiceMock
            .Setup(x => x.SummarizePatientAsync(patientId, "ar"))
            .ReturnsAsync(summary);

        var result = await _sut.SummarizePatient(patientId, request);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GenerateReportDraft_ValidPatientId_ReturnsOkWithDraft()
    {
        var patientId = Guid.NewGuid();
        var request = new ReportDraftRequest { TherapistInstructions = "ركز على التقدم", Language = "ar" };
        var draft = "مسودة التقرير الطبي";

        _reportGenerationServiceMock
            .Setup(x => x.GenerateDraftAsync(patientId, request.TherapistInstructions, "ar"))
            .ReturnsAsync(draft);

        var result = await _sut.GenerateReportDraft(patientId, request);

        result.Should().BeOfType<OkObjectResult>();
    }
}
