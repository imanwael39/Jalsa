namespace Jalsa.Application.DTOs.Chat;

public class ChatEngagementSummaryDto
{
    public Guid PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int TotalMessages { get; set; }
    public double AverageMessagesPerDay { get; set; }
    public Dictionary<string, int> SentimentDistribution { get; set; } = new();
    public int CrisisFlagsCount { get; set; }
    public DateTime? LastMessageTimestamp { get; set; }
    public bool IsActive { get; set; }
}
