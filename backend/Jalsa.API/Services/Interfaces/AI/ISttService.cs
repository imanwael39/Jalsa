namespace Jalsa.API.Services.Interfaces.AI;

public interface ISttService
{
    Task<string> TranscribeAsync(Stream audioStream, string fileName, string language = "ar");
}
