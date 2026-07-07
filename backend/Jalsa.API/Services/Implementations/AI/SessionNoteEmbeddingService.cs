using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Domain.Models.Session;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.API.Services.Implementations.AI;

public class SessionNoteEmbeddingService : ISessionNoteEmbeddingService
{
    private const string Source = "SessionNote";
    private const int MaxChunkChars = 6000;

    private readonly JalsaDbContext _context;
    private readonly IEmbeddingService _embedding;
    private readonly IVectorStore _vectorStore;
    private readonly ILogger<SessionNoteEmbeddingService> _logger;

    public SessionNoteEmbeddingService(
        JalsaDbContext context,
        IEmbeddingService embedding,
        IVectorStore vectorStore,
        ILogger<SessionNoteEmbeddingService> logger)
    {
        _context = context;
        _embedding = embedding;
        _vectorStore = vectorStore;
        _logger = logger;
    }

    public async Task<int> SyncNoteAsync(SessionNote note, CancellationToken ct = default)
    {
        if (note is null) throw new ArgumentNullException(nameof(note));

        var session = await _context.Sessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == note.SessionId, ct);

        if (session is null)
        {
            _logger.LogWarning("Cannot sync embeddings: session {SessionId} not found.", note.SessionId);
            return 0;
        }

        var composedText = ComposeNoteText(note);

        if (string.IsNullOrWhiteSpace(composedText))
        {
            await _vectorStore.DeleteBySessionAsync(session.Id, Source);
            return 0;
        }

        var existing = await _context.SessionEmbeddings
            .Where(e => e.SessionId == session.Id && e.Source == Source && e.ChunkIndex == 0)
            .ToListAsync(ct);

        var newEmbeddingText = composedText;
        if (existing.Count > 0)
        {
            if (string.Equals(existing[0].ChunkText, newEmbeddingText, StringComparison.Ordinal))
            {
                return 0;
            }

            _context.SessionEmbeddings.RemoveRange(existing);
            await _context.SaveChangesAsync(ct);
        }

        var vector = await _embedding.GenerateEmbeddingAsync(newEmbeddingText, "search_document");
        if (vector is null || vector.Length == 0)
        {
            _logger.LogWarning("Empty embedding returned for note {NoteId}; skipping persist.", note.Id);
            return 0;
        }

        await _vectorStore.StoreAsync(
            id: Guid.NewGuid(),
            sessionId: session.Id,
            patientId: session.PatientId,
            vector: vector,
            text: Truncate(newEmbeddingText, MaxChunkChars),
            source: Source);

        return 1;
    }

    public async Task RemoveForSessionAsync(Guid sessionId, CancellationToken ct = default)
    {
        await _vectorStore.DeleteBySessionAsync(sessionId, Source);
    }

    private static string ComposeNoteText(SessionNote n)
    {
        var parts = new List<string>(6);

        void Add(string? label, string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            parts.Add($"{label}: {value.Trim()}");
        }

        Add("Observations", n.Observations);
        Add("Interventions", n.Interventions);
        Add("PatientResponse", n.PatientResponse);
        Add("Homework", n.HomeworkAssigned);
        Add("NextGoals", n.NextGoals);

        return string.Join("\n\n", parts);
    }

    private static string Truncate(string text, int max)
    {
        if (string.IsNullOrEmpty(text) || text.Length <= max) return text;
        return text[..max];
    }
}
