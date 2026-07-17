using Jalsa.Application.DTOs.PatientSupportChat;

namespace Jalsa.Application.Interfaces.Services;

public interface IPatientSupportChatService
{
    /// <summary>Resolves (creating if needed) the caller's own open support conversation. Never accepts a patientId parameter.</summary>
    Task<PatientSupportConversationViewDto> GetOrCreateConversationAsync(Guid userId);
    Task<PatientSupportChatHistoryDto?> GetHistoryAsync(Guid userId, Guid conversationId);
    Task<PatientSupportMessageViewDto?> SendMessageAsync(Guid userId, Guid conversationId, string content, string senderType);
}
