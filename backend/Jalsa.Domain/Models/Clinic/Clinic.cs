namespace Jalsa.Domain.Models.Clinic;

public class Clinic
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Timezone { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public ICollection<TherapistClinic> TherapistClinics { get; set; } = new List<TherapistClinic>();
    public ICollection<Patient.Patient> Patients { get; set; } = new List<Patient.Patient>();
}
