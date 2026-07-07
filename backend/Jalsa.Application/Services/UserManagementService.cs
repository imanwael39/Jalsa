using System.Linq.Expressions;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Identity;

namespace Jalsa.Application.Services;

public class UserManagementService : IUserManagementService
{
    private static readonly Expression<Func<User, UserAdminViewDto>> ToDto = u => new UserAdminViewDto
    {
        Id = u.Id,
        Email = u.Email,
        FullName = u.Therapist != null ? u.Therapist.FullName : (u.Patient != null ? u.Patient.FullName : string.Empty),
        Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList(),
        IsActive = u.IsActive,
        IsDeleted = u.IsDeleted,
        IsLockedOut = u.LockoutEnd.HasValue && u.LockoutEnd.Value > DateTime.UtcNow,
        LastLoginAt = u.LastLoginAt,
        CreatedAt = u.CreatedAt
    };

    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogService _auditLogService;

    public UserManagementService(IUnitOfWork unitOfWork, IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _auditLogService = auditLogService;
    }

    public Task<IEnumerable<UserAdminViewDto>> GetAllUsersAsync()
    {
        var users = _unitOfWork.Repository<User>().Query()
            .OrderByDescending(u => u.CreatedAt)
            .Select(ToDto)
            .ToList();

        return Task.FromResult<IEnumerable<UserAdminViewDto>>(users);
    }

    public Task<PagedResultDto<UserAdminViewDto>> GetUsersAsync(UserFilterDto filter)
    {
        var query = _unitOfWork.Repository<User>().Query();

        if (!filter.IncludeDeleted)
            query = query.Where(u => !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var term = filter.SearchTerm.ToLower();
            query = query.Where(u =>
                u.Email.ToLower().Contains(term) ||
                (u.Therapist != null && u.Therapist.FullName.ToLower().Contains(term)) ||
                (u.Patient != null && u.Patient.FullName.ToLower().Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(filter.Role))
            query = query.Where(u => u.UserRoles.Any(ur => ur.Role.Name == filter.Role));

        if (filter.IsActive.HasValue)
            query = query.Where(u => u.IsActive == filter.IsActive.Value);

        var totalCount = query.Count();

        var page = filter.Page > 0 ? filter.Page : 1;
        var pageSize = filter.PageSize > 0 ? filter.PageSize : 20;

        var items = query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ToDto)
            .ToList();

        return Task.FromResult(new PagedResultDto<UserAdminViewDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<UserAdminViewDto?> SetActiveAsync(Guid userId, bool isActive, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null)
    {
        var user = await _unitOfWork.Repository<User>().FindSingleAsync(u => u.Id == userId);
        if (user is null)
            return null;

        var wasActive = user.IsActive;
        user.IsActive = isActive;
        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync(
            actingAdminUserId, "User", userId.ToString(), "User.SetActive",
            oldValues: new { IsActive = wasActive }, newValues: new { IsActive = isActive },
            ipAddress: ipAddress, userAgent: userAgent);

        return await GetDtoAsync(userId);
    }

    public async Task<UserAdminViewDto?> UnlockAsync(Guid userId, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null)
    {
        var user = await _unitOfWork.Repository<User>().FindSingleAsync(u => u.Id == userId);
        if (user is null)
            return null;

        user.FailedLoginAttempts = 0;
        user.LockoutEnd = null;
        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync(
            actingAdminUserId, "User", userId.ToString(), "User.Unlock",
            ipAddress: ipAddress, userAgent: userAgent);

        return await GetDtoAsync(userId);
    }

    public async Task<UserAdminViewDto?> ChangeRoleAsync(Guid userId, string roleName, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null)
    {
        var user = await _unitOfWork.Repository<User>().FindSingleAsync(u => u.Id == userId);
        if (user is null)
            return null;

        var role = await _unitOfWork.Repository<Role>().FindSingleAsync(r => r.Name == roleName)
            ?? throw new InvalidOperationException($"الدور '{roleName}' غير موجود.");

        var existingUserRoles = (await _unitOfWork.Repository<UserRole>().FindAsync(ur => ur.UserId == userId)).ToList();
        var oldRoleIds = existingUserRoles.Select(ur => ur.RoleId).ToList();
        _unitOfWork.Repository<UserRole>().RemoveRange(existingUserRoles);

        await _unitOfWork.Repository<UserRole>().AddAsync(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            CreatedAt = DateTime.UtcNow
        });

        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync(
            actingAdminUserId, "User", userId.ToString(), "User.ChangeRole",
            oldValues: new { RoleIds = oldRoleIds }, newValues: new { RoleName = roleName },
            ipAddress: ipAddress, userAgent: userAgent);

        return await GetDtoAsync(userId);
    }

    public async Task<UserAdminViewDto?> SoftDeleteAsync(Guid userId, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null)
    {
        var user = await _unitOfWork.Repository<User>().FindSingleAsync(u => u.Id == userId);
        if (user is null)
            return null;

        user.IsDeleted = true;
        user.UpdatedAt = DateTime.UtcNow;

        var activeTokens = (await _unitOfWork.Repository<RefreshToken>()
            .FindAsync(rt => rt.UserId == userId && rt.RevokedAt == null)).ToList();
        foreach (var token in activeTokens)
            token.RevokedAt = DateTime.UtcNow;
        _unitOfWork.Repository<RefreshToken>().UpdateRange(activeTokens);

        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync(
            actingAdminUserId, "User", userId.ToString(), "User.SoftDelete",
            ipAddress: ipAddress, userAgent: userAgent);

        return await GetDtoAsync(userId);
    }

    public async Task<UserAdminViewDto?> RestoreAsync(Guid userId, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null)
    {
        var user = await _unitOfWork.Repository<User>().FindSingleAsync(u => u.Id == userId);
        if (user is null)
            return null;

        user.IsDeleted = false;
        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync(
            actingAdminUserId, "User", userId.ToString(), "User.Restore",
            ipAddress: ipAddress, userAgent: userAgent);

        return await GetDtoAsync(userId);
    }

    public Task<PagedResultDto<AuditLogViewDto>> GetUserAuditHistoryAsync(Guid userId, int page, int pageSize)
    {
        return _auditLogService.GetLogsForUserAsync(userId, page, pageSize);
    }

    private Task<UserAdminViewDto?> GetDtoAsync(Guid userId)
    {
        var dto = _unitOfWork.Repository<User>().Query()
            .Where(u => u.Id == userId)
            .Select(ToDto)
            .FirstOrDefault();

        return Task.FromResult(dto);
    }
}
