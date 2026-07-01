using Jalsa.Application.DTOs.Chat;

namespace Jalsa.Application.Interfaces.Services;

public interface IChatService
{
    Task<IEnumerable<ConversationViewDto>> GetConversationsAsync(Guid userId, Guid? patientId);
    Task<ChatHistoryDto?> GetHistoryAsync(Guid userId, Guid conversationId);
    Task<ConversationViewDto> CreateConversationAsync(Guid userId, Guid patientId);
    Task<ConversationViewDto?> CloseConversationAsync(Guid userId, Guid conversationId);
    Task<ChatMessageViewDto?> SendMessageAsync(Guid userId, Guid conversationId, string content, string senderType);
}
