namespace Jalsa.Domain.Models.System;

public class SystemSetting
{
    public string Key { get; set; } = null!;
    public string? Value { get; set; }
    public DateTime UpdatedAt { get; set; }
}
