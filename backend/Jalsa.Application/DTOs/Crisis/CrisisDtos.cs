namespace Jalsa.Application.DTOs.Crisis;

public class CrisisAlertViewDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; }
}
