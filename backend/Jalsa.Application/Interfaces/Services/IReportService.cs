using Jalsa.Application.DTOs.Report;

namespace Jalsa.Application.Interfaces.Services;

public interface IReportService
{
    Task<ReportViewDto> CreateWithAiContentAsync(Guid patientId, string aiContent, Guid therapistId);
    Task<ReportViewDto> UpdateAsync(Guid id, ReportUpdateDto dto, Guid therapistId);
    Task<ReportViewDto> ApproveAsync(Guid id, Guid therapistId);
    Task<ReportViewDto> RejectAsync(Guid id, Guid therapistId);
    Task DeleteAsync(Guid id, Guid therapistId);
    Task<ReportViewDto> GetByIdAsync(Guid id, Guid therapistId);
    Task<IEnumerable<ReportViewDto>> GetByPatientIdAsync(Guid patientId, Guid therapistId);
}
