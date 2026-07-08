using Jalsa.Application.DTOs.Dashboard;

namespace Jalsa.Application.DTOs.PatientProgress;

public class PatientProgressDto
{
    public List<TrendDto> AssessmentScoreTrend { get; set; } = new();
    public List<TrendDto> MoodTrend { get; set; } = new();
    public List<TrendDto> ExerciseCompletionTrend { get; set; } = new();
    public List<AttendanceBreakdownDto> AttendanceBreakdown { get; set; } = new();
    public ProgressStatisticsDto Statistics { get; set; } = new();
}
