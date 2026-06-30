using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Exercise;

namespace Jalsa.Application.Interfaces.Repositories;

public interface IExerciseRepository : IGenericRepository<Exercise>
{
    Task<IEnumerable<Exercise>> GetByPatientIdAsync(Guid patientId);
    Task<IEnumerable<Exercise>> GetOverdueAsync();
    Task<IEnumerable<Exercise>> GetDueSoonAsync(int withinDays);
}
