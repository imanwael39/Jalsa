using Jalsa.Application.DTOs.Chat;

namespace Jalsa.API.Services.Interfaces;

public interface IChatMonitoringService
{
    Task<List<ChatEngagementSummaryDto>> GetEngagementSummariesAsync(DateTime? from = null, DateTime? to = null, Guid? patientId = null);
}
