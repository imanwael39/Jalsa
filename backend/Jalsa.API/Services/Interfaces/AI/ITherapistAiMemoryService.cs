namespace Jalsa.API.Services.Interfaces.AI;

public interface ITherapistAiMemoryService
{
    Task StoreMessageMemoryAsync(Guid conversationId, Guid patientId, Guid messageId, string text);
    Task<IReadOnlyList<string>> RetrieveSimilarMessagesAsync(Guid patientId, string query, int topK = 3);
}
