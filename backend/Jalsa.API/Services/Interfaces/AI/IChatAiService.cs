namespace Jalsa.API.Services.Interfaces.AI;

public interface IChatAiService
{
    Task<string> GenerateResponseAsync(Guid conversationId, Guid patientId, string message);
}
