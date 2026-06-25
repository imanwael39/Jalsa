using AutoMapper;
using Jalsa.Application.DTOs.Exercise;
using Jalsa.Domain.Models.Exercise;

namespace Jalsa.Application.Mappings;

public class ExerciseMappingProfile : Profile
{
    public ExerciseMappingProfile()
    {
        CreateMap<Exercise, PatientExerciseViewDto>();
        CreateMap<ExerciseLog, ExerciseLogViewDto>();
    }
}
