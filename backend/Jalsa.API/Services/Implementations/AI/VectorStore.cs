using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Jalsa.Domain.Models.Session;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.API.Services.Implementations.AI;

public class VectorStore : IVectorStore
{
    private readonly JalsaDbContext _context;

    public VectorStore(JalsaDbContext context)
    {
        _context = context;
    }

    public async Task StoreAsync(
        Guid id,
        Guid sessionId,
        Guid patientId,
        float[] vector,
        string text,
        string source = "SessionNote",
        string? metadata = null)
    {
        if (vector is null || vector.Length == 0)
            throw new ArgumentException("Vector must not be empty.", nameof(vector));

        var now = DateTime.UtcNow;
        var embedding = new SessionEmbedding
        {
            Id = id,
            SessionId = sessionId,
            PatientId = patientId,
            Source = source,
            ChunkText = text,
            EmbeddingVector = VectorHelper.Serialize(vector),
            CreatedAt = now,
            UpdatedAt = now
        };

        _context.SessionEmbeddings.Add(embedding);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<SearchResult>> SearchAsync(
        float[] queryVector,
        int topK = 5,
        string? metadataFilter = null)
    {
        var query = new VectorSearchQuery
        {
            QueryVector = queryVector,
            TopK = topK
        };

        if (!string.IsNullOrWhiteSpace(metadataFilter))
        {
            var parts = metadataFilter.Split('=', 2);
            if (parts.Length == 2)
            {
                if (parts[0] == "SessionId" && Guid.TryParse(parts[1], out var sessionId))
                    query.SessionId = sessionId;
                else if (parts[0] == "PatientId" && Guid.TryParse(parts[1], out var patientId))
                    query.PatientId = patientId;
            }
        }

        return await SearchAsync(query);
    }

    public async Task<IReadOnlyList<SearchResult>> SearchAsync(VectorSearchQuery query)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));
        if (query.QueryVector is null || query.QueryVector.Length == 0)
            return Array.Empty<SearchResult>();

        var sql = _context.SessionEmbeddings.AsNoTracking().AsQueryable();

        if (query.PatientId.HasValue)
            sql = sql.Where(e => e.PatientId == query.PatientId.Value);

        if (query.SessionId.HasValue)
            sql = sql.Where(e => e.SessionId == query.SessionId.Value);

        if (!string.IsNullOrWhiteSpace(query.Source))
            sql = sql.Where(e => e.Source == query.Source);

        var embeddings = await sql.ToListAsync();

        var scored = new List<SearchResult>(embeddings.Count);
        foreach (var e in embeddings)
        {
            if (string.IsNullOrEmpty(e.EmbeddingVector)) continue;
            float[] vector;
            try
            {
                vector = VectorHelper.Deserialize(e.EmbeddingVector);
            }
            catch
            {
                continue;
            }

            if (vector.Length != query.QueryVector.Length) continue;

            var score = VectorHelper.CosineSimilarity(query.QueryVector, vector);
            if (score < query.MinScore) continue;

            scored.Add(new SearchResult
            {
                Id = e.Id,
                Text = e.ChunkText ?? string.Empty,
                Score = score,
                SessionId = e.SessionId,
                PatientId = e.PatientId,
                Source = e.Source,
                Metadata = e.SessionId.ToString()
            });
        }

        return scored
            .OrderByDescending(s => s.Score)
            .Take(Math.Max(1, query.TopK))
            .ToList();
    }

    public async Task UpdateMetadataAsync(Guid id, string metadata)
    {
        var embedding = await _context.SessionEmbeddings.FindAsync(id);
        if (embedding is null) return;

        embedding.UpdatedAt = DateTime.UtcNow;
        _context.SessionEmbeddings.Update(embedding);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var embedding = await _context.SessionEmbeddings.FindAsync(id);
        if (embedding is null) return;

        _context.SessionEmbeddings.Remove(embedding);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBySessionAsync(Guid sessionId, string? source = null)
    {
        var query = _context.SessionEmbeddings.Where(e => e.SessionId == sessionId);
        if (!string.IsNullOrWhiteSpace(source))
            query = query.Where(e => e.Source == source);

        var items = await query.ToListAsync();
        if (items.Count == 0) return;

        _context.SessionEmbeddings.RemoveRange(items);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync(Guid? patientId = null, string? source = null)
    {
        var query = _context.SessionEmbeddings.AsNoTracking().AsQueryable();
        if (patientId.HasValue)
            query = query.Where(e => e.PatientId == patientId.Value);
        if (!string.IsNullOrWhiteSpace(source))
            query = query.Where(e => e.Source == source);
        return await query.CountAsync();
    }
}
