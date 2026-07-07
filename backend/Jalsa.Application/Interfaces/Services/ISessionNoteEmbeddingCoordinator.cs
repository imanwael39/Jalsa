namespace Jalsa.Application.Interfaces.Services;

/// <summary>
/// Application-layer abstraction over the AI module's session-note embedding
/// pipeline. Keeps the Application layer framework-agnostic and prevents a
/// circular dependency on the API project's AI services.
/// </summary>
public interface ISessionNoteEmbeddingCoordinator
{
    /// <summary>
    /// Synchronises the SessionNote with its SessionEmbedding row(s).
    /// Creates, updates, or removes the embedding depending on note content.
    /// </summary>
    Task SyncNoteAsync(Domain.Models.Session.SessionNote note, CancellationToken ct = default);

    /// <summary>
    /// Removes every embedding that belongs to the given session.
    /// </summary>
    Task RemoveForSessionAsync(Guid sessionId, CancellationToken ct = default);
}
