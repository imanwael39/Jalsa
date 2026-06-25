using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.DTOs.Dashboard;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Crisis;
using Jalsa.Domain.Models.Exercise;
using Jalsa.Domain.Models.Patient;
using Jalsa.Domain.Models.Session;
using Jalsa.Infrastructure.Services;
using Moq;

namespace Jalsa.Tests;

public class ProgressServiceTests
{
    private readonly Mock<IPatientRepository> _patientRepoMock;
    private readonly Mock<ISessionRepository> _sessionRepoMock;
    private readonly Mock<IExerciseRepository> _exerciseRepoMock;
    private readonly Mock<IAssessmentRepository> _assessmentRepoMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<CrisisAlert>> _crisisAlertRepoMock;
    private readonly ProgressService _sut;

    public ProgressServiceTests()
    {
        _patientRepoMock = new Mock<IPatientRepository>();
        _sessionRepoMock = new Mock<ISessionRepository>();
        _exerciseRepoMock = new Mock<IExerciseRepository>();
        _assessmentRepoMock = new Mock<IAssessmentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _crisisAlertRepoMock = new Mock<IGenericRepository<CrisisAlert>>();

        _unitOfWorkMock
            .Setup(x => x.Repository<CrisisAlert>())
            .Returns(_crisisAlertRepoMock.Object);

        SetupEmptyQueryables();

        _sut = new ProgressService(
            _patientRepoMock.Object,
            _sessionRepoMock.Object,
            _exerciseRepoMock.Object,
            _assessmentRepoMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetDashboardAsync_ReturnsCorrectPatientCounts()
    {
        // Arrange
        var patients = new List<Patient>
        {
            new() { Id = Guid.NewGuid(), Status = "Active" },
            new() { Id = Guid.NewGuid(), Status = "Active" },
            new() { Id = Guid.NewGuid(), Status = "Archived" }
        };

        _patientRepoMock
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns((Expression<Func<Patient, bool>> pred, CancellationToken _) =>
                Task.FromResult(patients.AsQueryable().Count(pred)));

        // Act
        var result = await _sut.GetDashboardAsync();

        // Assert
        result.TotalPatients.Should().Be(3);
        result.ActivePatients.Should().Be(2);
        result.ArchivedPatients.Should().Be(1);
    }

    [Fact]
    public async Task GetDashboardAsync_ExerciseCompletionRate_CalculatesCorrectly()
    {
        // Arrange
        var exercises = new List<Exercise>
        {
            new() { Id = Guid.NewGuid(), Status = "Complete" },
            new() { Id = Guid.NewGuid(), Status = "Complete" },
            new() { Id = Guid.NewGuid(), Status = "Partial" },
            new() { Id = Guid.NewGuid(), Status = "Skipped" }
        };

        _exerciseRepoMock
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Exercise, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns((Expression<Func<Exercise, bool>> pred, CancellationToken _) =>
                Task.FromResult(exercises.AsQueryable().Count(pred)));

        _exerciseRepoMock
            .Setup(x => x.Query())
            .Returns(new AsyncQueryProvider<Exercise>(exercises.AsQueryable()));

        // Act
        var result = await _sut.GetDashboardAsync();

        // Assert
        result.ExerciseCompletionRate.Should().Be(50);
        result.Analytics.ExerciseCompletion.Complete.Should().Be(2);
        result.Analytics.ExerciseCompletion.Partial.Should().Be(1);
        result.Analytics.ExerciseCompletion.Skipped.Should().Be(1);
    }

    [Fact]
    public async Task GetDashboardAsync_EmptyDatabase_ReturnsZeroValues()
    {
        // Arrange — all mocks return defaults (empty / zero)

        // Act
        var result = await _sut.GetDashboardAsync();

        // Assert
        result.TotalPatients.Should().Be(0);
        result.ActivePatients.Should().Be(0);
        result.ArchivedPatients.Should().Be(0);
        result.TotalSessions.Should().Be(0);
        result.SessionsThisMonth.Should().Be(0);
        result.ExerciseCompletionRate.Should().Be(0);
        result.AverageAssessmentScore.Should().Be(0);
        result.RecentAlerts.Should().BeEmpty();
        result.Analytics.AssessmentTrend.Should().NotBeNull();
        result.Analytics.ExerciseCompletion.Should().NotBeNull();
        result.Analytics.SessionFrequency.Should().NotBeNull();
    }

    private void SetupEmptyQueryables()
    {
        _sessionRepoMock
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Session, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _sessionRepoMock
            .Setup(x => x.Query())
            .Returns(new AsyncQueryProvider<Session>(new List<Session>().AsQueryable()));

        _exerciseRepoMock
            .Setup(x => x.CountAsync(It.IsAny<Expression<Func<Exercise, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);

        _exerciseRepoMock
            .Setup(x => x.Query())
            .Returns(new AsyncQueryProvider<Exercise>(new List<Exercise>().AsQueryable()));

        _assessmentRepoMock
            .Setup(x => x.Query())
            .Returns(new AsyncQueryProvider<Jalsa.Domain.Models.Assessment.Assessment>(
                new List<Jalsa.Domain.Models.Assessment.Assessment>().AsQueryable()));

        _crisisAlertRepoMock
            .Setup(x => x.Query())
            .Returns(new AsyncQueryProvider<CrisisAlert>(new List<CrisisAlert>().AsQueryable()));
    }
}
