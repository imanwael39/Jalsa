using Jalsa.Application.DTOs.Report;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Report;

namespace Jalsa.Application.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(
        IReportRepository reportRepository,
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork)
    {
        _reportRepository = reportRepository;
        _patientRepository = patientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReportViewDto> CreateWithAiContentAsync(Guid patientId, string aiContent, Guid therapistId)
    {
        await EnsurePatientBelongsToTherapist(patientId, therapistId);

        var reportId = Guid.NewGuid();
        var versionId = Guid.NewGuid();

        var version = new ReportVersion
        {
            Id = versionId,
            ReportId = reportId,
            VersionNumber = 1,
            Content = aiContent,
            CreatedByTherapistId = therapistId,
            CreatedAt = DateTime.UtcNow
        };

        var report = new ReferralReport
        {
            Id = reportId,
            PatientId = patientId,
            TherapistId = therapistId,
            GeneratedByTherapistId = therapistId,
            Status = "Draft",
            CurrentVersionId = versionId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Versions = new List<ReportVersion> { version }
        };

        await _reportRepository.AddAsync(report);
        await _unitOfWork.SaveChangesAsync();

        report.CurrentVersion = version;
        return MapToViewDto(report);
    }

    public async Task<ReportViewDto> UpdateAsync(Guid id, ReportUpdateDto dto, Guid therapistId)
    {
        var report = await GetReportWithOwnershipCheck(id, therapistId);

        var nextVersion = (report.Versions?.Count ?? 0) + 1;
        var version = new ReportVersion
        {
            Id = Guid.NewGuid(),
            ReportId = id,
            VersionNumber = nextVersion,
            Content = dto.Content,
            ChangeNote = dto.ChangeNote,
            CreatedByTherapistId = therapistId,
            CreatedAt = DateTime.UtcNow
        };

        report.Versions ??= new List<ReportVersion>();
        report.Versions.Add(version);
        report.CurrentVersionId = version.Id;
        report.CurrentVersion = version;
        report.Status = "Draft";
        report.UpdatedAt = DateTime.UtcNow;

        _reportRepository.Update(report);
        await _unitOfWork.SaveChangesAsync();

        return MapToViewDto(report);
    }

    public async Task<ReportViewDto> ApproveAsync(Guid id, Guid therapistId)
    {
        var report = await GetReportWithOwnershipCheck(id, therapistId);

        if (report.CurrentVersion is not null)
            report.CurrentVersion.ApprovedAt = DateTime.UtcNow;

        report.Status = "Approved";
        report.UpdatedAt = DateTime.UtcNow;

        _reportRepository.Update(report);
        await _unitOfWork.SaveChangesAsync();

        return MapToViewDto(report);
    }

    public async Task DeleteAsync(Guid id, Guid therapistId)
    {
        var report = await GetReportWithOwnershipCheck(id, therapistId);
        _reportRepository.Remove(report);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ReportViewDto> GetByIdAsync(Guid id, Guid therapistId)
    {
        var report = await _reportRepository.GetByIdWithVersionsAsync(id)
            ?? throw new KeyNotFoundException($"Report with ID {id} not found.");

        await EnsurePatientBelongsToTherapist(report.PatientId, therapistId);

        return MapToViewDto(report);
    }

    public async Task<IEnumerable<ReportViewDto>> GetByPatientIdAsync(Guid patientId, Guid therapistId)
    {
        await EnsurePatientBelongsToTherapist(patientId, therapistId);

        var reports = await _reportRepository.GetByPatientIdAsync(patientId);

        return reports.Select(MapToViewDto);
    }

    private async Task<ReferralReport> GetReportWithOwnershipCheck(Guid reportId, Guid therapistId)
    {
        var report = await _reportRepository.GetByIdWithVersionsAsync(reportId)
            ?? throw new KeyNotFoundException($"Report with ID {reportId} not found.");

        await EnsurePatientBelongsToTherapist(report.PatientId, therapistId);

        return report;
    }

    private async Task EnsurePatientBelongsToTherapist(Guid patientId, Guid therapistId)
    {
        var patient = await _patientRepository.GetByIdAsync(patientId)
            ?? throw new KeyNotFoundException($"Patient with ID {patientId} not found.");

        if (patient.TherapistId != therapistId)
            throw new UnauthorizedAccessException("You do not have access to this patient's data.");
    }

    private static ReportViewDto MapToViewDto(ReferralReport report) => new()
    {
        Id = report.Id,
        PatientId = report.PatientId,
        TherapistId = report.TherapistId,
        GeneratedByTherapistId = report.GeneratedByTherapistId,
        Status = report.Status,
        CurrentVersionId = report.CurrentVersionId,
        CreatedAt = report.CreatedAt,
        UpdatedAt = report.UpdatedAt,
        CurrentVersion = report.CurrentVersion is null ? null : MapToVersionViewDto(report.CurrentVersion),
        Versions = report.Versions?.OrderByDescending(v => v.VersionNumber)
            .Select(MapToVersionViewDto).ToList() ?? new()
    };

    private static ReportVersionViewDto MapToVersionViewDto(ReportVersion version) => new()
    {
        Id = version.Id,
        ReportId = version.ReportId,
        VersionNumber = version.VersionNumber,
        Content = version.Content,
        CreatedByTherapistId = version.CreatedByTherapistId,
        ApprovedAt = version.ApprovedAt,
        ChangeNote = version.ChangeNote,
        CreatedAt = version.CreatedAt
    };
}
