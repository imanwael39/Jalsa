namespace Jalsa.API.Services.Interfaces.AI;

public interface ISummarizationService
{
    Task<string> SummarizePatientAsync(Guid patientId, string language = "ar");
    Task<string> SummarizeSessionAsync(Guid sessionId, string language = "ar");
}
