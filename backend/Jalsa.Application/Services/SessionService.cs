using Jalsa.Application.DTOs.Session;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Session;

namespace Jalsa.Application.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SessionService(
        ISessionRepository sessionRepository,
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SessionViewDto> CreateAsync(SessionCreateDto dto, Guid therapistId)
    {
        await EnsurePatientBelongsToTherapist(dto.PatientId, therapistId);

        var sessionCount = await _sessionRepository
            .CountAsync(s => s.PatientId == dto.PatientId);

        var session = new Session
        {
            Id = Guid.NewGuid(),
            PatientId = dto.PatientId,
            IntakeFormId = dto.IntakeFormId,
            SessionNumber = sessionCount + 1,
            SessionDate = dto.SessionDate,
            DurationMinutes = dto.DurationMinutes,
            SessionType = dto.SessionType,
            Status = "Draft",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _sessionRepository.AddAsync(session);
        await _unitOfWork.SaveChangesAsync();

        return MapToViewDto(session);
    }

    public async Task<SessionViewDto> UpdateAsync(SessionUpdateDto dto, Guid therapistId)
    {
        var session = await GetSessionWithOwnershipCheck(dto.Id, therapistId);

        if (dto.SessionDate.HasValue)
            session.SessionDate = dto.SessionDate.Value;
        if (dto.DurationMinutes.HasValue)
            session.DurationMinutes = dto.DurationMinutes.Value;
        if (dto.SessionType is not null)
            session.SessionType = dto.SessionType;
        if (dto.Status is not null)
            session.Status = dto.Status;

        session.UpdatedAt = DateTime.UtcNow;

        _sessionRepository.Update(session);
        await _unitOfWork.SaveChangesAsync();

        return MapToViewDto(session);
    }

    public async Task DeleteAsync(Guid id, Guid therapistId)
    {
        var session = await GetSessionWithOwnershipCheck(id, therapistId);

        _sessionRepository.Remove(session);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<SessionViewDto> GetByIdAsync(Guid id, Guid therapistId)
    {
        var session = await _sessionRepository.GetByIdWithNoteAsync(id)
            ?? throw new KeyNotFoundException($"Session with ID {id} not found.");

        await EnsurePatientBelongsToTherapist(session.PatientId, therapistId);

        return MapToViewDto(session);
    }

    public async Task<IEnumerable<SessionViewDto>> GetByPatientIdAsync(Guid patientId, Guid therapistId)
    {
        await EnsurePatientBelongsToTherapist(patientId, therapistId);

        var sessions = await _sessionRepository.GetByPatientIdWithNotesAsync(patientId);

        return sessions.Select(MapToViewDto);
    }

    public async Task<SessionNoteViewDto> SaveNoteAsync(Guid sessionId, SessionNoteDto dto, Guid therapistId)
    {
        var session = await _sessionRepository.GetByIdWithNoteAsync(sessionId)
            ?? throw new KeyNotFoundException($"Session with ID {sessionId} not found.");

        await EnsurePatientBelongsToTherapist(session.PatientId, therapistId);

        if (session.SessionNote is null)
        {
            session.SessionNote = new SessionNote
            {
                Id = Guid.NewGuid(),
                SessionId = sessionId,
                Observations = dto.Observations,
                Interventions = dto.Interventions,
                PatientResponse = dto.PatientResponse,
                HomeworkAssigned = dto.HomeworkAssigned,
                NextGoals = dto.NextGoals,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }
        else
        {
            session.SessionNote.Observations = dto.Observations;
            session.SessionNote.Interventions = dto.Interventions;
            session.SessionNote.PatientResponse = dto.PatientResponse;
            session.SessionNote.HomeworkAssigned = dto.HomeworkAssigned;
            session.SessionNote.NextGoals = dto.NextGoals;
            session.SessionNote.UpdatedAt = DateTime.UtcNow;
        }

        session.UpdatedAt = DateTime.UtcNow;
        _sessionRepository.Update(session);
        await _unitOfWork.SaveChangesAsync();

        return MapToNoteViewDto(session.SessionNote);
    }

    public async Task<SessionNoteViewDto?> GetNoteAsync(Guid sessionId, Guid therapistId)
    {
        var session = await _sessionRepository.GetByIdWithNoteAsync(sessionId)
            ?? throw new KeyNotFoundException($"Session with ID {sessionId} not found.");

        await EnsurePatientBelongsToTherapist(session.PatientId, therapistId);

        return session.SessionNote is null ? null : MapToNoteViewDto(session.SessionNote);
    }

    public async Task<VoiceMemoViewDto> SaveVoiceMemoAsync(Guid sessionId, string? transcript, Guid therapistId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId)
            ?? throw new KeyNotFoundException($"Session with ID {sessionId} not found.");

        await EnsurePatientBelongsToTherapist(session.PatientId, therapistId);

        var memo = new VoiceMemo
        {
            Id = Guid.NewGuid(),
            SessionId = sessionId,
            AudioUrl = null,
            Transcript = transcript,
            DurationSeconds = null,
            CreatedAt = DateTime.UtcNow
        };

        var memoRepo = _unitOfWork.Repository<VoiceMemo>();
        await memoRepo.AddAsync(memo);
        await _unitOfWork.SaveChangesAsync();

        return new VoiceMemoViewDto
        {
            Id = memo.Id,
            SessionId = memo.SessionId,
            Transcript = memo.Transcript,
            CreatedAt = memo.CreatedAt
        };
    }

    private async Task<Session> GetSessionWithOwnershipCheck(Guid sessionId, Guid therapistId)
    {
        var session = await _sessionRepository.GetByIdAsync(sessionId)
            ?? throw new KeyNotFoundException($"Session with ID {sessionId} not found.");

        await EnsurePatientBelongsToTherapist(session.PatientId, therapistId);

        return session;
    }

    private async Task EnsurePatientBelongsToTherapist(Guid patientId, Guid userId)
    {
        var therapist = await _unitOfWork.Repository<Therapist>()
            .FindSingleAsync(t => t.UserId == userId)
            ?? throw new UnauthorizedAccessException("Therapist profile not found.");

        var patient = await _patientRepository.GetByIdAsync(patientId)
            ?? throw new KeyNotFoundException($"Patient with ID {patientId} not found.");

        if (patient.TherapistId != therapist.Id)
            throw new UnauthorizedAccessException("You do not have access to this patient's data.");
    }

    private static SessionViewDto MapToViewDto(Session session) => new()
    {
        Id = session.Id,
        PatientId = session.PatientId,
        IntakeFormId = session.IntakeFormId,
        SessionNumber = session.SessionNumber,
        SessionDate = session.SessionDate,
        DurationMinutes = session.DurationMinutes,
        SessionType = session.SessionType,
        Status = session.Status,
        CreatedAt = session.CreatedAt,
        UpdatedAt = session.UpdatedAt,
        Note = session.SessionNote is null ? null : MapToNoteViewDto(session.SessionNote)
    };

    private static SessionNoteViewDto MapToNoteViewDto(SessionNote note) => new()
    {
        Id = note.Id,
        SessionId = note.SessionId,
        Observations = note.Observations,
        Interventions = note.Interventions,
        PatientResponse = note.PatientResponse,
        HomeworkAssigned = note.HomeworkAssigned,
        NextGoals = note.NextGoals,
        CreatedAt = note.CreatedAt,
        UpdatedAt = note.UpdatedAt
    };
}