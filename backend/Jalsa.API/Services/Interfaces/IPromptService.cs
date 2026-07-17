namespace Jalsa.API.Services.Interfaces;

public interface IPromptService
{
    string Get(string key, string language = "ar");
    string Get(string key, string language, Dictionary<string, string> args);
    string GetEmbeddingQuery(string key, string language);
    string GetFallbackReason(string key, string language);
}
