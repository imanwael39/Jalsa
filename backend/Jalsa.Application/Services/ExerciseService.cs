using Jalsa.Application.DTOs.Exercise;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Exercise;

namespace Jalsa.Application.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IExerciseLogRepository _exerciseLogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExerciseService(
        IExerciseRepository exerciseRepository,
        IExerciseLogRepository exerciseLogRepository,
        IUnitOfWork unitOfWork)
    {
        _exerciseRepository = exerciseRepository;
        _exerciseLogRepository = exerciseLogRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ExerciseViewDto> CreateAsync(ExerciseCreateDto dto)
    {
        var exercise = new Exercise
        {
            Id = Guid.NewGuid(),
            PatientId = dto.PatientId,
            Description = dto.Description,
            Frequency = dto.Frequency,
            StartDate = dto.StartDate,
            DueDate = dto.DueDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _exerciseRepository.AddAsync(exercise);
        await _unitOfWork.SaveChangesAsync();

        return MapToViewDto(exercise);
    }

    public async Task<ExerciseViewDto> UpdateAsync(ExerciseUpdateDto dto)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Exercise with ID {dto.Id} not found.");

        if (dto.Description is not null)
            exercise.Description = dto.Description;
        if (dto.Frequency is not null)
            exercise.Frequency = dto.Frequency;
        if (dto.DueDate.HasValue)
            exercise.DueDate = dto.DueDate;
        if (dto.Status is not null)
            exercise.Status = dto.Status;

        exercise.UpdatedAt = DateTime.UtcNow;

        _exerciseRepository.Update(exercise);
        await _unitOfWork.SaveChangesAsync();

        return MapToViewDto(exercise);
    }

    public async Task DeleteAsync(Guid id)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Exercise with ID {id} not found.");

        _exerciseRepository.Remove(exercise);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ExerciseViewDto> GetByIdAsync(Guid id)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Exercise with ID {id} not found.");

        return MapToViewDto(exercise);
    }

    public async Task<IEnumerable<ExerciseViewDto>> GetAllAsync()
    {
        var exercises = await _exerciseRepository.GetAllAsync();
        return exercises.Select(MapToViewDto);
    }

    public async Task<IEnumerable<ExerciseViewDto>> GetByPatientIdAsync(Guid patientId)
    {
        var exercises = await _exerciseRepository.GetByPatientIdAsync(patientId);
        return exercises.Select(MapToViewDto);
    }

    public async Task ExtendDueDateAsync(Guid id, DateOnly newDueDate)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Exercise with ID {id} not found.");

        exercise.DueDate = newDueDate;
        exercise.UpdatedAt = DateTime.UtcNow;

        _exerciseRepository.Update(exercise);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ExerciseLogViewDto> LogCompletionAsync(ExerciseLogCreateDto dto)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(dto.ExerciseId)
            ?? throw new KeyNotFoundException($"Exercise with ID {dto.ExerciseId} not found.");

        if (exercise.PatientId != dto.PatientId)
            throw new UnauthorizedAccessException("Exercise does not belong to this patient.");

        var log = new ExerciseLog
        {
            Id = Guid.NewGuid(),
            ExerciseId = dto.ExerciseId,
            PatientId = dto.PatientId,
            CompletionStatus = dto.CompletionStatus,
            ReflectionNote = dto.ReflectionNote,
            CreatedAt = DateTime.UtcNow,
            LoggedAt = DateTime.UtcNow
        };

        await _exerciseLogRepository.AddAsync(log);
        await _unitOfWork.SaveChangesAsync();

        return MapToLogViewDto(log);
    }

    public async Task<IEnumerable<ExerciseLogViewDto>> GetLogsByPatientIdAsync(Guid patientId)
    {
        var logs = await _exerciseLogRepository.GetByPatientIdAsync(patientId);
        return logs.Select(MapToLogViewDto);
    }

    private static ExerciseViewDto MapToViewDto(Exercise exercise) => new()
    {
        Id = exercise.Id,
        PatientId = exercise.PatientId,
        Description = exercise.Description,
        Frequency = exercise.Frequency,
        StartDate = exercise.StartDate,
        DueDate = exercise.DueDate,
        Status = exercise.Status,
        CreatedAt = exercise.CreatedAt
    };

    private static ExerciseLogViewDto MapToLogViewDto(ExerciseLog log) => new()
    {
        Id = log.Id,
        ExerciseId = log.ExerciseId,
        PatientId = log.PatientId,
        CompletionStatus = log.CompletionStatus,
        ReflectionNote = log.ReflectionNote,
        LoggedAt = log.LoggedAt ?? log.CreatedAt,
        CreatedAt = log.CreatedAt
    };
}
