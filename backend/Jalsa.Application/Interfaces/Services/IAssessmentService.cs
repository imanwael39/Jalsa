using Jalsa.Application.DTOs.Assessment;

namespace Jalsa.Application.Interfaces.Services;

public interface IAssessmentService
{
    Task<IEnumerable<AssessmentViewDto>> GetByPatientIdAsync(Guid patientId, Guid therapistId, CancellationToken ct = default);
    Task<AssessmentViewDto> CreateAsync(Guid patientId, AssessmentCreateDto dto, Guid therapistId, CancellationToken ct = default);
}
