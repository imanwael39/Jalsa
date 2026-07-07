using Jalsa.API.DTOs.AI;

namespace Jalsa.API.Services.Interfaces.AI;

public interface ISummarizationService
{
    Task<string> SummarizePatientAsync(Guid patientId, string language = "ar", Guid? requestedByUserId = null);
    Task<string> SummarizeSessionAsync(Guid sessionId, string language = "ar", Guid? requestedByUserId = null);

    Task<AiGenerationDiagnosticsDto> SummarizePatientWithDiagnosticsAsync(Guid patientId, string language = "ar", Guid? requestedByUserId = null);
}
