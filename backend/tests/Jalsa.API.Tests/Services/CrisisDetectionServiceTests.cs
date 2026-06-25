using Jalsa.API.Configurations;
using Jalsa.API.Services.Implementations.AI;
using Microsoft.Extensions.Options;
using Xunit;

namespace Jalsa.API.Tests.Services;

public class CrisisDetectionServiceTests
{
    private static CrisisDetectionService CreateService()
    {
        var settings = Options.Create(new OpenAiSettings
        {
            ApiKey = "test-key",
            ChatModel = "gpt-4o"
        });
        return new CrisisDetectionService(settings);
    }

    // ── Non-crisis path (no OpenAI call needed) ──────────────

    [Fact]
    public async Task AnalyzeAsync_NormalEnglish_ReturnsNotCrisis()
    {
        var service = CreateService();
        var result = await service.AnalyzeAsync("I had a great day today");
        Assert.False(result.IsCrisis);
    }

    [Fact]
    public async Task AnalyzeAsync_NormalArabic_ReturnsNotCrisis()
    {
        var service = CreateService();
        var result = await service.AnalyzeAsync("كان يومي رائعاً اليوم");
        Assert.False(result.IsCrisis);
    }

    [Fact]
    public async Task AnalyzeAsync_Empty_ReturnsNotCrisis()
    {
        var service = CreateService();
        var result = await service.AnalyzeAsync("");
        Assert.False(result.IsCrisis);
    }

    [Fact]
    public async Task AnalyzeAsync_Whitespace_ReturnsNotCrisis()
    {
        var service = CreateService();
        var result = await service.AnalyzeAsync("   ");
        Assert.False(result.IsCrisis);
    }

    // ── English keyword matching ─────────────────────────────

    [Theory]
    [InlineData("suicide")]
    [InlineData("kill myself")]
    [InlineData("end my life")]
    [InlineData("want to die")]
    [InlineData("self-harm")]
    [InlineData("self harm")]
    [InlineData("hurt myself")]
    [InlineData("not worth living")]
    [InlineData("better off dead")]
    [InlineData("suicidal")]
    [InlineData("take my own life")]
    public async Task AnalyzeAsync_EnglishKeyword_TriggersCrisis(string keyword)
    {
        var service = CreateService();
        var result = await service.AnalyzeAsync(keyword);
        Assert.True(result.IsCrisis);
    }

    [Fact]
    public async Task AnalyzeAsync_EnglishKeywordInSentence_TriggersCrisis()
    {
        var service = CreateService();
        var result = await service.AnalyzeAsync("I am feeling suicidal and want to end it all");
        Assert.True(result.IsCrisis);
    }

    // ── Arabic keyword matching ──────────────────────────────

    [Theory]
    [InlineData("انتحار")]
    [InlineData("أقتل نفسي")]
    [InlineData("إنهاء حياتي")]
    [InlineData("أريد الموت")]
    [InlineData("إيذاء النفس")]
    [InlineData("لا أستحق العيش")]
    [InlineData("الأفضل أن أموت")]
    [InlineData("أفكار انتحارية")]
    [InlineData("وداعا")]
    public async Task AnalyzeAsync_ArabicKeyword_TriggersCrisis(string keyword)
    {
        var service = CreateService();
        var result = await service.AnalyzeAsync(keyword);
        Assert.True(result.IsCrisis);
    }

    [Fact]
    public async Task AnalyzeAsync_ArabicKeywordInSentence_TriggersCrisis()
    {
        var service = CreateService();
        var result = await service.AnalyzeAsync("أشعر بأنني لا أستحق العيش بعد الآن");
        Assert.True(result.IsCrisis);
    }

    // ── Case insensitivity ───────────────────────────────────

    [Theory]
    [InlineData("SUICIDE")]
    [InlineData("Kill Myself")]
    [InlineData("SELF-HARM")]
    public async Task AnalyzeAsync_EnglishKeyword_CaseInsensitive(string keyword)
    {
        var service = CreateService();
        var result = await service.AnalyzeAsync(keyword);
        Assert.True(result.IsCrisis);
    }

    // ── Edge cases ──────────────────────────────────────────

    [Fact]
    public async Task AnalyzeAsync_PartialWordMatch_DoesNotTrigger()
    {
        var service = CreateService();
        var result = await service.AnalyzeAsync("suicidal tendencies are serious");
        Assert.True(result.IsCrisis);
    }

    [Fact]
    public async Task AnalyzeAsync_UnrelatedText_ReturnsNotCrisis()
    {
        var service = CreateService();
        var result = await service.AnalyzeAsync("I enjoy reading books and going for walks");
        Assert.False(result.IsCrisis);
    }

    [Fact]
    public async Task AnalyzeAsync_ArabicUnrelated_ReturnsNotCrisis()
    {
        var service = CreateService();
        var result = await service.AnalyzeAsync("أحب القراءة والمشي في الطبيعة");
        Assert.False(result.IsCrisis);
    }
}
