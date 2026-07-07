using Jalsa.Application.DTOs.Admin;

namespace Jalsa.Application.Interfaces.Services;

/// <summary>
/// Admin-facing view of patient ACCOUNTS only (login/enable/disable state) — never
/// clinical data. Sessions, exercises, notes, and reports stay behind PatientService,
/// which this service does not call.
/// </summary>
public interface IPatientAccountAdminService
{
    Task<PagedResultDto<PatientAccountAdminViewDto>> GetPatientAccountsAsync(PatientAccountFilterDto filter);

    Task<PatientAccountAdminViewDto?> DisableAccountAsync(Guid patientId, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null);

    Task<PatientAccountAdminViewDto?> RestoreAccountAsync(Guid patientId, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null);

    Task<PatientAccountAdminViewDto?> DeleteAccountAsync(Guid patientId, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null);
}
