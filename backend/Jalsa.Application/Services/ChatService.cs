using Jalsa.Application.DTOs.Chat;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Chat;

namespace Jalsa.Application.Services;

public class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ConversationViewDto>> GetConversationsAsync(Guid? patientId)
    {
        var repo = _unitOfWork.Repository<ChatConversation>();
        var conversations = patientId.HasValue
            ? await repo.FindAsync(c => c.PatientId == patientId.Value)
            : await repo.GetAllAsync();

        return conversations
            .OrderByDescending(c => c.LastActivityAt ?? c.CreatedAt)
            .Select(c => new ConversationViewDto
            {
                Id = c.Id,
                PatientId = c.PatientId,
                PatientName = c.Patient?.FullName ?? string.Empty,
                Status = c.Status,
                LastActivityAt = c.LastActivityAt,
                CreatedAt = c.CreatedAt,
                MessageCount = c.ChatMessages?.Count ?? 0
            });
    }

    public async Task<ChatHistoryDto?> GetHistoryAsync(Guid conversationId)
    {
        var repo = _unitOfWork.Repository<ChatConversation>();
        var conversation = await repo.FindSingleAsync(c => c.Id == conversationId);

        if (conversation is null)
            return null;

        var messageRepo = _unitOfWork.Repository<ChatMessage>();
        var messages = await messageRepo.FindAsync(m => m.ConversationId == conversationId);

        return new ChatHistoryDto
        {
            ConversationId = conversation.Id,
            PatientId = conversation.PatientId,
            PatientName = conversation.Patient?.FullName ?? string.Empty,
            Messages = messages
                .OrderBy(m => m.CreatedAt)
                .Select(m => new ChatMessageViewDto
                {
                    Id = m.Id,
                    ConversationId = m.ConversationId,
                    SenderType = m.SenderType,
                    Content = m.Content,
                    CreatedAt = m.CreatedAt
                }).ToList()
        };
    }

    public async Task<ConversationViewDto> CreateConversationAsync(Guid patientId)
    {
        var patientRepo = _unitOfWork.Repository<Domain.Models.Patient.Patient>();
        var patientExists = await patientRepo.AnyAsync(p => p.Id == patientId);
        if (!patientExists)
            throw new KeyNotFoundException("المريض غير موجود");

        var repo = _unitOfWork.Repository<ChatConversation>();
        var existing = await repo.FindSingleAsync(c => c.PatientId == patientId && c.Status == "Open");

        if (existing is not null)
        {
            return new ConversationViewDto
            {
                Id = existing.Id,
                PatientId = existing.PatientId,
                PatientName = existing.Patient?.FullName ?? string.Empty,
                Status = existing.Status,
                LastActivityAt = existing.LastActivityAt,
                CreatedAt = existing.CreatedAt,
                MessageCount = existing.ChatMessages?.Count ?? 0
            };
        }

        var conversation = new ChatConversation
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

        return new ConversationViewDto
        {
            Id = conversation.Id,
            PatientId = conversation.PatientId,
            Status = conversation.Status,
            CreatedAt = conversation.CreatedAt,
            LastActivityAt = conversation.LastActivityAt
        };
    }

    public async Task<ConversationViewDto?> CloseConversationAsync(Guid conversationId)
    {
        var repo = _unitOfWork.Repository<ChatConversation>();
        var conversation = await repo.GetByIdAsync(conversationId);

        if (conversation is null)
            return null;

        conversation.Status = "Closed";
        conversation.UpdatedAt = DateTime.UtcNow;
        repo.Update(conversation);
        await _unitOfWork.SaveChangesAsync();

        return new ConversationViewDto
        {
            Id = conversation.Id,
            PatientId = conversation.PatientId,
            Status = conversation.Status,
            CreatedAt = conversation.CreatedAt,
            LastActivityAt = conversation.LastActivityAt
        };
    }

    public async Task<ChatMessageViewDto?> SendMessageAsync(Guid conversationId, string content, string senderType)
    {
        var repo = _unitOfWork.Repository<ChatConversation>();
        var conversation = await repo.GetByIdAsync(conversationId);

        if (conversation is null)
            return null;

        var message = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderType = senderType,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        var messageRepo = _unitOfWork.Repository<ChatMessage>();
        await messageRepo.AddAsync(message);
        conversation.LastActivityAt = DateTime.UtcNow;
        conversation.UpdatedAt = DateTime.UtcNow;
        repo.Update(conversation);
        await _unitOfWork.SaveChangesAsync();

        return new ChatMessageViewDto
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderType = message.SenderType,
            Content = message.Content,
            CreatedAt = message.CreatedAt
        };
    }
}
