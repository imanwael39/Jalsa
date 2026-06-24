using Jalsa.API.DTOs.Patient;
using Jalsa.API.Exceptions;
using Jalsa.API.Services.Interfaces;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Domain.Models.Patient;
using Jalsa.Domain.Models.Clinic;

namespace Jalsa.API.Services.Implementations;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;
    public PatientService(IUnitOfWork unitOfWork)
    {
        _unitOfWork=unitOfWork;
    }

        public async Task<PatientResponseDTO> CreateAsync(CreatePatientDTO dto, Guid currentUserId)
    {
        var therapistId = await ResolveTherapistIdAsync(currentUserId);

        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            TherapistId = therapistId,
            FullName = dto.FullName,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            ReferralSource = dto.ReferralSource,
            ChiefComplaint = dto.ChiefComplaint,
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Patient>().AddAsync(patient);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(patient);
    }

        public async Task<PatientResponseDTO> GetByIdAsync(Guid id, Guid currentUserId)
    {
        var therapistId = await ResolveTherapistIdAsync(currentUserId);
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);

        if (patient == null || patient.TherapistId != therapistId)
            throw new ApiException(404, "Patient not found");

        return MapToDto(patient);
    }

        public async Task<IEnumerable<PatientResponseDTO>> GetAllAsync(Guid currentUserId, PatientFilterDto? filter = null)
    {
        var therapistId = await ResolveTherapistIdAsync(currentUserId);
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
        public async Task<PatientResponseDTO> UpdateAsync(Guid id, UpdatePatientDto dto, Guid currentUserId)
    {
        var therapistId = await ResolveTherapistIdAsync(currentUserId);
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);

        if (patient == null || patient.TherapistId != therapistId)
            throw new ApiException(404, "Patient not found");

        patient.FullName = dto.FullName;
        patient.DateOfBirth = dto.DateOfBirth;
        patient.Gender = dto.Gender;
        patient.Phone = dto.Phone;
        patient.Email = dto.Email;
        patient.Address = dto.Address;
        patient.ReferralSource = dto.ReferralSource;
        patient.ChiefComplaint = dto.ChiefComplaint;
        patient.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<Patient>().Update(patient);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(patient);
    }
        public async Task ArchiveAsync(Guid id, Guid currentUserId)
    {
        var therapistId = await ResolveTherapistIdAsync(currentUserId);
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);

        if (patient == null || patient.TherapistId != therapistId)
            throw new ApiException(404, "Patient not found");

        patient.Status = "Archived";
        patient.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<Patient>().Update(patient);
        await _unitOfWork.SaveChangesAsync();
    }
        public async Task RestoreAsync(Guid id, Guid currentUserId)
    {
        var therapistId = await ResolveTherapistIdAsync(currentUserId);
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);

        if (patient == null || patient.TherapistId != therapistId)
            throw new ApiException(404, "Patient not found");

        patient.Status = "Active";
        patient.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<Patient>().Update(patient);
        await _unitOfWork.SaveChangesAsync();
    }
        public async Task DeleteAsync(Guid id, Guid currentUserId)
    {
        var therapistId = await ResolveTherapistIdAsync(currentUserId);
        var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(id);

        if (patient == null || patient.TherapistId != therapistId)
            throw new ApiException(404, "Patient not found");

        _unitOfWork.Repository<Patient>().Remove(patient);
        await _unitOfWork.SaveChangesAsync();
    }
        private async Task<Guid> ResolveTherapistIdAsync(Guid userId)
    {
        var therapist = await _unitOfWork.Repository<Therapist>()
            .FindSingleAsync(t => t.UserId == userId);

        if (therapist == null)
            throw new ApiException(403, "Authenticated user is not a therapist");

        return therapist.Id;
    }
        private static PatientResponseDTO MapToDto(Patient patient) => new()
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
        Status = patient.Status,
        CreatedAt = patient.CreatedAt,
        UpdatedAt = patient.UpdatedAt
    };
}
