namespace Jalsa.API.DTOs.Patient;

public class PatientFilterDto
{
    public string? SearchTerm { get; set; }
    public string? Status { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}