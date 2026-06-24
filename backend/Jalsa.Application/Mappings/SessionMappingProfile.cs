using AutoMapper;
using Jalsa.Application.DTOs.Session;
using Jalsa.Domain.Models.Session;

namespace Jalsa.Application.Mappings;

public class SessionMappingProfile : Profile
{
    public SessionMappingProfile()
    {
        CreateMap<Session, SessionViewDto>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : null))
            .ForMember(dest => dest.SessionNote, opt => opt.MapFrom(src => src.SessionNote));

        CreateMap<SessionNote, SessionNoteViewDto>();
    }
}
