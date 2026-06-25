using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Jalsa.Application.DTOs.Exercise;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Infrastructure.Data;

namespace Jalsa.API.Services.Implementations;

public class ExerciseService : IExerciseService
{
    private readonly Galsa_DBDbContext _context;
    private readonly IMapper _mapper;

    public ExerciseService(Galsa_DBDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PatientExerciseViewDto>> GetPatientExercisesAsync(Guid patientId, string? status = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Exercises
            .Where(e => e.PatientId == patientId);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(e => e.Status == status);

        var exercises = await query
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<PatientExerciseViewDto>>(exercises);
    }

    public async Task<PatientExerciseViewDto?> GetExerciseByIdAsync(Guid exerciseId, CancellationToken cancellationToken = default)
    {
        var exercise = await _context.Exercises
            .FirstOrDefaultAsync(e => e.Id == exerciseId, cancellationToken);

        return exercise is null ? null : _mapper.Map<PatientExerciseViewDto>(exercise);
    }

    public async Task<ExerciseLogViewDto?> UpdateExerciseStatusAsync(Guid exerciseId, Guid patientId, ExerciseStatusUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var exercise = await _context.Exercises
            .FirstOrDefaultAsync(e => e.Id == exerciseId && e.PatientId == patientId, cancellationToken);

        if (exercise is null) return null;

        exercise.Status = dto.Status;
        exercise.UpdatedAt = DateTime.UtcNow;

        var exerciseLog = new Domain.Models.Exercise.ExerciseLog
        {
            Id = Guid.NewGuid(),
            ExerciseId = exerciseId,
            PatientId = patientId,
            CompletionStatus = dto.Status,
            ReflectionNote = dto.ReflectionNote,
            LoggedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.ExerciseLogs.Add(exerciseLog);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ExerciseLogViewDto>(exerciseLog);
    }

    public async Task<bool> IsExerciseOwnedByPatientAsync(Guid exerciseId, Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Exercises
            .AnyAsync(e => e.Id == exerciseId && e.PatientId == patientId, cancellationToken);
    }
}
