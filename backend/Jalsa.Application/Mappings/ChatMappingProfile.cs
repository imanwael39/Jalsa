using AutoMapper;
using Jalsa.Application.DTOs.Chat;
using Jalsa.Application.DTOs.Crisis;
using Jalsa.Domain.Models.Chat;
using Jalsa.Domain.Models.Crisis;

namespace Jalsa.Application.Mappings;

public class ChatMappingProfile : Profile
{
    public ChatMappingProfile()
    {
        CreateMap<ChatConversation, ChatSessionViewDto>()
            .ForMember(dest => dest.StartedAt, opt => opt.MapFrom(src => src.CreatedAt));

        CreateMap<ChatMessage, ChatMessageViewDto>();

        CreateMap<CrisisAlert, CrisisAlertViewDto>();
    }
}
