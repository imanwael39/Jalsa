using Jalsa.Domain.Models.Session;

namespace Jalsa.API.Services.Interfaces.AI;

public interface ISessionNoteEmbeddingService
{
    /// <summary>
    /// Synchronises the SessionNote with SessionEmbeddings. Creates an embedding
    /// when the note is new, updates the existing embedding when the note text has
    /// changed, and removes the embedding when the note is empty.
    /// Returns the number of embeddings stored or removed.
    /// </summary>
    Task<int> SyncNoteAsync(SessionNote note, CancellationToken ct = default);

    /// <summary>
    /// Removes all embeddings that belong to a session (used when a session is deleted).
    /// </summary>
    Task RemoveForSessionAsync(Guid sessionId, CancellationToken ct = default);
}
