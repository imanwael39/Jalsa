using Jalsa.Application.DTOs.Dashboard;

namespace Jalsa.Application.DTOs.PatientDashboard;

public class ProgressOverviewDto
{
    public double ExerciseCompletionRate { get; set; }
    public decimal? LatestAssessmentScore { get; set; }
    public decimal? PreviousAssessmentScore { get; set; }
    public List<TrendDto> AssessmentTrend { get; set; } = new();
    public int CompletedSessionsCount { get; set; }
}
