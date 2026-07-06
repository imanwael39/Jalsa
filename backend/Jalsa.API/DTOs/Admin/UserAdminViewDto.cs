namespace Jalsa.API.DTOs.Admin;

public class UserAdminViewDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];
    public bool IsActive { get; set; }
    public bool IsLockedOut { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ChangeRoleDto
{
    public string RoleName { get; set; } = string.Empty;
}
