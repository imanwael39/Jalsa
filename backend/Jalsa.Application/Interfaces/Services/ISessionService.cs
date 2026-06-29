using Jalsa.Application.DTOs.Session;

namespace Jalsa.Application.Interfaces.Services;

public interface ISessionService
{
    Task<SessionViewDto> CreateAsync(SessionCreateDto dto, Guid therapistId);
    Task<SessionViewDto> UpdateAsync(SessionUpdateDto dto, Guid therapistId);
    Task DeleteAsync(Guid id, Guid therapistId);
    Task<SessionViewDto> GetByIdAsync(Guid id, Guid therapistId);
    Task<IEnumerable<SessionViewDto>> GetByPatientIdAsync(Guid patientId, Guid therapistId);
    Task<SessionNoteViewDto> SaveNoteAsync(Guid sessionId, SessionNoteDto dto, Guid therapistId);
    Task<SessionNoteViewDto?> GetNoteAsync(Guid sessionId, Guid therapistId);
    Task<VoiceMemoViewDto> SaveVoiceMemoAsync(Guid sessionId, string? transcript, Guid therapistId);
}
