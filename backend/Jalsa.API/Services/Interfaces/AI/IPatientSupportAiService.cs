namespace Jalsa.API.Services.Interfaces.AI;

public interface IPatientSupportAiService
{
    Task<string> GenerateResponseAsync(Guid conversationId, Guid patientId, string message);

    Task<string> GenerateResponseStreamingAsync(
        Guid conversationId, Guid patientId, string message, Func<string, Task> onChunk, CancellationToken cancellationToken = default);
}
