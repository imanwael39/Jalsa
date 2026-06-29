namespace Jalsa.Application.DTOs.Chat;

public record SendMessageDto(Guid ConversationId, string Content);
