namespace Jalsa.API.Services.Interfaces.AI;

public interface IReportGenerationService
{
    Task<string> GenerateDraftAsync(Guid patientId, string? therapistInstructions = null, string language = "ar");
}
