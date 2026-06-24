using Jalsa.Application.DTOs.Exercise;

namespace Jalsa.Application.Interfaces.Services;

public interface IExerciseService
{
    Task<ExerciseViewDto> CreateAsync(ExerciseCreateDto dto);
    Task<ExerciseViewDto> UpdateAsync(ExerciseUpdateDto dto);
    Task DeleteAsync(Guid id);
    Task<ExerciseViewDto> GetByIdAsync(Guid id);
    Task<IEnumerable<ExerciseViewDto>> GetAllAsync();
    Task<IEnumerable<ExerciseViewDto>> GetByPatientIdAsync(Guid patientId);
    Task ExtendDueDateAsync(Guid id, DateOnly newDueDate);
    Task<ExerciseLogViewDto> LogCompletionAsync(ExerciseLogCreateDto dto);
    Task<IEnumerable<ExerciseLogViewDto>> GetLogsByPatientIdAsync(Guid patientId);
}
