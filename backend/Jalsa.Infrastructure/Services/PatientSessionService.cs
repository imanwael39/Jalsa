using Jalsa.Application.DTOs.PatientSession;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Session;
using Microsoft.EntityFrameworkCore;
using PatientEntity = Jalsa.Domain.Models.Patient.Patient;

namespace Jalsa.Infrastructure.Services;

public class PatientSessionService : IPatientSessionService
{
    private readonly IPatientRepository _patientRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PatientSessionService(
        IPatientRepository patientRepository,
        ISessionRepository sessionRepository,
        IUnitOfWork unitOfWork)
    {
        _patientRepository = patientRepository;
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PatientSessionSummaryDto>> GetListAsync(Guid userId)
    {
        var patient = await ResolvePatientAsync(userId);

        var sessions = await _sessionRepository.Query()
            .AsNoTracking()
            .Where(s => s.PatientId == patient.Id)
            .OrderByDescending(s => s.SessionDate)
            .ToListAsync();

        return sessions.Select(MapToSummaryDto).ToList();
    }

    public async Task<PatientSessionDetailDto> GetDetailAsync(Guid userId, Guid sessionId)
    {
        var patient = await ResolvePatientAsync(userId);
        var session = await GetOwnedSessionAsync(patient.Id, sessionId);

        var therapist = await _unitOfWork.Repository<Therapist>().FindSingleAsync(t => t.Id == patient.TherapistId);

        return new PatientSessionDetailDto
        {
            Id = session.Id,
            SessionNumber = session.SessionNumber,
            SessionDate = session.SessionDate,
            DurationMinutes = session.DurationMinutes,
            SessionType = session.SessionType,
            Status = session.Status,
            TherapistName = therapist?.FullName ?? "",
            PatientRequestType = session.PatientRequestType,
            PatientRequestNote = session.PatientRequestNote,
            PatientRequestStatus = session.PatientRequestStatus,
            PatientRequestedAt = session.PatientRequestedAt,
            CanRequestChange = CanRequestChange(session)
        };
    }

    public async Task RequestRescheduleAsync(Guid userId, Guid sessionId, SessionChangeRequestDto dto)
    {
        await SubmitRequestAsync(userId, sessionId, "Reschedule", dto);
    }

    public async Task RequestCancelAsync(Guid userId, Guid sessionId, SessionChangeRequestDto dto)
    {
        await SubmitRequestAsync(userId, sessionId, "Cancel", dto);
    }

    private async Task SubmitRequestAsync(Guid userId, Guid sessionId, string requestType, SessionChangeRequestDto dto)
    {
        var patient = await ResolvePatientAsync(userId);
        var session = await GetOwnedSessionAsync(patient.Id, sessionId);

        if (!CanRequestChange(session))
            throw new InvalidOperationException("لا يمكن تقديم طلب لهذه الجلسة في الوقت الحالي.");

        session.PatientRequestType = requestType;
        session.PatientRequestNote = dto.Note;
        session.PatientRequestStatus = "Pending";
        session.PatientRequestedAt = DateTime.UtcNow;
        session.UpdatedAt = DateTime.UtcNow;

        _sessionRepository.Update(session);
        await _unitOfWork.SaveChangesAsync();
    }

    private static bool CanRequestChange(Session session)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return session.Status == "Scheduled"
            && session.SessionDate >= today
            && session.PatientRequestStatus != "Pending";
    }

    private async Task<Session> GetOwnedSessionAsync(Guid patientId, Guid sessionId)
    {
        return await _sessionRepository.FindSingleAsync(s => s.Id == sessionId && s.PatientId == patientId)
            ?? throw new KeyNotFoundException("Session not found.");
    }

    private static PatientSessionSummaryDto MapToSummaryDto(Session session) => new()
    {
        Id = session.Id,
        SessionNumber = session.SessionNumber,
        SessionDate = session.SessionDate,
        DurationMinutes = session.DurationMinutes,
        SessionType = session.SessionType,
        Status = session.Status,
        PatientRequestType = session.PatientRequestType,
        PatientRequestStatus = session.PatientRequestStatus
    };

    private async Task<PatientEntity> ResolvePatientAsync(Guid userId)
    {
        return await _patientRepository.FindSingleAsync(p => p.UserId == userId)
            ?? throw new UnauthorizedAccessException("Patient profile not found.");
    }
}
