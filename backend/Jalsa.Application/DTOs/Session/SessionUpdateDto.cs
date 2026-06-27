namespace Jalsa.Application.DTOs.Session;

public class SessionUpdateDto
{
    public Guid Id { get; set; }
    public DateOnly? SessionDate { get; set; }
    public int? DurationMinutes { get; set; }
    public string? SessionType { get; set; }
    public string? Status { get; set; }
}
