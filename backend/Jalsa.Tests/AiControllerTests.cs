using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Configurations;
using Jalsa.API.Controllers;
using Jalsa.API.DTOs.AI;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace Jalsa.Tests;

public class AiControllerTests
{
    private readonly Mock<ISummarizationService> _summarizationServiceMock;
    private readonly Mock<IReportGenerationService> _reportGenerationServiceMock;
    private readonly Mock<ITherapistChatAiService> _therapistChatAiMock;
    private readonly Mock<IEmbeddingService> _embeddingServiceMock;
    private readonly Mock<IVectorStore> _vectorStoreMock;
    private readonly AiController _sut;
    private readonly Guid _therapistUserId;

    public AiControllerTests()
    {
        _summarizationServiceMock = new Mock<ISummarizationService>();
        _reportGenerationServiceMock = new Mock<IReportGenerationService>();
        _therapistChatAiMock = new Mock<ITherapistChatAiService>();
        _embeddingServiceMock = new Mock<IEmbeddingService>();
        _vectorStoreMock = new Mock<IVectorStore>();

        var geminiSettings = Options.Create(new GeminiSettings
        {
            BaseUrl = "https://generativelanguage.googleapis.com/v1beta/",
            ApiKey = "test-key",
            ChatModelId = "gemini-2.5-flash",
            EmbeddingModelId = "gemini-embedding-001",
            EmbeddingDimensions = 768
        });

        _sut = new AiController(
            _summarizationServiceMock.Object,
            _reportGenerationServiceMock.Object,
            _therapistChatAiMock.Object,
            _embeddingServiceMock.Object,
            _vectorStoreMock.Object,
            geminiSettings,
            NullLogger<AiController>.Instance);

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
    public async Task SummarizePatient_ValidPatientId_ReturnsOkWithSummary()
    {
        var patientId = Guid.NewGuid();
        var request = new SummarizeRequest { Language = "ar" };
        var summary = "ملخص المريض: يعاني من اضطراب القلق";

        _summarizationServiceMock
            .Setup(x => x.SummarizePatientAsync(patientId, "ar", _therapistUserId))
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
            .Setup(x => x.GenerateDraftAsync(patientId, request.TherapistInstructions, "ar", _therapistUserId))
            .ReturnsAsync(draft);

        var result = await _sut.GenerateReportDraft(patientId, request);

        result.Should().BeOfType<OkObjectResult>();
    }
}
