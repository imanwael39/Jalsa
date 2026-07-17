namespace Jalsa.Application.DTOs.PatientSupportChat;

public record SendPatientSupportMessageDto(Guid ConversationId, string Content);
