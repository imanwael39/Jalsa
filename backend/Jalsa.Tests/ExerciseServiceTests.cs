using FluentAssertions;
using Jalsa.Application.DTOs.Exercise;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Exercise;
using Moq;

namespace Jalsa.Tests;

public class ExerciseServiceTests
{
    private readonly Mock<IExerciseRepository> _exerciseRepositoryMock;
    private readonly Mock<IExerciseLogRepository> _exerciseLogRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly ExerciseService _sut;

    public ExerciseServiceTests()
    {
        _exerciseRepositoryMock = new Mock<IExerciseRepository>();
        _exerciseLogRepositoryMock = new Mock<IExerciseLogRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _sut = new ExerciseService(
            _exerciseRepositoryMock.Object,
            _exerciseLogRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsExerciseViewDto()
    {
        // Arrange
        var dto = new ExerciseCreateDto
        {
            PatientId = Guid.NewGuid(),
            Description = "Test Exercise",
            Frequency = "Daily",
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
            DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7))
        };

        _exerciseRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Exercise>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _sut.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.PatientId.Should().Be(dto.PatientId);
        result.Description.Should().Be(dto.Description);
        result.Frequency.Should().Be(dto.Frequency);
        _exerciseRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Exercise>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingId_UpdatesDescription()
    {
        // Arrange
        var exerciseId = Guid.NewGuid();
        var existingExercise = new Exercise
        {
            Id = exerciseId,
            PatientId = Guid.NewGuid(),
            Description = "Old Description",
            Frequency = "Weekly",
            Status = "Active",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var dto = new ExerciseUpdateDto
        {
            Id = exerciseId,
            Description = "New Description"
        };

        _exerciseRepositoryMock
            .Setup(x => x.GetByIdAsync(exerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingExercise);

        _exerciseRepositoryMock
            .Setup(x => x.Update(It.IsAny<Exercise>()));

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _sut.UpdateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Description.Should().Be("New Description");
        result.Frequency.Should().Be("Weekly");
        _exerciseRepositoryMock.Verify(x => x.Update(It.IsAny<Exercise>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_CallsSaveChanges()
    {
        // Arrange
        var exerciseId = Guid.NewGuid();
        var existingExercise = new Exercise
        {
            Id = exerciseId,
            PatientId = Guid.NewGuid(),
            Description = "Exercise to delete",
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _exerciseRepositoryMock
            .Setup(x => x.GetByIdAsync(exerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingExercise);

        _exerciseRepositoryMock
            .Setup(x => x.Remove(It.IsAny<Exercise>()));

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _sut.DeleteAsync(exerciseId);

        // Assert
        _exerciseRepositoryMock.Verify(x => x.Remove(existingExercise), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByPatientIdAsync_ReturnsOnlyPatientExercises()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var exercises = new List<Exercise>
        {
            new Exercise { Id = Guid.NewGuid(), PatientId = patientId, Description = "Exercise 1", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Exercise { Id = Guid.NewGuid(), PatientId = patientId, Description = "Exercise 2", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Exercise { Id = Guid.NewGuid(), PatientId = patientId, Description = "Exercise 3", Status = "Completed", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        _exerciseRepositoryMock
            .Setup(x => x.GetByPatientIdAsync(patientId))
            .ReturnsAsync(exercises);

        // Act
        var result = await _sut.GetByPatientIdAsync(patientId);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(3);
        result.Should().OnlyContain(e => e.PatientId == patientId);
    }

    [Fact]
    public async Task LogCompletionAsync_WrongPatient_ThrowsUnauthorized()
    {
        // Arrange
        var exerciseId = Guid.NewGuid();
        var exercisePatientId = Guid.NewGuid();
        var differentPatientId = Guid.NewGuid();

        var existingExercise = new Exercise
        {
            Id = exerciseId,
            PatientId = exercisePatientId,
            Description = "Test Exercise",
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var dto = new ExerciseLogCreateDto
        {
            ExerciseId = exerciseId,
            PatientId = differentPatientId,
            CompletionStatus = "Completed",
            ReflectionNote = "Done"
        };

        _exerciseRepositoryMock
            .Setup(x => x.GetByIdAsync(exerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingExercise);

        // Act & Assert
        var act = () => _sut.LogCompletionAsync(dto);
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Exercise does not belong to this patient.");
    }

    [Fact]
    public async Task ExtendDueDateAsync_ValidId_UpdatesDueDate()
    {
        // Arrange
        var exerciseId = Guid.NewGuid();
        var newDueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14));
        var existingExercise = new Exercise
        {
            Id = exerciseId,
            PatientId = Guid.NewGuid(),
            Description = "Test Exercise",
            DueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)),
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _exerciseRepositoryMock
            .Setup(x => x.GetByIdAsync(exerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingExercise);

        _exerciseRepositoryMock
            .Setup(x => x.Update(It.IsAny<Exercise>()));

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _sut.ExtendDueDateAsync(exerciseId, newDueDate);

        // Assert
        existingExercise.DueDate.Should().Be(newDueDate);
        _exerciseRepositoryMock.Verify(x => x.Update(existingExercise), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
