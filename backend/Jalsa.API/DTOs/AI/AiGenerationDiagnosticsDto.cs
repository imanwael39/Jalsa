namespace Jalsa.API.DTOs.AI;

public class RagSourceDto
{
    public Guid SessionId { get; set; }
    public float Score { get; set; }
    public string? Source { get; set; }
    public string TextPreview { get; set; } = string.Empty;
}

public class AiGenerationDiagnosticsDto
{
    public string Output { get; set; } = string.Empty;
    public string SystemPrompt { get; set; } = string.Empty;
    public string UserPrompt { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int LatencyMs { get; set; }
    public int EmbeddingCount { get; set; }
    public int RagChunkCount { get; set; }
    public int? InputTokens { get; set; }
    public int? OutputTokens { get; set; }
    public int? TotalTokens { get; set; }
    public List<RagSourceDto> RagSources { get; set; } = new();
}
