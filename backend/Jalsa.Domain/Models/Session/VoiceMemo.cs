namespace Jalsa.Domain.Models.Session;

public class VoiceMemo
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public string? AudioUrl { get; set; }
    public string? Transcript { get; set; }
    public int? DurationSeconds { get; set; }
    public DateTime CreatedAt { get; set; }

    public Session Session { get; set; } = null!;
}
