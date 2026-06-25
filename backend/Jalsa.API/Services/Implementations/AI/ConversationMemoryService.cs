using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Domain.Models.Ai;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.API.Services.Implementations.AI;

public class ConversationMemoryService : IConversationMemoryService
{
    private readonly Galsa_DBDbContext _context;
    private readonly IEmbeddingService _embedding;

    public ConversationMemoryService(Galsa_DBDbContext context, IEmbeddingService embedding)
    {
        _context = context;
        _embedding = embedding;
    }

    public async Task StoreMessageMemoryAsync(Guid conversationId, Guid patientId, Guid messageId, string text)
    {
        var vector = await _embedding.GenerateEmbeddingAsync(text);

        _context.AiArtifacts.Add(new AiArtifact
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            PatientId = patientId,
            TriggerMessageId = messageId,
            SourceType = "Chat",
            ContentText = text,
            EmbeddingVector = VectorHelper.Serialize(vector),
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<string>> RetrieveSimilarMessagesAsync(Guid patientId, string query, int topK = 3)
    {
        var queryVec = await _embedding.GenerateEmbeddingAsync(query);

        var memories = await _context.AiArtifacts
            .Where(a => a.PatientId == patientId && a.SourceType == "Chat" && a.EmbeddingVector != null)
            .ToListAsync();

        var scored = memories
            .Select(m => new
            {
                m.ContentText,
                Score = VectorHelper.CosineSimilarity(
                    queryVec,
                    VectorHelper.Deserialize(m.EmbeddingVector!))
            })
            .Where(x => x.ContentText != null)
            .OrderByDescending(x => x.Score)
            .Take(topK)
            .Select(x => x.ContentText!)
            .ToList();

        return scored;
    }
}
