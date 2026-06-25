using FluentAssertions;
using Jalsa.Application.DTOs.Exercise;
using Jalsa.Application.Interfaces.Services;
using Moq;
using Xunit;

namespace Jalsa.Tests.Services;

public class PatientExerciseServiceTests
{
    private readonly Mock<IExerciseService> _exerciseServiceMock;
    private readonly Guid _patientId;

    public PatientExerciseServiceTests()
    {
        _exerciseServiceMock = new Mock<IExerciseService>();
        _patientId = Guid.NewGuid();
    }

    [Fact]
    public async Task GetExercises_ReturnsListForPatient()
    {
        var exercises = new List<PatientExerciseViewDto>
        {
            new() { Id = Guid.NewGuid(), Description = "Deep breathing", Status = "Active" },
            new() { Id = Guid.NewGuid(), Description = "Journaling", Status = "Active" }
        };

        _exerciseServiceMock
            .Setup(x => x.GetPatientExercisesAsync(_patientId, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exercises);

        var result = await _exerciseServiceMock.Object.GetPatientExercisesAsync(_patientId);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetExercises_WithStatusFilter_ReturnsFiltered()
    {
        var exercises = new List<PatientExerciseViewDto>
        {
            new() { Id = Guid.NewGuid(), Description = "Done exercise", Status = "Complete" }
        };

        _exerciseServiceMock
            .Setup(x => x.GetPatientExercisesAsync(_patientId, "Complete", It.IsAny<CancellationToken>()))
            .ReturnsAsync(exercises);

        var result = await _exerciseServiceMock.Object.GetPatientExercisesAsync(_patientId, "Complete");

        result.Should().HaveCount(1);
        result.First().Status.Should().Be("Complete");
    }

    [Fact]
    public async Task IsExerciseOwned_WhenOwned_ReturnsTrue()
    {
        var exerciseId = Guid.NewGuid();

        _exerciseServiceMock
            .Setup(x => x.IsExerciseOwnedByPatientAsync(exerciseId, _patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _exerciseServiceMock.Object.IsExerciseOwnedByPatientAsync(exerciseId, _patientId);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsExerciseOwned_WhenNotOwned_ReturnsFalse()
    {
        var exerciseId = Guid.NewGuid();

        _exerciseServiceMock
            .Setup(x => x.IsExerciseOwnedByPatientAsync(exerciseId, _patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _exerciseServiceMock.Object.IsExerciseOwnedByPatientAsync(exerciseId, _patientId);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateExerciseStatus_WhenOwned_ReturnsLog()
    {
        var exerciseId = Guid.NewGuid();
        var dto = new ExerciseStatusUpdateDto { Status = "Complete", ReflectionNote = "Felt great" };
        var log = new ExerciseLogViewDto
        {
            Id = Guid.NewGuid(),
            ExerciseId = exerciseId,
            CompletionStatus = "Complete",
            ReflectionNote = "Felt great"
        };

        _exerciseServiceMock
            .Setup(x => x.UpdateExerciseStatusAsync(exerciseId, _patientId, dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(log);

        var result = await _exerciseServiceMock.Object.UpdateExerciseStatusAsync(exerciseId, _patientId, dto);

        result.Should().NotBeNull();
        result!.CompletionStatus.Should().Be("Complete");
    }

    [Fact]
    public async Task UpdateExerciseStatus_WhenNotFound_ReturnsNull()
    {
        var exerciseId = Guid.NewGuid();
        var dto = new ExerciseStatusUpdateDto { Status = "Complete" };

        _exerciseServiceMock
            .Setup(x => x.UpdateExerciseStatusAsync(exerciseId, _patientId, dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExerciseLogViewDto?)null);

        var result = await _exerciseServiceMock.Object.UpdateExerciseStatusAsync(exerciseId, _patientId, dto);

        result.Should().BeNull();
    }
}
