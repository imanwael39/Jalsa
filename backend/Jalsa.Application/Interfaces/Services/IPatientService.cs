using Jalsa.Application.DTOs.Patient;

namespace Jalsa.Application.Interfaces.Services;

public interface IPatientService
{
    Task<PatientViewDto> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<PatientViewDto>> GetAllAsync(Guid userId, PatientFilterDto? filter = null);
    Task<PatientViewDto> CreateAsync(PatientCreateDto dto, Guid userId);
    Task<PatientViewDto> UpdateAsync(Guid id, PatientUpdateDto dto, Guid userId);
    Task ArchiveAsync(Guid id, Guid userId);
    Task RestoreAsync(Guid id, Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
}
