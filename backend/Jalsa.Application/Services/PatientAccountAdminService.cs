using System.Linq.Expressions;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Identity;
using Jalsa.Domain.Models.Patient;

namespace Jalsa.Application.Services;

public class PatientAccountAdminService : IPatientAccountAdminService
{
    // Account-level fields ONLY — deliberately excludes MedicalHistory, ChiefComplaint,
    // EmergencyContact and every clinical navigation (Sessions/Assessments/Exercises/Reports).
    // Admin manages accounts, never treatment data.
    private static readonly Expression<Func<Patient, PatientAccountAdminViewDto>> ToDto = p => new PatientAccountAdminViewDto
    {
        Id = p.Id,
        UserId = p.UserId,
        FullName = p.FullName,
        Email = p.Email,
        TherapistName = p.Therapist.FullName,
        Status = p.Status,
        IsActive = p.User != null ? p.User.IsActive : (bool?)null,
        IsDeleted = p.User != null ? p.User.IsDeleted : (bool?)null,
        CreatedAt = p.CreatedAt,
        LastLoginAt = p.User != null ? p.User.LastLoginAt : null
    };

    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogService _auditLogService;

    public PatientAccountAdminService(IUnitOfWork unitOfWork, IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _auditLogService = auditLogService;
    }

    public Task<PagedResultDto<PatientAccountAdminViewDto>> GetPatientAccountsAsync(PatientAccountFilterDto filter)
    {
        var query = _unitOfWork.Repository<Patient>().Query();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var term = filter.SearchTerm.ToLower();
            query = query.Where(p =>
                p.FullName.ToLower().Contains(term) ||
                (p.Email != null && p.Email.ToLower().Contains(term)));
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
            query = query.Where(p => p.Status == filter.Status);

        var totalCount = query.Count();

        var page = filter.Page > 0 ? filter.Page : 1;
        var pageSize = filter.PageSize > 0 ? filter.PageSize : 20;

        var items = query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ToDto)
            .ToList();

        return Task.FromResult(new PagedResultDto<PatientAccountAdminViewDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<PatientAccountAdminViewDto?> DisableAccountAsync(Guid patientId, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null)
    {
        var (patient, user) = await FindPatientAndUserAsync(patientId);
        if (patient is null)
            return null;
        if (user is null)
            throw new InvalidOperationException("لا يوجد حساب دخول مرتبط بهذا المريض لتعطيله.");

        var wasActive = user.IsActive;
        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync(
            actingAdminUserId, "Patient", patientId.ToString(), "Patient.DisableAccount",
            oldValues: new { IsActive = wasActive }, newValues: new { IsActive = false },
            ipAddress: ipAddress, userAgent: userAgent);

        return await GetDtoAsync(patientId);
    }

    public async Task<PatientAccountAdminViewDto?> RestoreAccountAsync(Guid patientId, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null)
    {
        var (patient, user) = await FindPatientAndUserAsync(patientId);
        if (patient is null)
            return null;
        if (user is null)
            throw new InvalidOperationException("لا يوجد حساب دخول مرتبط بهذا المريض لاستعادته.");

        user.IsActive = true;
        user.IsDeleted = false;
        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync(
            actingAdminUserId, "Patient", patientId.ToString(), "Patient.RestoreAccount",
            ipAddress: ipAddress, userAgent: userAgent);

        return await GetDtoAsync(patientId);
    }

    public async Task<PatientAccountAdminViewDto?> DeleteAccountAsync(Guid patientId, Guid? actingAdminUserId = null, string? ipAddress = null, string? userAgent = null)
    {
        var (patient, user) = await FindPatientAndUserAsync(patientId);
        if (patient is null)
            return null;
        if (user is null)
            throw new InvalidOperationException("لا يوجد حساب دخول مرتبط بهذا المريض لحذفه.");

        user.IsDeleted = true;
        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Repository<User>().Update(user);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogAsync(
            actingAdminUserId, "Patient", patientId.ToString(), "Patient.DeleteAccount",
            ipAddress: ipAddress, userAgent: userAgent);

        return await GetDtoAsync(patientId);
    }

    private async Task<(Patient? Patient, User? User)> FindPatientAndUserAsync(Guid patientId)
    {
        var patient = await _unitOfWork.Repository<Patient>().FindSingleAsync(p => p.Id == patientId);
        if (patient is null)
            return (null, null);

        if (patient.UserId is null)
            return (patient, null);

        var user = await _unitOfWork.Repository<User>().FindSingleAsync(u => u.Id == patient.UserId.Value);
        return (patient, user);
    }

    private Task<PatientAccountAdminViewDto?> GetDtoAsync(Guid patientId)
    {
        var dto = _unitOfWork.Repository<Patient>().Query()
            .Where(p => p.Id == patientId)
            .Select(ToDto)
            .FirstOrDefault();

        return Task.FromResult(dto);
    }
}
