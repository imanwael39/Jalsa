using Jalsa.Application.DTOs.TherapistChat;

namespace Jalsa.Application.Interfaces.Services;

public interface ITherapistAiChatService
{
    Task<IEnumerable<TherapistConversationViewDto>> GetConversationsAsync(Guid userId, Guid? patientId);
    Task<TherapistChatHistoryDto?> GetHistoryAsync(Guid userId, Guid conversationId);
    Task<TherapistConversationViewDto> CreateConversationAsync(Guid userId, Guid patientId);
    Task<TherapistConversationViewDto?> CloseConversationAsync(Guid userId, Guid conversationId);
    Task<TherapistChatMessageViewDto?> SendMessageAsync(Guid userId, Guid conversationId, string content, string senderType);
}
