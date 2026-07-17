using Jalsa.Application.DTOs.TherapistChat;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Chat;
using Jalsa.Domain.Models.Clinic;

namespace Jalsa.Application.Services;

/// <summary>
/// Therapist-only clinical assistant chat. Every query is scoped to
/// Patient.TherapistId == CurrentTherapistId — there is no code path here that a patient
/// can reach, and no fallback to Patient.UserId. A patient can never read, write, or
/// otherwise resolve a TherapistAiConversation.
/// </summary>
public class TherapistAiChatService : ITherapistAiChatService
{
    private readonly IUnitOfWork _unitOfWork;

    public TherapistAiChatService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TherapistConversationViewDto>> GetConversationsAsync(Guid userId, Guid? patientId)
    {
        var therapistId = await ResolveTherapistIdAsync(userId);
        var repo = _unitOfWork.Repository<TherapistAiConversation>();
        var conversations = patientId.HasValue
            ? await repo.FindAsync(c => c.PatientId == patientId.Value && c.TherapistId == therapistId)
            : await repo.FindAsync(c => c.TherapistId == therapistId);

        return conversations
            .OrderByDescending(c => c.LastActivityAt ?? c.CreatedAt)
            .Select(c => new TherapistConversationViewDto
            {
                Id = c.Id,
                PatientId = c.PatientId,
                PatientName = c.Patient?.FullName ?? string.Empty,
                Status = c.Status,
                LastActivityAt = c.LastActivityAt,
                CreatedAt = c.CreatedAt,
                MessageCount = c.Messages?.Count ?? 0
            });
    }

    public async Task<TherapistChatHistoryDto?> GetHistoryAsync(Guid userId, Guid conversationId)
    {
        var therapistId = await ResolveTherapistIdAsync(userId);
        var repo = _unitOfWork.Repository<TherapistAiConversation>();
        var conversation = await repo.FindSingleAsync(c => c.Id == conversationId && c.TherapistId == therapistId);

        if (conversation is null)
            return null;

        var messageRepo = _unitOfWork.Repository<TherapistAiMessage>();
        var messages = await messageRepo.FindAsync(m => m.ConversationId == conversationId);

        return new TherapistChatHistoryDto
        {
            ConversationId = conversation.Id,
            PatientId = conversation.PatientId,
            PatientName = conversation.Patient?.FullName ?? string.Empty,
            Messages = messages
                .OrderBy(m => m.CreatedAt)
                .Select(m => new TherapistChatMessageViewDto
                {
                    Id = m.Id,
                    ConversationId = m.ConversationId,
                    SenderType = m.SenderType,
                    Content = m.Content,
                    CreatedAt = m.CreatedAt
                }).ToList()
        };
    }

    public async Task<TherapistConversationViewDto> CreateConversationAsync(Guid userId, Guid patientId)
    {
        var therapistId = await ResolveTherapistIdAsync(userId);
        var patientRepo = _unitOfWork.Repository<Domain.Models.Patient.Patient>();
        var patient = await patientRepo.FindSingleAsync(p => p.Id == patientId && p.TherapistId == therapistId);
        if (patient is null)
            throw new KeyNotFoundException("المريض غير موجود");

        var repo = _unitOfWork.Repository<TherapistAiConversation>();
        var existing = await repo.FindSingleAsync(c => c.PatientId == patientId && c.TherapistId == therapistId && c.Status == "Open");

        if (existing is not null)
        {
            return new TherapistConversationViewDto
            {
                Id = existing.Id,
                PatientId = existing.PatientId,
                PatientName = existing.Patient?.FullName ?? string.Empty,
                Status = existing.Status,
                LastActivityAt = existing.LastActivityAt,
                CreatedAt = existing.CreatedAt,
                MessageCount = existing.Messages?.Count ?? 0
            };
        }

        var conversation = new TherapistAiConversation
        {
            Id = Guid.NewGuid(),
            TherapistId = therapistId,
            PatientId = patientId,
            Status = "Open",
            LastActivityAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(conversation);
        await _unitOfWork.SaveChangesAsync();

        return new TherapistConversationViewDto
        {
            Id = conversation.Id,
            PatientId = conversation.PatientId,
            Status = conversation.Status,
            CreatedAt = conversation.CreatedAt,
            LastActivityAt = conversation.LastActivityAt
        };
    }

    public async Task<TherapistConversationViewDto?> CloseConversationAsync(Guid userId, Guid conversationId)
    {
        var therapistId = await ResolveTherapistIdAsync(userId);
        var repo = _unitOfWork.Repository<TherapistAiConversation>();
        var conversation = await repo.FindSingleAsync(c => c.Id == conversationId && c.TherapistId == therapistId);

        if (conversation is null)
            return null;

        conversation.Status = "Closed";
        conversation.UpdatedAt = DateTime.UtcNow;
        repo.Update(conversation);
        await _unitOfWork.SaveChangesAsync();

        return new TherapistConversationViewDto
        {
            Id = conversation.Id,
            PatientId = conversation.PatientId,
            Status = conversation.Status,
            CreatedAt = conversation.CreatedAt,
            LastActivityAt = conversation.LastActivityAt
        };
    }

    public async Task<TherapistChatMessageViewDto?> SendMessageAsync(Guid userId, Guid conversationId, string content, string senderType)
    {
        var therapistId = await ResolveTherapistIdAsync(userId);
        var repo = _unitOfWork.Repository<TherapistAiConversation>();
        var conversation = await repo.FindSingleAsync(c => c.Id == conversationId && c.TherapistId == therapistId);

        if (conversation is null)
            return null;

        var message = new TherapistAiMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderType = senderType,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        var messageRepo = _unitOfWork.Repository<TherapistAiMessage>();
        await messageRepo.AddAsync(message);
        conversation.LastActivityAt = DateTime.UtcNow;
        conversation.UpdatedAt = DateTime.UtcNow;
        repo.Update(conversation);
        await _unitOfWork.SaveChangesAsync();

        return new TherapistChatMessageViewDto
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderType = message.SenderType,
            Content = message.Content,
            CreatedAt = message.CreatedAt
        };
    }

    private async Task<Guid> ResolveTherapistIdAsync(Guid userId)
    {
        var therapist = await _unitOfWork.Repository<Therapist>().FindSingleAsync(t => t.UserId == userId)
            ?? throw new UnauthorizedAccessException("Therapist profile not found.");

        return therapist.Id;
    }
}
