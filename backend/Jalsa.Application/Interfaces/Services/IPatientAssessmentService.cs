using Jalsa.Application.DTOs.PatientAssessment;

namespace Jalsa.Application.Interfaces.Services;

public interface IPatientAssessmentService
{
    Task<List<PatientAssessmentSummaryDto>> GetListAsync(Guid userId);
    Task<PatientAssessmentDetailDto> GetDetailAsync(Guid userId, Guid assessmentId);
    Task SaveAnswerAsync(Guid userId, Guid assessmentId, Guid questionId, SaveAnswerRequestDto dto);
    Task<PatientAssessmentDetailDto> SubmitAsync(Guid userId, Guid assessmentId);
}
