using Jalsa.Application.DTOs.Exercise;

namespace Jalsa.Application.Interfaces.Services;

public interface IExerciseService
{
    Task<IEnumerable<PatientExerciseViewDto>> GetPatientExercisesAsync(Guid patientId, string? status = null, CancellationToken cancellationToken = default);
    Task<PatientExerciseViewDto?> GetExerciseByIdAsync(Guid exerciseId, CancellationToken cancellationToken = default);
    Task<ExerciseLogViewDto?> UpdateExerciseStatusAsync(Guid exerciseId, Guid patientId, ExerciseStatusUpdateDto dto, CancellationToken cancellationToken = default);
    Task<bool> IsExerciseOwnedByPatientAsync(Guid exerciseId, Guid patientId, CancellationToken cancellationToken = default);
}
