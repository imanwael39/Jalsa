using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Domain.Models.Exercise;

namespace Jalsa.Application.Interfaces.Repositories;

public interface IExerciseLogRepository : IGenericRepository<ExerciseLog>
{
    Task<IEnumerable<ExerciseLog>> GetByExerciseIdAsync(Guid exerciseId);
    Task<IEnumerable<ExerciseLog>> GetByPatientIdAsync(Guid patientId);
}
