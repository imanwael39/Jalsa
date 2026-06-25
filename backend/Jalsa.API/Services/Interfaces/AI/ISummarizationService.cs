namespace Jalsa.API.Services.Interfaces.AI;

public interface ISummarizationService
{
    Task<string> SummarizePatientAsync(Guid patientId, string language = "ar");
}
