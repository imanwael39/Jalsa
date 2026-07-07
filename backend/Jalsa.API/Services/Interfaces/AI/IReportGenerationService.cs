using Jalsa.API.DTOs.AI;

namespace Jalsa.API.Services.Interfaces.AI;

public interface IReportGenerationService
{
    Task<string> GenerateDraftAsync(Guid patientId, string? therapistInstructions = null, string language = "ar", Guid? requestedByUserId = null);

    Task<AiGenerationDiagnosticsDto> GenerateDraftWithDiagnosticsAsync(Guid patientId, string? therapistInstructions = null, string language = "ar", Guid? requestedByUserId = null);
}
