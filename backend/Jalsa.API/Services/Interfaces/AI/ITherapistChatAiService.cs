using Jalsa.API.DTOs.AI;

namespace Jalsa.API.Services.Interfaces.AI;

public interface ITherapistChatAiService
{
    Task<string> AnswerQuestionAsync(Guid conversationId, Guid patientId, string question, string language = "ar");

    Task<AiGenerationDiagnosticsDto> AnswerQuestionWithDiagnosticsAsync(Guid conversationId, Guid patientId, string question, string language = "ar");

    Task<AiGenerationDiagnosticsDto> AnswerQuestionStreamingAsync(
        Guid conversationId, Guid patientId, string question, string language, Func<string, Task> onChunk, CancellationToken cancellationToken = default);
}
