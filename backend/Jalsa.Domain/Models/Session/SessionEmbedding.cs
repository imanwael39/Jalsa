namespace Jalsa.Domain.Models.Session;

public class SessionEmbedding
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Guid PatientId { get; set; }
    public int ChunkIndex { get; set; }
    public string Source { get; set; } = "SessionNote";
    public string? ChunkText { get; set; }
    public string? EmbeddingVector { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Session Session { get; set; } = null!;
}
