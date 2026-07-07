namespace Jalsa.API.Services.Interfaces.AI;

public class SearchResult
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public float Score { get; set; }
    public string? Metadata { get; set; }
    public Guid? SessionId { get; set; }
    public Guid? PatientId { get; set; }
    public string? Source { get; set; }
}

public class VectorSearchQuery
{
    public float[] QueryVector { get; set; } = Array.Empty<float>();
    public int TopK { get; set; } = 5;
    public Guid? PatientId { get; set; }
    public Guid? SessionId { get; set; }
    public string? Source { get; set; }
    public double MinScore { get; set; } = 0.0;
}

public interface IVectorStore
{
    Task StoreAsync(
        Guid id,
        Guid sessionId,
        Guid patientId,
        float[] vector,
        string text,
        string source = "SessionNote",
        string? metadata = null);

    Task<IReadOnlyList<SearchResult>> SearchAsync(
        float[] queryVector,
        int topK = 5,
        string? metadataFilter = null);

    Task<IReadOnlyList<SearchResult>> SearchAsync(VectorSearchQuery query);

    Task UpdateMetadataAsync(Guid id, string metadata);

    Task DeleteAsync(Guid id);

    Task DeleteBySessionAsync(Guid sessionId, string? source = null);

    Task<int> CountAsync(Guid? patientId = null, string? source = null);
}
