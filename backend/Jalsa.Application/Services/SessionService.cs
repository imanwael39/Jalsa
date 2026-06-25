using AutoMapper;
using Jalsa.Application.DTOs.Session;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Session;

namespace Jalsa.Application.Services;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IGenericRepository<SessionNote> _sessionNoteRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SessionService(
        ISessionRepository sessionRepository,
        IGenericRepository<SessionNote> sessionNoteRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _sessionRepository = sessionRepository;
        _sessionNoteRepository = sessionNoteRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SessionViewDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetWithDetailsAsync(id, cancellationToken);
        if (session is null) return null;

        return _mapper.Map<SessionViewDto>(session);
    }

    public async Task<PagedResult<SessionViewDto>> GetPagedAsync(SessionSearchDto search, CancellationToken cancellationToken = default)
    {
        var query = _sessionRepository.Query();

        if (search.PatientId.HasValue)
            query = query.Where(s => s.PatientId == search.PatientId.Value);

        if (search.DateFrom.HasValue)
            query = query.Where(s => s.SessionDate >= search.DateFrom.Value);

        if (search.DateTo.HasValue)
            query = query.Where(s => s.SessionDate <= search.DateTo.Value);

        if (!string.IsNullOrWhiteSpace(search.Status))
            query = query.Where(s => s.Status == search.Status);

        var totalCount = query.Count();
        var items = query
            .OrderByDescending(s => s.SessionDate)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .ToList();

        var sessionIds = items.Select(s => s.Id).ToList();
        var sessionsWithDetails = await _sessionRepository.GetAllWithDetailsAsync(cancellationToken);
        var filteredSessions = sessionsWithDetails.Where(s => sessionIds.Contains(s.Id)).ToList();

        return new PagedResult<SessionViewDto>
        {
            Items = _mapper.Map<IEnumerable<SessionViewDto>>(filteredSessions),
            TotalCount = totalCount,
            Page = search.Page,
            PageSize = search.PageSize
        };
    }

    public async Task<SessionViewDto> CreateAsync(SessionCreateDto dto, CancellationToken cancellationToken = default)
    {
        var nextNumber = await _sessionRepository.GetNextSessionNumberAsync(dto.PatientId, cancellationToken);

        var session = new Domain.Models.Session.Session
        {
            Id = Guid.NewGuid(),
            PatientId = dto.PatientId,
            SessionNumber = nextNumber,
            SessionDate = dto.SessionDate,
            DurationMinutes = dto.DurationMinutes,
            SessionType = dto.SessionType,
            Status = dto.Status ?? "Draft",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _sessionRepository.AddAsync(session, cancellationToken);

        if (dto.SessionNote is not null)
        {
            var sessionNote = new SessionNote
            {
                Id = Guid.NewGuid(),
                SessionId = session.Id,
                Observations = dto.SessionNote.Observations,
                Interventions = dto.SessionNote.Interventions,
                PatientResponse = dto.SessionNote.PatientResponse,
                HomeworkAssigned = dto.SessionNote.HomeworkAssigned,
                NextGoals = dto.SessionNote.NextGoals,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _sessionNoteRepository.AddAsync(sessionNote, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var created = await _sessionRepository.GetWithDetailsAsync(session.Id, cancellationToken);
        return _mapper.Map<SessionViewDto>(created!);
    }

    public async Task<SessionViewDto?> UpdateAsync(Guid id, SessionUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetWithDetailsAsync(id, cancellationToken);
        if (session is null) return null;

        session.SessionDate = dto.SessionDate;
        session.DurationMinutes = dto.DurationMinutes;
        session.SessionType = dto.SessionType;
        session.Status = dto.Status ?? session.Status;
        session.UpdatedAt = DateTime.UtcNow;

        _sessionRepository.Update(session);

        if (dto.SessionNote is not null)
        {
            if (session.SessionNote is not null)
            {
                session.SessionNote.Observations = dto.SessionNote.Observations;
                session.SessionNote.Interventions = dto.SessionNote.Interventions;
                session.SessionNote.PatientResponse = dto.SessionNote.PatientResponse;
                session.SessionNote.HomeworkAssigned = dto.SessionNote.HomeworkAssigned;
                session.SessionNote.NextGoals = dto.SessionNote.NextGoals;
                session.SessionNote.UpdatedAt = DateTime.UtcNow;
                _sessionNoteRepository.Update(session.SessionNote);
            }
            else
            {
                var sessionNote = new SessionNote
                {
                    Id = Guid.NewGuid(),
                    SessionId = session.Id,
                    Observations = dto.SessionNote.Observations,
                    Interventions = dto.SessionNote.Interventions,
                    PatientResponse = dto.SessionNote.PatientResponse,
                    HomeworkAssigned = dto.SessionNote.HomeworkAssigned,
                    NextGoals = dto.SessionNote.NextGoals,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _sessionNoteRepository.AddAsync(sessionNote, cancellationToken);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updated = await _sessionRepository.GetWithDetailsAsync(id, cancellationToken);
        return _mapper.Map<SessionViewDto>(updated);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var session = await _sessionRepository.GetWithDetailsAsync(id, cancellationToken);
        if (session is null) return false;

        if (session.SessionNote is not null)
        {
            _sessionNoteRepository.Remove(session.SessionNote);
        }

        _sessionRepository.Remove(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<SessionViewDto>> GetByPatientIdAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        var sessions = await _sessionRepository.GetByPatientIdAsync(patientId, cancellationToken);
        return _mapper.Map<IEnumerable<SessionViewDto>>(sessions);
    }
}
