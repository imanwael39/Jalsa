using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Domain.Models.Exercise;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.Infrastructure.Repositories;

public class ExerciseRepository : GenericRepository<Exercise>, IExerciseRepository
{
    public ExerciseRepository(DbContext context) : base(context) { }

    public async Task<IEnumerable<Exercise>> GetByPatientIdAsync(Guid patientId)
        => await _dbSet.Where(e => e.PatientId == patientId).ToListAsync();

    public async Task<IEnumerable<Exercise>> GetOverdueAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return await _dbSet.Where(e => e.DueDate < today && e.Status != "Completed").ToListAsync();
    }

    public async Task<IEnumerable<Exercise>> GetDueSoonAsync(int withinDays)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var deadline = today.AddDays(withinDays);
        return await _dbSet.Where(e => e.DueDate >= today && e.DueDate <= deadline && e.Status != "Completed").ToListAsync();
    }
}
