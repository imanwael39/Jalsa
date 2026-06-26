using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Jalsa.Application.DTOs.Chat;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Infrastructure.Data;
using Jalsa.Domain.Models.Chat;

namespace Jalsa.API.Services.Implementations;

public class ChatService : IChatService
{
    private readonly Galsa_DBDbContext _context;
    private readonly IMapper _mapper;

    public ChatService(Galsa_DBDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ChatSessionViewDto> StartSessionAsync(Guid patientId, CancellationToken cancellationToken = default)
    {
        var conversation = new ChatConversation
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            Status = "Open",
            LastActivityAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.ChatConversations.Add(conversation);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ChatSessionViewDto>(conversation);
    }

    public async Task<ChatSendResponseDto?> SendMessageAsync(Guid patientId, ChatSendMessageDto dto, CancellationToken cancellationToken = default)
    {
        var conversation = await _context.ChatConversations
            .FirstOrDefaultAsync(c => c.Id == dto.SessionId && c.PatientId == patientId, cancellationToken);

        if (conversation is null) return null;

        var patientMessage = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = dto.SessionId,
            SenderType = "Patient",
            Content = dto.Message,
            CreatedAt = DateTime.UtcNow
        };

        _context.ChatMessages.Add(patientMessage);

        var aiResponseContent = GenerateAiResponse(dto.Message);

        var aiMessage = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ConversationId = dto.SessionId,
            SenderType = "AI",
            Content = aiResponseContent,
            CreatedAt = DateTime.UtcNow
        };

        _context.ChatMessages.Add(aiMessage);

        conversation.LastActivityAt = DateTime.UtcNow;
        conversation.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new ChatSendResponseDto
        {
            PatientMessage = _mapper.Map<ChatMessageViewDto>(patientMessage),
            AiMessage = _mapper.Map<ChatMessageViewDto>(aiMessage)
        };
    }

    public async Task<IEnumerable<ChatMessageViewDto>> GetHistoryAsync(Guid patientId, Guid? sessionId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.ChatMessages
            .Include(m => m.Conversation)
            .Where(m => m.Conversation.PatientId == patientId);

        if (sessionId.HasValue)
            query = query.Where(m => m.ConversationId == sessionId.Value);

        var messages = await query
            .OrderBy(m => m.CreatedAt)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ChatMessageViewDto>>(messages);
    }

    public async Task<bool> IsSessionOwnedByPatientAsync(Guid sessionId, Guid patientId, CancellationToken cancellationToken = default)
    {
        return await _context.ChatConversations
            .AnyAsync(c => c.Id == sessionId && c.PatientId == patientId, cancellationToken);
    }

    private string GenerateAiResponse(string patientMessage)
    {
        var lowerMessage = patientMessage.ToLowerInvariant();

        if (lowerMessage.Contains("suicide") || lowerMessage.Contains("kill myself") || lowerMessage.Contains("end my life") || lowerMessage.Contains("don't want to live"))
        {
            return "I'm concerned about what you're sharing. Your safety is the most important thing. Please reach out to the 988 Suicide and Crisis Lifeline by calling or texting 988 right now. You can also text HOME to 741741. Your therapist will also be notified. You are not alone, and help is available.";
        }

        if (lowerMessage.Contains("harm myself") || lowerMessage.Contains("self-harm") || lowerMessage.Contains("cutting"))
        {
            return "I hear you, and I want you to know that what you're feeling matters. Self-harm is a serious concern. Please contact the Crisis Text Line by texting HOME to 741741. I'm also letting your therapist know so they can support you. You deserve help and support.";
        }

        if (lowerMessage.Contains("anxious") || lowerMessage.Contains("anxiety") || lowerMessage.Contains("worried") || lowerMessage.Contains("panic"))
        {
            return "Thank you for sharing that with me. Anxiety can feel overwhelming, but you're taking a positive step by talking about it. Some things that might help right now: try slow, deep breaths (inhale for 4 counts, hold for 4, exhale for 4). Ground yourself by naming 5 things you can see, 4 you can hear, 3 you can touch. Would you like to discuss some coping strategies with your therapist?";
        }

        if (lowerMessage.Contains("sad") || lowerMessage.Contains("depressed") || lowerMessage.Contains("hopeless") || lowerMessage.Contains("lonely"))
        {
            return "I appreciate you opening up about how you're feeling. Those emotions are valid, and it takes courage to express them. Remember that feelings are temporary, even when they feel permanent. You're not alone in this. Would you like me to help you connect with your therapist to discuss these feelings further?";
        }

        if (lowerMessage.Contains("exercise") || lowerMessage.Contains("homework") || lowerMessage.Contains("assignment"))
        {
            return "It's great that you're thinking about your exercises! Sticking to your assigned activities is an important part of your progress. If you're having trouble with any specific exercise, I can help you break it down into smaller steps, or we can discuss it with your therapist. What would be most helpful?";
        }

        return "Thank you for sharing. I'm here to support you between your therapy sessions. Your message has been received, and your therapist can review our conversation. Is there anything specific you'd like to discuss or any concerns you'd like to share?";
    }
}
