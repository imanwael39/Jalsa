namespace Jalsa.Application.DTOs.Admin;

public class AdminDashboardDto
{
    public int TotalUsers { get; set; }
    public int TotalTherapists { get; set; }
    public int TotalPatients { get; set; }
    public int TotalAdmins { get; set; }
    public int ActiveUsers { get; set; }
    public int DisabledUsers { get; set; }
    public int NewRegistrations7Days { get; set; }
    public int NewRegistrations30Days { get; set; }
    public int PendingDoctorApprovals { get; set; }
    public List<AuditLogViewDto> RecentAuditLogs { get; set; } = [];
    public List<UserAdminViewDto> LatestRegisteredUsers { get; set; } = [];
}
