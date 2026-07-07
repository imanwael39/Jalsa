using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.DTOs.Report;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Patient;
using Jalsa.Domain.Models.Report;
using Moq;

namespace Jalsa.Tests;

public class ReportServiceTests
{
    private readonly Mock<IReportRepository> _reportRepoMock;
    private readonly Mock<IPatientRepository> _patientRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Therapist>> _therapistRepoMock;
    private readonly Mock<IGenericRepository<ReportVersion>> _reportVersionRepoMock;
    private readonly ReportService _sut;

    private readonly Guid _therapistUserId = Guid.NewGuid();
    private readonly Guid _therapistProfileId = Guid.NewGuid();
    private readonly Guid _patientId = Guid.NewGuid();

    public ReportServiceTests()
    {
        _reportRepoMock = new Mock<IReportRepository>();
        _patientRepoMock = new Mock<IPatientRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _therapistRepoMock = new Mock<IGenericRepository<Therapist>>();
        _reportVersionRepoMock = new Mock<IGenericRepository<ReportVersion>>();

        _unitOfWorkMock
            .Setup(x => x.Repository<Therapist>())
            .Returns(_therapistRepoMock.Object);

        _unitOfWorkMock
            .Setup(x => x.Repository<ReportVersion>())
            .Returns(_reportVersionRepoMock.Object);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _sut = new ReportService(
            _reportRepoMock.Object,
            _patientRepoMock.Object,
            _unitOfWorkMock.Object);
    }

