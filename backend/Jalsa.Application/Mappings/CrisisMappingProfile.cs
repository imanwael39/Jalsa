using AutoMapper;
using Jalsa.Application.DTOs.Crisis;
using Jalsa.Domain.Models.Crisis;

namespace Jalsa.Application.Mappings;

public class CrisisMappingProfile : Profile
{
    public CrisisMappingProfile()
    {
        CreateMap<CrisisAlert, CrisisAlertViewDto>()
            .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient.FullName))
            .ForMember(d => d.MessageSnippet, o => o.MapFrom(s => s.ChatMessage != null ? s.ChatMessage.Content : null));
    }
}
