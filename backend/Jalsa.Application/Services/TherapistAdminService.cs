using System.Linq.Expressions;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Identity;
using Jalsa.Domain.Models.Patient;

namespace Jalsa.Application.Services;

public class TherapistAdminService : ITherapistAdminService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogService _auditLogService;

    public TherapistAdminService(IUnitOfWork unitOfWork, IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _auditLogService = auditLogService;
    }

    // Built per-instance (not a static field) because it closes over _unitOfWork to
    // express the correlated patient-count subquery — still a real Expression tree,
    // so Queryable.Select translates it server-side instead of falling back to
    // client-side evaluation (which would NPE on t.User being unloaded).
    private Expression<Func<Therapist, TherapistAdminViewDto>> ToDtoExpr()
    {
        var patientRepo = _unitOfWork.Repository<Patient>();
        return t => new TherapistAdminViewDto
        {
            Id = t.Id,
            UserId = t.UserId,
            FullName = t.FullName,
            Email = t.User.Email,
            LicenseNumber = t.LicenseNumber,
            Specialization = t.Specialization,
            ApprovalStatus = t.ApprovalStatus,
            ApprovalStatusUpdatedAt = t.ApprovalStatusUpdatedAt,
            PatientCount = patientRepo.Query().Count(p => p.TherapistId == t.Id),
            IsActive = t.User.IsActive,
            CreatedAt = t.CreatedAt,
            LastLoginAt = t.User.LastLoginAt
        };
    }

    public Task<PagedResultDto<TherapistAdminViewDto>> GetTherapistsAsync(TherapistFilterDto filter)
    {
        var query = _unitOfWork.Repository<Therapist>().Query();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var term = filter.SearchTerm.ToLower();
            query = query.Where(t =>
                t.FullName.ToLower().Contains(term) ||
                t.LicenseNumber.ToLower().Contains(term) ||
                t.User.Email.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(filter.ApprovalStatus))
            query = query.Where(t => t.ApprovalStatus == filter.ApprovalStatus);

        var totalCount = query.Count();

        var page = filter.Page > 0 ? filter.Page : 1;
        var pageSize = filter.PageSize > 0 ? filter.PageSize : 20;

        var items = query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ToDtoExpr())
            .ToList();

        return Task.FromResult(new PagedResultDto<TherapistAdminViewDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<TherapistAdminDetailDto?> GetTherapistDetailAsync(Guid therapistId)
    {
        var patientRepo = _unitOfWork.Repository<Patient>();

        var detail = _unitOfWork.Repository<Therapist>().Query()
            .Where(t => t.Id == therapistId)
            .Select(t => new TherapistAdminDetailDto
            {
                Id = t.Id,
                UserId = t.UserId,
                FullName = t.FullName,
                Email = t.User.Email,
                LicenseNumber = t.LicenseNumber,
                Specialization = t.Specialization,
                ApprovalStatus = t.ApprovalStatus,
                ApprovalStatusUpdatedAt = t.ApprovalStatusUpdatedAt,
                PatientCount = patientRepo.Query().Count(p => p.TherapistId == t.Id),
                IsActive = t.User.IsActive,
                CreatedAt = t.CreatedAt,
                LastLoginAt = t.User.LastLoginAt,
                Phone = t.Phone,
                Bio = t.Bio,
                ProfileImageUrl = t.ProfileImageUrl
            })
            .FirstOrDefault();

        if (detail is null)
            return null;

        var auditLogs = await _auditLogService.GetLogsAsync(new AuditLogFilterDto
        {
            EntityName = "Therapist",
            EntityId = therapistId.ToString(),
            Page = 1,
            PageSize = 10
        });
        detail.RecentAuditLogs = auditLogs.Items;

        return detail;
    }

    public async Task<TherapistAdminViewDto?> UpdateApprovalStatusAsync(
        Guid therapistId,
        string newStatus,
        Guid? actingAdminUserId = null,
        string? ipAddress = null,
        string? userAgent = null)
    {
        if (!TherapistApprovalStatus.All.Contains(newStatus))
            throw new InvalidOperationException($"حالة الاعتماد '{newStatus}' غير صالحة.");

        var therapist = await _unitOfWork.Repository<Therapist>().FindSingleAsync(t => t.Id == therapistId);
        if (therapist is null)
            return null;

        var oldStatus = therapist.ApprovalStatus;
        therapist.ApprovalStatus = newStatus;
        therapist.ApprovalStatusUpdatedAt = DateTime.UtcNow;
        _unitOfWork.Repository<Therapist>().Update(therapist);

        if (newStatus is TherapistApprovalStatus.Suspended or TherapistApprovalStatus.Rejected)
        {
            var activeTokens = (await _unitOfWork.Repository<RefreshToken>()
                .FindAsync(rt => rt.UserId == therapist.UserId && rt.RevokedAt == null)).ToList();

            foreach (var token in activeTokens)
                token.RevokedAt = DateTime.UtcNow;

            _unitOfWork.Repository<RefreshToken>().UpdateRange(activeTokens);
        }

        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync(
            actingAdminUserId, "Therapist", therapistId.ToString(), "Therapist.ApprovalStatus",
            oldValues: new { ApprovalStatus = oldStatus }, newValues: new { ApprovalStatus = newStatus },
            ipAddress: ipAddress, userAgent: userAgent);

        return _unitOfWork.Repository<Therapist>().Query()
            .Where(t => t.Id == therapistId)
            .Select(ToDtoExpr())
            .FirstOrDefault();
    }
}
