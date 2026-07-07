namespace Jalsa.API.DTOs.Admin;

public class SystemHealthDto
{
    public long ProcessMemoryMb { get; set; }
    public int Gen0Collections { get; set; }
    public int Gen1Collections { get; set; }
    public int Gen2Collections { get; set; }
    public decimal? DatabaseSizeMb { get; set; }
    public long? HangfireEnqueued { get; set; }
    public long? HangfireProcessing { get; set; }
    public long? HangfireSucceeded { get; set; }
    public long? HangfireFailed { get; set; }
    public int AiChatCallsToday { get; set; }
    public int AiChatCallsThisMonth { get; set; }
    public int AiReportCallsToday { get; set; }
    public int AiReportCallsThisMonth { get; set; }
    public long AiTokensUsedThisMonth { get; set; }
}
