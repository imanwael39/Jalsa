namespace Jalsa.Domain.Models.File;

public class UploadedFile
{
    public Guid Id { get; set; }
    public Guid? PatientId { get; set; }
    public Guid? SessionId { get; set; }
    public string BlobUrl { get; set; } = null!;
    public string? FileType { get; set; }
    public long? SizeBytes { get; set; }
    public string? Sha256 { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UploadedByUserId { get; set; }

    public Patient.Patient? Patient { get; set; }
    public Session.Session? Session { get; set; }
    public Identity.User UploadedByUser { get; set; } = null!;
}
