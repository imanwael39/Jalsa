using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Exercise;

namespace Jalsa.Application.Interfaces.Repositories;

public interface IExerciseLogRepository : IGenericRepository<ExerciseLog>
{
    Task<IEnumerable<ExerciseLog>> GetByExerciseIdAsync(Guid exerciseId);
    Task<IEnumerable<ExerciseLog>> GetByPatientIdAsync(Guid patientId);
}
