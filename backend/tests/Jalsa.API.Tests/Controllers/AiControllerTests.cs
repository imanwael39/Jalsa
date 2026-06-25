using Jalsa.API.Controllers;
using Jalsa.API.Services.Interfaces.AI;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Jalsa.API.Tests.Controllers;

public class AiControllerTests
{
    private readonly Mock<ISummarizationService> _summarizationMock = new();
    private readonly Mock<IReportGenerationService> _reportGenMock = new();
    private readonly AiController _controller;

    public AiControllerTests()
    {
        _controller = new AiController(_summarizationMock.Object, _reportGenMock.Object);
    }

    // ── Summarize ────────────────────────────────────────────

    [Fact]
    public async Task SummarizePatient_NoBody_DefaultsToArabic()
    {
        var patientId = Guid.NewGuid();
        _summarizationMock
            .Setup(s => s.SummarizePatientAsync(patientId, "ar"))
            .ReturnsAsync("ملخص بالعربية");

        var result = await _controller.SummarizePatient(patientId, null);

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value?.GetType().GetProperty("summary")?.GetValue(ok.Value);
        Assert.Equal("ملخص بالعربية", value);
    }

    [Fact]
    public async Task SummarizePatient_ExplicitArabic_ReturnsArabicSummary()
    {
        var patientId = Guid.NewGuid();
        _summarizationMock
            .Setup(s => s.SummarizePatientAsync(patientId, "ar"))
            .ReturnsAsync("ملخص سريري للمريض");

        var result = await _controller.SummarizePatient(patientId, new SummarizeRequest { Language = "ar" });

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value?.GetType().GetProperty("summary")?.GetValue(ok.Value);
        Assert.Equal("ملخص سريري للمريض", value);
    }

    [Fact]
    public async Task SummarizePatient_English_ReturnsEnglishSummary()
    {
        var patientId = Guid.NewGuid();
        _summarizationMock
            .Setup(s => s.SummarizePatientAsync(patientId, "en"))
            .ReturnsAsync("Clinical summary for patient");

        var result = await _controller.SummarizePatient(patientId, new SummarizeRequest { Language = "en" });

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value?.GetType().GetProperty("summary")?.GetValue(ok.Value);
        Assert.Equal("Clinical summary for patient", value);
    }

    [Fact]
    public async Task SummarizePatient_PassesCorrectLanguageToService()
    {
        var patientId = Guid.NewGuid();

        await _controller.SummarizePatient(patientId, new SummarizeRequest { Language = "en" });

        _summarizationMock.Verify(s => s.SummarizePatientAsync(patientId, "en"), Times.Once);
    }

    [Fact]
    public async Task SummarizePatient_DefaultLanguageIsArabic()
    {
        var patientId = Guid.NewGuid();

        await _controller.SummarizePatient(patientId, null);

        _summarizationMock.Verify(s => s.SummarizePatientAsync(patientId, "ar"), Times.Once);
    }

    // ── Report Draft ─────────────────────────────────────────

    [Fact]
    public async Task GenerateReportDraft_NoBody_DefaultsToArabic()
    {
        var patientId = Guid.NewGuid();
        _reportGenMock
            .Setup(r => r.GenerateDraftAsync(patientId, null, "ar"))
            .ReturnsAsync("تقرير بالعربية");

        var result = await _controller.GenerateReportDraft(patientId, null);

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value?.GetType().GetProperty("draft")?.GetValue(ok.Value);
        Assert.Equal("تقرير بالعربية", value);
    }

    [Fact]
    public async Task GenerateReportDraft_Arabic_ReturnsArabicDraft()
    {
        var patientId = Guid.NewGuid();
        _reportGenMock
            .Setup(r => r.GenerateDraftAsync(patientId, null, "ar"))
            .ReturnsAsync("مسودة تقرير الإحالة");

        var result = await _controller.GenerateReportDraft(patientId, new ReportDraftRequest
        {
            Language = "ar"
        });

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value?.GetType().GetProperty("draft")?.GetValue(ok.Value);
        Assert.Equal("مسودة تقرير الإحالة", value);
    }

    [Fact]
    public async Task GenerateReportDraft_English_ReturnsEnglishDraft()
    {
        var patientId = Guid.NewGuid();
        _reportGenMock
            .Setup(r => r.GenerateDraftAsync(patientId, null, "en"))
            .ReturnsAsync("Referral report draft");

        var result = await _controller.GenerateReportDraft(patientId, new ReportDraftRequest
        {
            Language = "en"
        });

        var ok = Assert.IsType<OkObjectResult>(result);
        var value = ok.Value?.GetType().GetProperty("draft")?.GetValue(ok.Value);
        Assert.Equal("Referral report draft", value);
    }

    [Fact]
    public async Task GenerateReportDraft_PassesInstructions()
    {
        var patientId = Guid.NewGuid();

        await _controller.GenerateReportDraft(patientId, new ReportDraftRequest
        {
            Language = "ar",
            TherapistInstructions = "focus on anxiety"
        });

        _reportGenMock.Verify(
            r => r.GenerateDraftAsync(patientId, "focus on anxiety", "ar"), Times.Once);
    }

    [Fact]
    public async Task GenerateReportDraft_PassesCorrectLanguageToService()
    {
        var patientId = Guid.NewGuid();

        await _controller.GenerateReportDraft(patientId, new ReportDraftRequest
        {
            Language = "en",
            TherapistInstructions = "trauma-focused"
        });

        _reportGenMock.Verify(
            r => r.GenerateDraftAsync(patientId, "trauma-focused", "en"), Times.Once);
    }

    [Fact]
    public async Task GenerateReportDraft_DefaultLanguageIsArabic()
    {
        var patientId = Guid.NewGuid();

        await _controller.GenerateReportDraft(patientId, null);

        _reportGenMock.Verify(r => r.GenerateDraftAsync(patientId, null, "ar"), Times.Once);
    }
}
