using Jalsa.Application.DTOs.PatientSupportChat;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Chat;

namespace Jalsa.Application.Services;

/// <summary>
/// Patient-only support chat. Every method resolves the caller's own Patient record via
/// Patient.UserId == CurrentUserId — no method on this service accepts a patientId
/// parameter from the caller, which eliminates any IDOR path into another patient's
/// conversation. A therapist token can never reach this service (controller is
/// [Authorize(Roles = "Patient")]).
/// </summary>
public class PatientSupportChatService : IPatientSupportChatService
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientSupportChatService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PatientSupportConversationViewDto> GetOrCreateConversationAsync(Guid userId)
    {
        var patientId = await ResolvePatientIdAsync(userId);
        var repo = _unitOfWork.Repository<PatientSupportConversation>();
        var existing = await repo.FindSingleAsync(c => c.PatientId == patientId && c.Status == "Open");

        if (existing is not null)
        {
            return new PatientSupportConversationViewDto
            {
                Id = existing.Id,
                Status = existing.Status,
                LastActivityAt = existing.LastActivityAt,
                CreatedAt = existing.CreatedAt,
                MessageCount = existing.Messages?.Count ?? 0
            };
        }

        var conversation = new PatientSupportConversation
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            Status = "Open",
            LastActivityAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(conversation);
        await _unitOfWork.SaveChangesAsync();

        return new PatientSupportConversationViewDto
        {
            Id = conversation.Id,
            Status = conversation.Status,
            CreatedAt = conversation.CreatedAt,
            LastActivityAt = conversation.LastActivityAt
        };
    }

    public async Task<PatientSupportChatHistoryDto?> GetHistoryAsync(Guid userId, Guid conversationId)
    {
        var patientId = await ResolvePatientIdAsync(userId);
        var repo = _unitOfWork.Repository<PatientSupportConversation>();
        var conversation = await repo.FindSingleAsync(c => c.Id == conversationId && c.PatientId == patientId);

        if (conversation is null)
            return null;

        var messageRepo = _unitOfWork.Repository<PatientSupportMessage>();
        var messages = await messageRepo.FindAsync(m => m.ConversationId == conversationId);

        return new PatientSupportChatHistoryDto
        {
            ConversationId = conversation.Id,
            Status = conversation.Status,
            Messages = messages
                .OrderBy(m => m.CreatedAt)
                .Select(m => new PatientSupportMessageViewDto
                {
                    Id = m.Id,
                    ConversationId = m.ConversationId,
                    SenderType = m.SenderType,
                    Content = m.Content,
                    CreatedAt = m.CreatedAt
                }).ToList()
        };
    }

    public async Task<PatientSupportMessageViewDto?> SendMessageAsync(Guid userId, Guid conversationId, string content, string senderType)
    {
        var patientId = await ResolvePatientIdAsync(userId);
        var repo = _unitOfWork.Repository<PatientSupportConversation>();
        var conversation = await repo.FindSingleAsync(c => c.Id == conversationId && c.PatientId == patientId);

        if (conversation is null)
            return null;

        var message = new PatientSupportMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderType = senderType,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        var messageRepo = _unitOfWork.Repository<PatientSupportMessage>();
        await messageRepo.AddAsync(message);
        conversation.LastActivityAt = DateTime.UtcNow;
        conversation.UpdatedAt = DateTime.UtcNow;
        repo.Update(conversation);
        await _unitOfWork.SaveChangesAsync();

        return new PatientSupportMessageViewDto
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderType = message.SenderType,
            Content = message.Content,
            CreatedAt = message.CreatedAt
        };
    }

    private async Task<Guid> ResolvePatientIdAsync(Guid userId)
    {
        var patient = await _unitOfWork.Repository<Domain.Models.Patient.Patient>().FindSingleAsync(p => p.UserId == userId)
            ?? throw new UnauthorizedAccessException("Patient profile not found.");

        return patient.Id;
    }
}
