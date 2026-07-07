namespace Jalsa.Application.DTOs.PatientDashboard;

public class PatientDashboardDto
{
    public string PatientFirstName { get; set; } = "";
    public List<UpcomingSessionDto> UpcomingSessions { get; set; } = new();
    public List<TodayReminderDto> TodayReminders { get; set; } = new();
    public List<AssignedExerciseSummaryDto> AssignedExercises { get; set; } = new();
    public List<PendingAssessmentDto> PendingAssessments { get; set; } = new();
    public List<RecentConversationDto> RecentConversations { get; set; } = new();
    public ProgressOverviewDto ProgressOverview { get; set; } = new();
    public TherapistInfoDto? Therapist { get; set; }
    public CrisisSupportInfoDto CrisisSupport { get; set; } = new();
}
