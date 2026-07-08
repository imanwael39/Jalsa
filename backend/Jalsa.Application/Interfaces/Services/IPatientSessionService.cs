using Jalsa.Application.DTOs.PatientSession;

namespace Jalsa.Application.Interfaces.Services;

public interface IPatientSessionService
{
    Task<List<PatientSessionSummaryDto>> GetListAsync(Guid userId);
    Task<PatientSessionDetailDto> GetDetailAsync(Guid userId, Guid sessionId);
    Task RequestRescheduleAsync(Guid userId, Guid sessionId, SessionChangeRequestDto dto);
    Task RequestCancelAsync(Guid userId, Guid sessionId, SessionChangeRequestDto dto);
}
