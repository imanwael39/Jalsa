namespace Jalsa.Domain.Models.Patient;

public class IntakeFormOcrExtraction
{
    public Guid Id { get; set; }
    public Guid IntakeFormId { get; set; }
    public string? ModelUsed { get; set; }
    public string? ExtractedJson { get; set; }
    public decimal? Confidence { get; set; }
    public string? RawOcrText { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public IntakeForm IntakeForm { get; set; } = null!;
}
