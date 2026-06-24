namespace Jalsa.Domain.Models.Clinic;

public class TherapistClinic
{
    public Guid TherapistId { get; set; }
    public Guid ClinicId { get; set; }
    public bool IsPrimary { get; set; }

    public Therapist Therapist { get; set; } = null!;
    public Clinic Clinic { get; set; } = null!;
}
