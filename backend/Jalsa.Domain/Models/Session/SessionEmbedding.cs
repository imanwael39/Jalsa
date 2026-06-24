namespace Jalsa.Domain.Models.Session;

public class SessionEmbedding
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public int ChunkIndex { get; set; }
    public string? ChunkText { get; set; }
    public string? EmbeddingVector { get; set; }
    public DateTime CreatedAt { get; set; }

    public Session Session { get; set; } = null!;
}
