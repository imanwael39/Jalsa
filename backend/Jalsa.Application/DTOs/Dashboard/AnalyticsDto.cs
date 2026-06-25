namespace Jalsa.Application.DTOs.Dashboard;

public class AnalyticsDto
{
    public List<TrendDto> AssessmentTrend { get; set; } = new();
    public ExerciseCompletionBreakdownDto ExerciseCompletion { get; set; } = new();
    public List<TrendDto> SessionFrequency { get; set; } = new();
}
