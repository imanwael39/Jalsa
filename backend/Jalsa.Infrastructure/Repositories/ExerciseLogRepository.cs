using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Exercise;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.Infrastructure.Repositories;

public class ExerciseLogRepository : GenericRepository<ExerciseLog>, IExerciseLogRepository
{
    public ExerciseLogRepository(JalsaDbContext context) : base(context) { }

    public async Task<IEnumerable<ExerciseLog>> GetByExerciseIdAsync(Guid exerciseId)
        => await _dbSet.Where(el => el.ExerciseId == exerciseId).ToListAsync();

    public async Task<IEnumerable<ExerciseLog>> GetByPatientIdAsync(Guid patientId)
        => await _dbSet.Where(el => el.PatientId == patientId).ToListAsync();
}
