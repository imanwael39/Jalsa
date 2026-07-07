namespace Jalsa.Application.DTOs.Admin;

public class TherapistFilterDto
{
    public string? SearchTerm { get; set; }
    public string? ApprovalStatus { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
