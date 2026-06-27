using Jalsa.Application.DTOs.Intake;

namespace Jalsa.Application.Interfaces.Services;

public interface IIntakeService
{
    Task<IntakeFormViewDto> GetByPatientIdAsync(Guid patientId, Guid therapistId, CancellationToken ct = default);
    Task<IntakeFormViewDto> SaveAsync(Guid patientId, IntakeFormSaveDto dto, Guid therapistId, CancellationToken ct = default);
    Task<IntakeFormViewDto> SubmitAsync(Guid patientId, Guid therapistId, CancellationToken ct = default);
}
