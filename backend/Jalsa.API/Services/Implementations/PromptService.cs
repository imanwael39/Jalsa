using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Hosting;

namespace Jalsa.API.Services.Implementations;

public class PromptService : Services.Interfaces.IPromptService
{
    private readonly JsonNode _root;

    public PromptService(IWebHostEnvironment env)
    {
        var path = Path.Combine(env.ContentRootPath, "prompts", "prompts.json");
        var json = File.ReadAllText(path);
        _root = JsonNode.Parse(json)!;
    }

    public string Get(string key, string language = "ar")
    {
        var node = _root["prompts"]?[key];
        if (node == null) return string.Empty;

        return node["system"] switch
        {
            JsonValue val => val.GetValue<string>(),
            JsonObject obj => obj[language]?.GetValue<string>() ?? string.Empty,
            _ => string.Empty
        };
    }

    public string Get(string key, string language, Dictionary<string, string> args)
    {
        var node = _root["prompts"]?[key];
        if (node == null) return string.Empty;

        var template = node["user"] switch
        {
            JsonValue val => val.GetValue<string>(),
            JsonObject obj => obj[language]?.GetValue<string>() ?? string.Empty,
            _ => string.Empty
        };

        if (string.IsNullOrEmpty(template)) return string.Empty;

        var result = template;
        foreach (var kvp in args)
        {
            result = result.Replace($"{{{kvp.Key}}}", kvp.Value);
        }
        return result;
    }

    public string GetEmbeddingQuery(string key, string language)
    {
        return _root["prompts"]?[key]?["embeddingQuery"]?[language]?.GetValue<string>() ?? string.Empty;
    }

    public string[] GetKeywords(string key)
    {
        var arr = _root["prompts"]?[key]?["keywords"] as JsonArray;
        return arr?.Select(n => n?.GetValue<string>() ?? string.Empty)
                   .Where(s => !string.IsNullOrEmpty(s))
                   .ToArray() ?? [];
    }

    public string GetFallbackMessage(string key, string language)
    {
        return _root["prompts"]?[key]?["fallbackMessage"]?[language]?.GetValue<string>() ?? string.Empty;
    }

    public string GetFallbackReason(string key, string language)
    {
        return _root["prompts"]?[key]?["fallbackReason"]?[language]?.GetValue<string>() ?? string.Empty;
    }
}
