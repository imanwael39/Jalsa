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
    private readonly ISessionNoteEmbeddingCoordinator _embeddingCoordinator;

    public SessionService(
        ISessionRepository sessionRepository,
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork,
        ISessionNoteEmbeddingCoordinator embeddingCoordinator)
    {
        _sessionRepository = sessionRepository;
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
        _embeddingCoordinator = embeddingCoordinator;
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

        // Remove embeddings first so that the delete + vector-store sync are atomic
        // from the caller's perspective; if the embedding call throws, the DB write
        // is rolled back by SaveChangesAsync's enclosing transaction.
        await _embeddingCoordinator.RemoveForSessionAsync(session.Id);

        // Explicitly remove the SessionNote before the session to avoid FK violation
        // (SessionNotes.SessionId → Sessions.Id uses OnDelete(NoAction)).
        if (session.SessionNote is not null)
        {
            _unitOfWork.Repository<SessionNote>().Remove(session.SessionNote);
            session.SessionNote = null;
        }

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
            var note = new SessionNote
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

            // The Id is a client-assigned, non-default GUID, so EF Core's automatic
            // graph-fixup cannot tell this note apart from an existing tracked entity and
            // would mark it Modified instead of Added. Adding it explicitly avoids that.
            await _unitOfWork.Repository<SessionNote>().AddAsync(note);
            session.SessionNote = note;
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
        await _unitOfWork.SaveChangesAsync();

        // Sync embeddings AFTER SaveChangesAsync so the FK columns are committed
        // before the vector store writes a new row. The coordinator performs its
        // own DB round-trips; a failure here does not undo the note write —
        // the note is the source of truth and re-syncing on the next save will heal.
        try
        {
            await _embeddingCoordinator.SyncNoteAsync(session.SessionNote);
        }
        catch
        {
            // Embedding failures must never break a clinician's note save.
        }

        return MapToNoteViewDto(session.SessionNote);
    }

    public async Task<SessionNoteViewDto?> GetNoteAsync(Guid sessionId, Guid therapistId)
    {
        var session = await _sessionRepository.GetByIdWithNoteAsync(sessionId)
            ?? throw new KeyNotFoundException($"Session with ID {sessionId} not found.");

        await EnsurePatientBelongsToTherapist(session.PatientId, therapistId);

        return session.SessionNote is null ? null : MapToNoteViewDto(session.SessionNote);
    }

    public async Task<SessionViewDto> ApprovePatientRequestAsync(Guid sessionId, ApprovePatientRequestDto dto, Guid therapistId)
    {
        var session = await GetSessionWithOwnershipCheck(sessionId, therapistId);

        if (session.PatientRequestStatus != "Pending")
            throw new InvalidOperationException("لا يوجد طلب معلق لهذه الجلسة.");

        if (session.PatientRequestType == "Reschedule")
        {
            if (!dto.NewSessionDate.HasValue)
                throw new InvalidOperationException("يرجى تحديد الموعد الجديد للجلسة.");

            session.SessionDate = dto.NewSessionDate.Value;
        }
        else if (session.PatientRequestType == "Cancel")
        {
            session.Status = "Cancelled";
        }

        ClearPatientRequest(session);
        session.UpdatedAt = DateTime.UtcNow;

        _sessionRepository.Update(session);
        await _unitOfWork.SaveChangesAsync();

        return MapToViewDto(session);
    }

    public async Task<SessionViewDto> RejectPatientRequestAsync(Guid sessionId, Guid therapistId)
    {
        var session = await GetSessionWithOwnershipCheck(sessionId, therapistId);

        if (session.PatientRequestStatus != "Pending")
            throw new InvalidOperationException("لا يوجد طلب معلق لهذه الجلسة.");

        ClearPatientRequest(session);
        session.UpdatedAt = DateTime.UtcNow;

        _sessionRepository.Update(session);
        await _unitOfWork.SaveChangesAsync();

        return MapToViewDto(session);
    }

    private static void ClearPatientRequest(Session session)
    {
        session.PatientRequestType = null;
        session.PatientRequestNote = null;
        session.PatientRequestStatus = null;
        session.PatientRequestedAt = null;
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
        PatientRequestType = session.PatientRequestType,
        PatientRequestNote = session.PatientRequestNote,
        PatientRequestStatus = session.PatientRequestStatus,
        PatientRequestedAt = session.PatientRequestedAt,
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