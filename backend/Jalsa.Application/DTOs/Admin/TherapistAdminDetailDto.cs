namespace Jalsa.Application.DTOs.Admin;

public class TherapistAdminDetailDto : TherapistAdminViewDto
{
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }
    public List<AuditLogViewDto> RecentAuditLogs { get; set; } = [];
}
