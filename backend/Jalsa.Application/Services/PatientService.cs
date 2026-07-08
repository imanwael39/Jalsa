using System.Text.RegularExpressions;
using Jalsa.Application.DTOs.Patient;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Identity;
using Jalsa.Domain.Models.Patient;

namespace Jalsa.Application.Services;

public class PatientService : IPatientService
{
    private static readonly Regex PasswordComplexityRegex =
        new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", RegexOptions.Compiled);

    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PatientViewDto> CreateAsync(PatientCreateDto dto, Guid userId)
    {
        var therapistId = await ResolveTherapistIdAsync(userId);

        Guid? linkedUserId = null;

        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new InvalidOperationException("كلمة المرور مطلوبة عند إدخال بريد إلكتروني للمريض.");

            if (!PasswordComplexityRegex.IsMatch(dto.Password))
                throw new InvalidOperationException("كلمة المرور يجب أن تحتوي على 8 أحرف على الأقل وحرف كبير وحرف صغير ورقم.");

            var userRepo = _unitOfWork.Repository<User>();
            var existingUser = await userRepo.FindSingleAsync(u => u.Email == dto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("البريد الإلكتروني مستخدم بالفعل لحساب آخر.");

            var patientRole = await _unitOfWork.Repository<Role>().FindSingleAsync(r => r.Name == "Patient")
                ?? throw new InvalidOperationException("دور 'Patient' غير معرف في النظام.");

            var patientUser = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            patientUser.UserRoles.Add(new UserRole
            {
                UserId = patientUser.Id,
                RoleId = patientRole.Id,
                CreatedAt = DateTime.UtcNow,
            });

            await userRepo.AddAsync(patientUser);
            linkedUserId = patientUser.Id;
        }

        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            TherapistId = therapistId,
            UserId = linkedUserId,
            FullName = dto.FullName,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            ReferralSource = dto.ReferralSource,
            ChiefComplaint = dto.ChiefComplaint,
            EmergencyContactName = dto.EmergencyContactName,
            EmergencyContactRelationship = dto.EmergencyContactRelationship,
            EmergencyContactPhone = dto.EmergencyContactPhone,
            TreatmentStartDate = dto.TreatmentStartDate,
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Patient>().AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(patient);
    }

    public async Task<PatientViewDto> GetByIdAsync(Guid id, Guid userId)
    {
        var therapistId = await ResolveTherapistIdAsync(userId);
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);

        if (patient == null || patient.TherapistId != therapistId)
            throw new KeyNotFoundException("Patient not found");

        return MapToDto(patient);
    }

    public async Task<IEnumerable<PatientViewDto>> GetAllAsync(Guid userId, PatientFilterDto? filter = null)
    {
        var therapistId = await ResolveTherapistIdAsync(userId);
        var query = _unitOfWork.Repository<Patient>().Query()
            .Where(p => p.TherapistId == therapistId);

        if (filter != null)
        {
            if (!string.IsNullOrWhiteSpace(filter.Status))
                query = query.Where(p => p.Status == filter.Status);

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.ToLower();
                query = query.Where(p =>
                    p.FullName.ToLower().Contains(term) ||
                    (p.Phone != null && p.Phone.Contains(term)) ||
                    (p.Email != null && p.Email.ToLower().Contains(term)));
            }

            if (filter.Page > 0 && filter.PageSize > 0)
                query = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize);
        }

        var patients = query.OrderByDescending(p => p.CreatedAt).ToList();
        return patients.Select(MapToDto);
    }

    public async Task<PatientViewDto> UpdateAsync(Guid id, PatientUpdateDto dto, Guid userId)
    {
        var therapistId = await ResolveTherapistIdAsync(userId);
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);

        if (patient == null || patient.TherapistId != therapistId)
            throw new KeyNotFoundException("Patient not found");

        patient.FullName = dto.FullName;
        patient.DateOfBirth = dto.DateOfBirth;
        patient.Gender = dto.Gender;
        patient.Phone = dto.Phone;
        patient.Email = dto.Email;
        patient.Address = dto.Address;
        patient.ReferralSource = dto.ReferralSource;
        patient.ChiefComplaint = dto.ChiefComplaint;
        patient.EmergencyContactName = dto.EmergencyContactName;
        patient.EmergencyContactRelationship = dto.EmergencyContactRelationship;
        patient.EmergencyContactPhone = dto.EmergencyContactPhone;
        patient.TreatmentStartDate = dto.TreatmentStartDate;
        patient.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<Patient>().Update(patient);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(patient);
    }

    public async Task ArchiveAsync(Guid id, Guid userId)
    {
        var therapistId = await ResolveTherapistIdAsync(userId);
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);

        if (patient == null || patient.TherapistId != therapistId)
            throw new KeyNotFoundException("Patient not found");

        patient.Status = "Archived";
        patient.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<Patient>().Update(patient);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RestoreAsync(Guid id, Guid userId)
    {
        var therapistId = await ResolveTherapistIdAsync(userId);
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);

        if (patient == null || patient.TherapistId != therapistId)
            throw new KeyNotFoundException("Patient not found");

        patient.Status = "Active";
        patient.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<Patient>().Update(patient);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var therapistId = await ResolveTherapistIdAsync(userId);
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);

        if (patient == null || patient.TherapistId != therapistId)
            throw new KeyNotFoundException("Patient not found");

        _unitOfWork.Repository<Patient>().Remove(patient);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<Guid> ResolveTherapistIdAsync(Guid userId)
    {
        var therapist = await _unitOfWork.Repository<Therapist>()
            .FindSingleAsync(t => t.UserId == userId)
            ?? throw new UnauthorizedAccessException("Therapist profile not found.");

        return therapist.Id;
    }

    private static PatientViewDto MapToDto(Patient patient) => new()
    {
        Id = patient.Id,
        TherapistId = patient.TherapistId,
        ClinicId = patient.ClinicId,
        UserId = patient.UserId,
        FullName = patient.FullName,
        DateOfBirth = patient.DateOfBirth,
        Gender = patient.Gender,
        Phone = patient.Phone,
        Email = patient.Email,
        Address = patient.Address,
        ReferralSource = patient.ReferralSource,
        ChiefComplaint = patient.ChiefComplaint,
        EmergencyContactName = patient.EmergencyContactName,
        EmergencyContactRelationship = patient.EmergencyContactRelationship,
        EmergencyContactPhone = patient.EmergencyContactPhone,
        TreatmentStartDate = patient.TreatmentStartDate,
        Status = patient.Status,
        CreatedAt = patient.CreatedAt,
        UpdatedAt = patient.UpdatedAt
    };
}
