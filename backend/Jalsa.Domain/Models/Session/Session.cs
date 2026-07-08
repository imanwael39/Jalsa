namespace Jalsa.Domain.Models.Session;

public class Session
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid? IntakeFormId { get; set; }
    public int SessionNumber { get; set; }
    public DateOnly SessionDate { get; set; }
    public int? DurationMinutes { get; set; }
    public string? SessionType { get; set; }
    public string Status { get; set; } = "Draft";
    public string? PatientRequestType { get; set; }
    public string? PatientRequestNote { get; set; }
    public string? PatientRequestStatus { get; set; }
    public DateTime? PatientRequestedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Patient.Patient Patient { get; set; } = null!;
    public Patient.IntakeForm? IntakeForm { get; set; }
    public SessionNote? SessionNote { get; set; }
    public ICollection<SessionEmbedding> SessionEmbeddings { get; set; } = new List<SessionEmbedding>();
    public ICollection<VoiceMemo> VoiceMemos { get; set; } = new List<VoiceMemo>();
    public ICollection<File.UploadedFile> UploadedFiles { get; set; } = new List<File.UploadedFile>();
    public ICollection<Assessment.Assessment> Assessments { get; set; } = new List<Assessment.Assessment>();
}
