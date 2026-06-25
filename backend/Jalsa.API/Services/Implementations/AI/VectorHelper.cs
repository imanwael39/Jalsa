using System.Text.Json;

namespace Jalsa.API.Services.Implementations.AI;

public static class VectorHelper
{
    public static string Serialize(float[] vector)
    {
        return JsonSerializer.Serialize(vector);
    }

    public static float[] Deserialize(string json)
    {
        return JsonSerializer.Deserialize<float[]>(json) ?? [];
    }

    public static float CosineSimilarity(float[] a, float[] b)
    {
        if (a.Length != b.Length)
            throw new ArgumentException("Vectors must have the same dimension");

        double dot = 0, normA = 0, normB = 0;
        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            normA += a[i] * a[i];
            normB += b[i] * b[i];
        }

        var denom = Math.Sqrt(normA) * Math.Sqrt(normB);
        return denom == 0 ? 0 : (float)(dot / denom);
    }
}
