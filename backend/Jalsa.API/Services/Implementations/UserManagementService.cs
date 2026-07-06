using Microsoft.EntityFrameworkCore;
using Jalsa.API.DTOs.Admin;
using Jalsa.API.Exceptions;
using Jalsa.API.Services.Interfaces;
using Jalsa.Domain.Models.Identity;
using Jalsa.Infrastructure.Data;

namespace Jalsa.API.Services.Implementations;

public class UserManagementService : IUserManagementService
{
    private readonly JalsaDbContext _context;

    public UserManagementService(JalsaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserAdminViewDto>> GetUsersAsync()
    {
        var users = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(u => u.Therapist)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();

        return users.Select(MapToDto);
    }

    public async Task<UserAdminViewDto?> SetActiveAsync(Guid userId, bool isActive)
    {
        var user = await FindUserAsync(userId);
        if (user is null)
            return null;

        user.IsActive = isActive;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return MapToDto(user);
    }

    public async Task<UserAdminViewDto?> UnlockAsync(Guid userId)
    {
        var user = await FindUserAsync(userId);
        if (user is null)
            return null;

        user.FailedLoginAttempts = 0;
        user.LockoutEnd = null;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return MapToDto(user);
    }

    public async Task<UserAdminViewDto?> ChangeRoleAsync(Guid userId, string roleName)
    {
        var user = await FindUserAsync(userId);
        if (user is null)
            return null;

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName)
            ?? throw new ApiException(400, $"Role '{roleName}' does not exist.");

        _context.UserRoles.RemoveRange(user.UserRoles);
        user.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id,
            CreatedAt = DateTime.UtcNow
        });
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return MapToDto(user);
    }

    private async Task<User?> FindUserAsync(Guid userId) =>
        await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(u => u.Therapist)
            .FirstOrDefaultAsync(u => u.Id == userId);

    private static UserAdminViewDto MapToDto(User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        FullName = user.Therapist?.FullName ?? string.Empty,
        Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList(),
        IsActive = user.IsActive,
        IsLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow,
        LastLoginAt = user.LastLoginAt,
        CreatedAt = user.CreatedAt
    };
}
