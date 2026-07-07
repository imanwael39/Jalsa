namespace Jalsa.Application.DTOs.Admin;

public class AuditLogFilterDto
{
    public Guid? UserId { get; set; }
    public string? EntityName { get; set; }
    public string? EntityId { get; set; }
    public string? Action { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
