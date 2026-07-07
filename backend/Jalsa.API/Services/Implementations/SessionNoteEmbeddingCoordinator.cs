using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Services.Implementations;

/// <summary>
/// Adapts the API-layer <see cref="ISessionNoteEmbeddingService"/> to the
/// Application-layer <see cref="ISessionNoteEmbeddingCoordinator"/>. This keeps
/// the Application layer free of API-only dependencies while still letting
/// SessionService drive the embedding pipeline on note save.
/// </summary>
public class SessionNoteEmbeddingCoordinator : ISessionNoteEmbeddingCoordinator
{
    private readonly ISessionNoteEmbeddingService _service;

    public SessionNoteEmbeddingCoordinator(ISessionNoteEmbeddingService service)
    {
        _service = service;
    }

    public Task SyncNoteAsync(Domain.Models.Session.SessionNote note, CancellationToken ct = default)
    {
        return _service.SyncNoteAsync(note, ct);
    }

    public Task RemoveForSessionAsync(Guid sessionId, CancellationToken ct = default)
    {
        return _service.RemoveForSessionAsync(sessionId, ct);
    }
}
