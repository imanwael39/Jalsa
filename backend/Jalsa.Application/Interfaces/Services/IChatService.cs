using Jalsa.Application.DTOs.Chat;

namespace Jalsa.Application.Interfaces.Services;

public interface IChatService
{
    Task<ChatSessionViewDto> StartSessionAsync(Guid patientId, CancellationToken cancellationToken = default);
    Task<ChatSendResponseDto?> SendMessageAsync(Guid patientId, ChatSendMessageDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<ChatMessageViewDto>> GetHistoryAsync(Guid patientId, Guid? sessionId = null, CancellationToken cancellationToken = default);
    Task<bool> IsSessionOwnedByPatientAsync(Guid sessionId, Guid patientId, CancellationToken cancellationToken = default);
}
