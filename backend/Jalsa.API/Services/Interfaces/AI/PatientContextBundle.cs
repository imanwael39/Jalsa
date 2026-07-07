namespace Jalsa.API.Services.Interfaces.AI;

public class PatientDemographics
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? ReferralSource { get; set; }
    public string? ChiefComplaint { get; set; }
    public string? MedicalHistory { get; set; }
    public string? EmergencyContact { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
}

public class IntakeContext
{
    public string? PresentingProblem { get; set; }
    public string? PsychiatricHistory { get; set; }
    public string? FamilyHistory { get; set; }
    public string? Medications { get; set; }
    public string? SocialHistory { get; set; }
    public string Status { get; set; } = "Draft";
    public DateTime? SubmittedAt { get; set; }
}

public class AssessmentContext
{
    public string? Title { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public DateOnly? AssessmentDate { get; set; }
    public decimal? TotalScore { get; set; }
    public string? Severity { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = "Active";
}

public class SessionNoteContext
{
    public Guid SessionId { get; set; }
    public int SessionNumber { get; set; }
    public DateOnly SessionDate { get; set; }
    public string? SessionType { get; set; }
    public int? DurationMinutes { get; set; }
    public string? Observations { get; set; }
    public string? Interventions { get; set; }
    public string? PatientResponse { get; set; }
    public string? HomeworkAssigned { get; set; }
    public string? NextGoals { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class VoiceTranscriptContext
{
    public Guid SessionId { get; set; }
    public int SessionNumber { get; set; }
    public string Transcript { get; set; } = string.Empty;
}

public class RagContextChunk
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public float Score { get; set; }
    public string? Source { get; set; }
}

public class PatientContextBundle
{
    public PatientDemographics? Demographics { get; set; }
    public IntakeContext? Intake { get; set; }
    public IReadOnlyList<AssessmentContext> Assessments { get; set; } = Array.Empty<AssessmentContext>();
    public IReadOnlyList<SessionNoteContext> RecentSessionNotes { get; set; } = Array.Empty<SessionNoteContext>();
    public IReadOnlyList<VoiceTranscriptContext> VoiceTranscripts { get; set; } = Array.Empty<VoiceTranscriptContext>();
    public IReadOnlyList<RagContextChunk> RagChunks { get; set; } = Array.Empty<RagContextChunk>();
    public int EmbeddingCount { get; set; }
}
