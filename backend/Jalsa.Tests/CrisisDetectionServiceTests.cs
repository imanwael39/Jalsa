using FluentAssertions;
using Jalsa.API.Configurations;
using Jalsa.API.Services.Implementations.AI;
using Jalsa.API.Services.Interfaces;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Domain.Models.Crisis;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace Jalsa.Tests;

public class CrisisDetectionServiceTests
{
    private readonly Mock<IGeminiClient> _geminiMock;
    private readonly Mock<ILlmObservabilityService> _observabilityMock;
    private readonly Mock<IPromptService> _promptsMock;
    private readonly CrisisDetectionService _sut;

    public CrisisDetectionServiceTests()
    {
        _geminiMock = new Mock<IGeminiClient>();
        _observabilityMock = new Mock<ILlmObservabilityService>();
        _promptsMock = new Mock<IPromptService>();

        _promptsMock.Setup(p => p.Get("crisis-detection", It.IsAny<string>())).Returns("system prompt");
        _promptsMock
            .Setup(p => p.Get("crisis-detection", It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
            .Returns("user prompt");
        _promptsMock.Setup(p => p.GetFallbackReason("crisis-detection", It.IsAny<string>())).Returns("fallback reason");

        var settings = Options.Create(new GeminiSettings { ChatModelId = "gemini-2.5-flash" });

        _sut = new CrisisDetectionService(
            settings,
            _geminiMock.Object,
            _observabilityMock.Object,
            _promptsMock.Object,
            NullLogger<CrisisDetectionService>.Instance);
    }

    private void SetupModelResponse(string json) =>
        _geminiMock.Setup(c => c.ChatAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(json);

    [Fact]
    public async Task AnalyzeAsync_ModelReportsCriticalSuicidalIntent_ReturnsCrisisWithSeverityAndConfidence()
    {
        SetupModelResponse("""{"isCrisis": true, "severity": "Critical", "confidence": 0.95, "reason": "Explicit statement of intent to end their life."}""");

        var result = await _sut.AnalyzeAsync("عاوز أموت نفسي");

        result.IsCrisis.Should().BeTrue();
        result.Severity.Should().Be(CrisisSeverity.Critical);
        result.Confidence.Should().Be(0.95);
        result.Reason.Should().Contain("intent to end their life");
    }

    [Fact]
    public async Task AnalyzeAsync_ModelReportsNoCrisis_ReturnsNotCrisisRegardlessOfOtherFields()
    {
        SetupModelResponse("""{"isCrisis": false, "severity": "None", "confidence": 0.1}""");

        var result = await _sut.AnalyzeAsync("I had a great day today");

        result.IsCrisis.Should().BeFalse();
    }

    [Fact]
    public async Task AnalyzeAsync_DiscussingSomeoneElsesSuicide_ModelCorrectlyReturnsNotCrisis()
    {
        // Represents the model correctly distinguishing "my friend committed suicide" (a
        // discussion) from the patient's own current risk, per the false-positive
        // avoidance requirement in the system prompt.
        SetupModelResponse("""{"isCrisis": false, "severity": "None", "confidence": 0.05, "reason": "Patient is discussing a third party's death, not expressing personal risk."}""");

        var result = await _sut.AnalyzeAsync("My friend committed suicide last year.");

        result.IsCrisis.Should().BeFalse();
    }

    [Fact]
    public async Task AnalyzeAsync_MissingReasonInModelResponse_FallsBackToPromptServiceReason()
    {
        SetupModelResponse("""{"isCrisis": true, "severity": "High", "confidence": 0.7}""");

        var result = await _sut.AnalyzeAsync("I want to end it.");

        result.Reason.Should().Be("fallback reason");
    }

    [Fact]
    public async Task AnalyzeAsync_UnparseableModelResponse_ReturnsNotCrisisInsteadOfThrowing()
    {
        SetupModelResponse("not json at all");

        var result = await _sut.AnalyzeAsync("some message");

        result.IsCrisis.Should().BeFalse();
    }

    [Fact]
    public async Task AnalyzeAsync_GeminiClientThrows_ReturnsNotCrisisInsteadOfPropagating()
    {
        _geminiMock
            .Setup(c => c.ChatAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ThrowsAsync(new HttpRequestException("gateway down"));

        var result = await _sut.AnalyzeAsync("some message");

        result.IsCrisis.Should().BeFalse();
    }

    [Fact]
    public async Task AnalyzeAsync_PassesConversationHistoryIntoPrompt_WhenProvided()
    {
        SetupModelResponse("""{"isCrisis": false}""");
        var history = new[] { "Patient: I feel sad", "AI: I'm sorry to hear that" };

        await _sut.AnalyzeAsync("still sad", history);

        _promptsMock.Verify(
            p => p.Get(
                "crisis-detection",
                It.IsAny<string>(),
                It.Is<Dictionary<string, string>>(args => args["historyText"].Contains("I feel sad"))),
            Times.Once);
    }

    [Fact]
    public async Task AnalyzeAsync_EmptyMessage_ReturnsNotCrisisWithoutCallingModel()
    {
        var result = await _sut.AnalyzeAsync("   ");

        result.IsCrisis.Should().BeFalse();
        _geminiMock.Verify(c => c.ChatAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
    }
}
