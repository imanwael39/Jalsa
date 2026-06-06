namespace Jalsa.Domain.Models.Notification;

public class Notification
{
    public Guid Id { get; set; }
    public Guid RecipientUserId { get; set; }
    public string Type { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Body { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public Identity.User RecipientUser { get; set; } = null!;
}
