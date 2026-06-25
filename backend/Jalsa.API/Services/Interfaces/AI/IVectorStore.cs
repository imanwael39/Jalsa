namespace Jalsa.API.Services.Interfaces.AI;

public class SearchResult
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public float Score { get; set; }
    public string? Metadata { get; set; }
}

public interface IVectorStore
{
    Task StoreAsync(Guid id, float[] vector, string text, string? metadata = null);
    Task<IReadOnlyList<SearchResult>> SearchAsync(float[] queryVector, int topK = 5, string? metadataFilter = null);
    Task UpdateMetadataAsync(Guid id, string metadata);
    Task DeleteAsync(Guid id);
}
