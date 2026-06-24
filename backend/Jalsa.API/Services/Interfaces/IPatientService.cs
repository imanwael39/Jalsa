using Jalsa.API.DTOs.Patient;
namespace Jalsa.API.DTOs.Patient;
public interface IPatientService
{
    Task<PatientResponseDTO> GetByIdAsync(Guid id, Guid currentUserId);
    Task<IEnumerable<PatientResponseDTO>> GetAllAsync(Guid currentUserId, PatientFilterDto? filter = null);
    Task<PatientResponseDTO> CreateAsync(CreatePatientDTO dto, Guid currentUserId);
    Task<PatientResponseDTO> UpdateAsync(Guid id, UpdatePatientDto dto, Guid currentUserId);
    Task ArchiveAsync(Guid id, Guid currentUserId);
    Task RestoreAsync(Guid id, Guid currentUserId);
    Task DeleteAsync(Guid id, Guid currentUserId);
}

