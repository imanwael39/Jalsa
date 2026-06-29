namespace Jalsa.Application.DTOs.Session;

public class VoiceMemoViewDto
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public string? Transcript { get; set; }
    public DateTime CreatedAt { get; set; }
}
