using Jalsa.Application.DTOs.Exercise;

namespace Jalsa.Application.Interfaces.Services;

public interface IExerciseService
{
    Task<ExerciseViewDto> CreateAsync(ExerciseCreateDto dto, Guid userId);
    Task<ExerciseViewDto> UpdateAsync(ExerciseUpdateDto dto, Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
    Task<ExerciseViewDto> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<ExerciseViewDto>> GetAllAsync(Guid userId);
    Task<IEnumerable<ExerciseViewDto>> GetByPatientIdAsync(Guid patientId, Guid userId);
    Task<IEnumerable<ExerciseViewDto>> GetByPatientIdAsync(Guid patientId);
    Task ExtendDueDateAsync(Guid id, DateOnly newDueDate, Guid userId);
    Task<ExerciseLogViewDto> LogCompletionAsync(ExerciseLogCreateDto dto);
    Task<IEnumerable<ExerciseLogViewDto>> GetLogsByPatientIdAsync(Guid patientId);
}
