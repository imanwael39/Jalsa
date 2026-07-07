using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Identity;
using Jalsa.Domain.Models.Patient;

namespace Jalsa.Application.Services;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogService _auditLogService;
    private readonly IUserManagementService _userManagementService;

    public AdminDashboardService(
        IUnitOfWork unitOfWork,
        IAuditLogService auditLogService,
        IUserManagementService userManagementService)
    {
        _unitOfWork = unitOfWork;
        _auditLogService = auditLogService;
        _userManagementService = userManagementService;
    }

    public async Task<AdminDashboardDto> GetDashboardAsync()
    {
        var userQuery = _unitOfWork.Repository<User>().Query();
        var now = DateTime.UtcNow;

        var dto = new AdminDashboardDto
        {
            TotalUsers = userQuery.Count(),
            TotalTherapists = _unitOfWork.Repository<Therapist>().Query().Count(),
            TotalPatients = _unitOfWork.Repository<Patient>().Query().Count(),
            TotalAdmins = userQuery.Count(u => u.UserRoles.Any(ur => ur.Role.Name == "Admin")),
            ActiveUsers = userQuery.Count(u => u.IsActive && !u.IsDeleted),
            DisabledUsers = userQuery.Count(u => !u.IsActive && !u.IsDeleted),
            NewRegistrations7Days = userQuery.Count(u => u.CreatedAt >= now.AddDays(-7)),
            NewRegistrations30Days = userQuery.Count(u => u.CreatedAt >= now.AddDays(-30)),
            PendingDoctorApprovals = _unitOfWork.Repository<Therapist>().Query().Count(t => t.ApprovalStatus == TherapistApprovalStatus.Pending)
        };

        var recentLogs = await _auditLogService.GetLogsAsync(new AuditLogFilterDto { Page = 1, PageSize = 10 });
        dto.RecentAuditLogs = recentLogs.Items;

        var allUsers = await _userManagementService.GetAllUsersAsync();
        dto.LatestRegisteredUsers = allUsers.Take(10).ToList();

        return dto;
    }
}
