using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Domain.Models.Ai;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.API.Services.Implementations.AI;

/// <summary>
/// Semantic recall scoped exclusively to the therapist clinical assistant. Backed by
/// TherapistAiMemories — never shares rows with PatientSupportMemories, so a therapist's
/// Q&amp;A history can never surface in a patient's support chat retrieval, or vice versa.
/// </summary>
public class TherapistAiMemoryService : ITherapistAiMemoryService
{
    private readonly JalsaDbContext _context;
    private readonly IEmbeddingService _embedding;

    public TherapistAiMemoryService(JalsaDbContext context, IEmbeddingService embedding)
    {
        _context = context;
        _embedding = embedding;
    }

    public async Task StoreMessageMemoryAsync(Guid conversationId, Guid patientId, Guid messageId, string text)
    {
        var vector = await _embedding.GenerateEmbeddingAsync(text);

        _context.TherapistAiMemories.Add(new TherapistAiMemory
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            PatientId = patientId,
            TriggerMessageId = messageId,
            ContentText = text,
            EmbeddingVector = VectorHelper.Serialize(vector),
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
    }

    private const double SemanticWeight = 0.75;
    private const double RecencyWeight = 0.25;
    private const int MaxCandidates = 200;

    public async Task<IReadOnlyList<string>> RetrieveSimilarMessagesAsync(Guid patientId, string query, int topK = 3)
    {
        var queryVec = await _embedding.GenerateEmbeddingAsync(query);

        var memories = await _context.TherapistAiMemories
            .Where(m => m.PatientId == patientId && m.EmbeddingVector != null)
            .OrderByDescending(m => m.CreatedAt)
            .Take(MaxCandidates)
            .ToListAsync();

        if (memories.Count == 0)
            return Array.Empty<string>();

        var now = DateTime.UtcNow;
        var oldestAgeHours = Math.Max(1.0, memories.Max(m => (now - m.CreatedAt).TotalHours));

        var scored = memories
            .Where(m => m.ContentText != null)
            .Select(m =>
            {
                var semanticScore = VectorHelper.CosineSimilarity(
                    queryVec,
                    VectorHelper.Deserialize(m.EmbeddingVector!));

                var ageHours = (now - m.CreatedAt).TotalHours;
                var recencyScore = 1.0 - Math.Min(1.0, ageHours / oldestAgeHours);

                return new
                {
                    m.ContentText,
                    Combined = (semanticScore * SemanticWeight) + (recencyScore * RecencyWeight)
                };
            })
            .OrderByDescending(x => x.Combined)
            .Take(topK)
            .Select(x => x.ContentText!)
            .ToList();

        return scored;
    }
}
