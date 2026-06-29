using Jalsa.Application.DTOs.Chat;

namespace Jalsa.Application.Interfaces.Services;

public interface IChatService
{
    Task<IEnumerable<ConversationViewDto>> GetConversationsAsync(Guid? patientId);
    Task<ChatHistoryDto?> GetHistoryAsync(Guid conversationId);
    Task<ConversationViewDto> CreateConversationAsync(Guid patientId);
    Task<ConversationViewDto?> CloseConversationAsync(Guid conversationId);
    Task<ChatMessageViewDto?> SendMessageAsync(Guid conversationId, string content, string senderType);
}
