namespace Jalsa.API.Services.Interfaces.AI;

public interface IPatientContextBuilder
{
    /// <summary>
    /// Builds the full clinical context bundle for a patient, including patient-scoped RAG chunks.
    /// The RAG query text is resolved from the "embeddingQuery" template for <paramref name="embeddingQueryKey"/>
    /// in prompts.json (with "{patientName}" substituted), unless <paramref name="explicitEmbeddingQuery"/>
    /// is provided, in which case that raw text (e.g. the user's live message/question) is embedded instead.
    /// </summary>
    Task<PatientContextBundle> BuildAsync(
        Guid patientId,
        string embeddingQueryKey,
        string language,
        int ragTopK = 5,
        int recentSessionCount = 5,
        string? explicitEmbeddingQuery = null,
        CancellationToken ct = default);
}
