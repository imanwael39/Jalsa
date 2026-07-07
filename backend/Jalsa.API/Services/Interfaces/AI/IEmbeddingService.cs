namespace Jalsa.API.Services.Interfaces.AI;

public interface IEmbeddingService
{
    Task<float[]> GenerateEmbeddingAsync(string text, string inputType = "search_document");
}
