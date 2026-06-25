namespace Jalsa.Application.DTOs.Dashboard;

public class DashboardSummaryDto
{
    public int TotalPatients { get; set; }
    public int ActivePatients { get; set; }
    public int ArchivedPatients { get; set; }
    public int TotalSessions { get; set; }
    public int SessionsThisMonth { get; set; }
    public double ExerciseCompletionRate { get; set; }
    public double AverageAssessmentScore { get; set; }
    public List<string> RecentAlerts { get; set; } = new();
    public AnalyticsDto Analytics { get; set; } = new();
}
