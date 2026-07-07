namespace Jalsa.Application.DTOs.PatientDashboard;

public class TherapistInfoDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = "";
    public string? Specialization { get; set; }
    public string? Phone { get; set; }
    public string? ProfileImageUrl { get; set; }
}
