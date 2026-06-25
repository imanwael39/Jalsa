using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Jalsa.Domain.Models.Session;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.API.Services.Implementations.AI;

public class VectorStore : IVectorStore
{
    private readonly Galsa_DBDbContext _context;

    public VectorStore(Galsa_DBDbContext context)
    {
        _context = context;
    }

    public async Task StoreAsync(Guid id, float[] vector, string text, string? metadata = null)
    {
        var embedding = new SessionEmbedding
        {
            Id = id,
            ChunkText = text,
            EmbeddingVector = VectorHelper.Serialize(vector),
            CreatedAt = DateTime.UtcNow
        };

        _context.SessionEmbeddings.Add(embedding);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<SearchResult>> SearchAsync(float[] queryVector, int topK = 5, string? metadataFilter = null)
    {
        var query = _context.SessionEmbeddings.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(metadataFilter))
        {
            var parts = metadataFilter.Split('=');
            if (parts.Length == 2)
            {
                if (parts[0] == "SessionId" && Guid.TryParse(parts[1], out var sessionId))
                    query = query.Where(e => e.SessionId == sessionId);
            }
        }

        var embeddings = await query.ToListAsync();

        var scored = embeddings
            .Select(e => new
            {
                e.Id,
                e.ChunkText,
                Vector = !string.IsNullOrEmpty(e.EmbeddingVector)
                    ? VectorHelper.Deserialize(e.EmbeddingVector)
                    : null,
                e.SessionId
            })
            .Where(x => x.Vector != null)
            .Select(x => new SearchResult
            {
                Id = x.Id,
                Text = x.ChunkText ?? "",
                Score = VectorHelper.CosineSimilarity(queryVector, x.Vector!),
                Metadata = x.SessionId.ToString()
            })
            .OrderByDescending(x => x.Score)
            .Take(topK)
            .ToList();

        return scored;
    }

    public async Task UpdateMetadataAsync(Guid id, string metadata)
    {
        var embedding = await _context.SessionEmbeddings.FindAsync(id);
        if (embedding != null)
        {
            _context.SessionEmbeddings.Update(embedding);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var embedding = await _context.SessionEmbeddings.FindAsync(id);
        if (embedding != null)
        {
            _context.SessionEmbeddings.Remove(embedding);
            await _context.SaveChangesAsync();
        }
    }
}
