using Jalsa.Domain.Models.Crisis;

namespace Jalsa.API.Services.Interfaces.AI;

public class CrisisDetectionResult
{
    public bool IsCrisis { get; set; }
    public CrisisSeverity Severity { get; set; } = CrisisSeverity.None;
    public string? Reason { get; set; }
    public double Confidence { get; set; }
}

/// <summary>
/// Classifies whether a patient message indicates a current, personal mental-health
/// crisis (not a discussion of the topic in the abstract). Consumed only by the Patient
/// Support Chat pipeline (PatientSupportChatHub) via this interface — never a concrete
/// type — so the detection engine (LLM, Azure AI Content Safety, a custom model, ...) can
/// be swapped without touching the chat flow. Must never be wired into the Therapist AI
/// Assistant.
/// </summary>
public interface ICrisisDetectionService
{
    /// <param name="message">The patient's current message.</param>
    /// <param name="conversationHistory">
    /// Optional recent turns (oldest first) giving the classifier context — e.g. so
    /// "yes" after "are you thinking about hurting yourself?" can be read correctly.
    /// </param>
    Task<CrisisDetectionResult> AnalyzeAsync(string message, IReadOnlyList<string>? conversationHistory = null);
}
