using Jalsa.Application.DTOs.PatientProgress;

namespace Jalsa.Application.Interfaces.Services;

public interface IPatientProgressService
{
    Task<PatientProgressDto> GetProgressAsync(Guid userId, DateOnly? fromDate, DateOnly? toDate);
}
