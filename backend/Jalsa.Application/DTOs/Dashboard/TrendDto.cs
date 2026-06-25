namespace Jalsa.Application.DTOs.Dashboard;

public class TrendDto
{
    public DateTime Date { get; set; }
    public double Value { get; set; }
    public string Label { get; set; } = null!;
}
