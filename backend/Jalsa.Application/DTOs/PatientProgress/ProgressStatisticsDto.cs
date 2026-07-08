namespace Jalsa.Application.DTOs.PatientProgress;

public class ProgressStatisticsDto
{
    public int TotalSessions { get; set; }
    public int CompletedSessions { get; set; }
    public double AttendanceRate { get; set; }
    public int TotalExercises { get; set; }
    public int CompletedExercises { get; set; }
    public double ExerciseCompletionRate { get; set; }
    public int TotalAssessments { get; set; }
    public decimal? LatestAssessmentScore { get; set; }
    public decimal? FirstAssessmentScore { get; set; }
    public double? ImprovementPercentage { get; set; }
}