    private void SetupOwnershipCheck()
    {
        var therapist = new Therapist { Id = _therapistProfileId, UserId = _therapistUserId, FullName = "د. سارة", LicenseNumber = "LIC002", CreatedAt = DateTime.UtcNow };
        var patient = new Patient { Id = _patientId, TherapistId = _therapistProfileId, FullName = "مريض تجربة", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

        _therapistRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(therapist);

        _patientRepoMock
            .Setup(x => x.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);
    }

    private ReferralReport MakeReport(Guid? reportId = null)
    {
        var id = reportId ?? Guid.NewGuid();
        var versionId = Guid.NewGuid();
        var version = new ReportVersion
        {
            Id = versionId,
            ReportId = id,
            VersionNumber = 1,
            Content = "محتوى التقرير التجريبي",
            CreatedByTherapistId = _therapistUserId,
            CreatedAt = DateTime.UtcNow
        };
        return new ReferralReport
        {
            Id = id,
            PatientId = _patientId,
            TherapistId = _therapistUserId,
            GeneratedByTherapistId = _therapistUserId,
            Status = "Draft",
            CurrentVersionId = versionId,
            CurrentVersion = version,
            Versions = new List<ReportVersion> { version },
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task CreateWithAiContentAsync_ValidArgs_ReturnsReportViewDto()
    {
        // Arrange
        SetupOwnershipCheck();
        _reportRepoMock
            .Setup(x => x.AddAsync(It.IsAny<ReferralReport>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.CreateWithAiContentAsync(_patientId, "محتوى التقرير المولّد بالذكاء الاصطناعي", _therapistUserId);

        // Assert
        result.Should().NotBeNull();
        result.PatientId.Should().Be(_patientId);
        result.Status.Should().Be("Draft");
        result.CurrentVersion.Should().NotBeNull();
        result.CurrentVersion!.VersionNumber.Should().Be(1);
        result.CurrentVersion.Content.Should().Be("محتوى التقرير المولّد بالذكاء الاصطناعي");
        // TherapistId/GeneratedByTherapistId are FKs to Therapists.Id, not Users.Id — must be the
        // resolved Therapist profile id, never the raw auth user id passed into the service.
        result.TherapistId.Should().Be(_therapistProfileId);
        result.GeneratedByTherapistId.Should().Be(_therapistProfileId);
        result.CurrentVersion.CreatedByTherapistId.Should().Be(_therapistProfileId);
        // ReferralReport.CurrentVersionId (FK -> ReportVersion.Id) and ReportVersion.ReportId
        // (FK -> ReferralReport.Id) form a hard cycle when both rows are Added in the same
        // SaveChangesAsync — EF Core's topological sort throws "circular dependency detected".
        // The service must break the cycle by inserting CurrentVersionId as null in the first
        // save, then back-filling it via Update + second SaveChangesAsync.
        _reportRepoMock.Verify(x => x.AddAsync(It.IsAny<ReferralReport>(), It.IsAny<CancellationToken>()), Times.Once);
        _reportVersionRepoMock.Verify(x => x.AddAsync(It.IsAny<ReportVersion>(), It.IsAny<CancellationToken>()), Times.Once);
        _reportRepoMock.Verify(x => x.Update(It.Is<ReferralReport>(r => r.CurrentVersionId != null)), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Fact]
    public async Task CreateWithAiContentAsync_PatientNotOwned_ThrowsUnauthorized()
    {
        // Arrange
        var otherTherapistId = Guid.NewGuid();
        var therapist = new Therapist { Id = _therapistProfileId, UserId = _therapistUserId, FullName = "د. سارة", LicenseNumber = "LIC002", CreatedAt = DateTime.UtcNow };
        var patient = new Patient { Id = _patientId, TherapistId = otherTherapistId, FullName = "مريض", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

        _therapistRepoMock
            .Setup(x => x.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(therapist);
        _patientRepoMock
            .Setup(x => x.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(patient);

        // Act & Assert
        var act = () => _sut.CreateWithAiContentAsync(_patientId, "content", _therapistUserId);
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("You do not have access to this patient's data.");
    }

    [Fact]
    public async Task UpdateAsync_ExistingReport_AddsNewVersionAndReturnsDraft()
    {
        // Arrange
        SetupOwnershipCheck();
        var report = MakeReport();

        _reportRepoMock
            .Setup(x => x.GetByIdWithVersionsAsync(report.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(report);

        var dto = new ReportUpdateDto { Content = "محتوى محدث", ChangeNote = "تحديث بناءً على تغذية راجعة" };

        // Act
        var result = await _sut.UpdateAsync(report.Id, dto, _therapistUserId);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be("Draft");
        result.CurrentVersion!.VersionNumber.Should().Be(2);
        result.CurrentVersion.Content.Should().Be("محتوى محدث");
        // CreatedByTherapistId is a FK to Therapists.Id, not Users.Id — must be the resolved
        // Therapist profile id, never the raw auth user id passed into the service.
        result.CurrentVersion.CreatedByTherapistId.Should().Be(_therapistProfileId);
        result.CurrentVersion.CreatedByTherapistId.Should().NotBe(_therapistUserId);
        // The new version's Id is a client-assigned, non-default GUID, so EF Core's automatic
        // graph-fixup can't distinguish it from an existing entity and would mark it Modified
        // instead of Added. It must be added explicitly instead of relying on Update(report).
        _reportVersionRepoMock.Verify(x => x.AddAsync(It.IsAny<ReportVersion>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ApproveAsync_DraftReport_SetsStatusApproved()
    {
        // Arrange
        SetupOwnershipCheck();
        var report = MakeReport();

        _reportRepoMock
            .Setup(x => x.GetByIdWithVersionsAsync(report.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(report);

        // Act
        var result = await _sut.ApproveAsync(report.Id, _therapistUserId);

        // Assert
        result.Status.Should().Be("Approved");
        report.CurrentVersion!.ApprovedAt.Should().NotBeNull();
        _reportRepoMock.Verify(x => x.Update(report), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ExistingReport_CallsRemoveAndSave()
    {
        // Arrange
        SetupOwnershipCheck();
        var report = MakeReport();

        _reportRepoMock
            .Setup(x => x.GetByIdWithVersionsAsync(report.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(report);

        // Act
        await _sut.DeleteAsync(report.Id, _therapistUserId);

        // Assert
        _reportRepoMock.Verify(x => x.Remove(report), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ThrowsKeyNotFound()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        _reportRepoMock
            .Setup(x => x.GetByIdWithVersionsAsync(reportId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ReferralReport?)null);

        // Act & Assert
        var act = () => _sut.GetByIdAsync(reportId, _therapistUserId);
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task GetByPatientIdAsync_ReturnsReportList()
    {
        // Arrange
        SetupOwnershipCheck();
        var reports = new List<ReferralReport> { MakeReport(), MakeReport() };

        _reportRepoMock
            .Setup(x => x.GetByPatientIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(reports);

        // Act
        var result = await _sut.GetByPatientIdAsync(_patientId, _therapistUserId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(r => r.PatientId == _patientId);
    }
}