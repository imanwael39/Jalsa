using System.Linq.Expressions;
using FluentAssertions;
using Jalsa.Application.DTOs.Exercise;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Application.Services;
using Jalsa.Domain.Models.Clinic;
using Jalsa.Domain.Models.Exercise;
using Jalsa.Domain.Models.Patient;
using Moq;

namespace Jalsa.Tests;

public class ExerciseServiceTests
{
    private readonly Mock<IExerciseRepository> _exerciseRepositoryMock;
    private readonly Mock<IExerciseLogRepository> _exerciseLogRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IGenericRepository<Therapist>> _therapistRepoMock;
    private readonly Mock<IGenericRepository<Patient>> _patientRepoMock;
    private readonly ExerciseService _sut;

    private readonly Guid _userId = Guid.NewGuid();
    private readonly Guid _therapistId = Guid.NewGuid();
    private readonly Guid _patientId = Guid.NewGuid();

    public ExerciseServiceTests()
    {
        _exerciseRepositoryMock = new Mock<IExerciseRepository>();
        _exerciseLogRepositoryMock = new Mock<IExerciseLogRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _therapistRepoMock = new Mock<IGenericRepository<Therapist>>();
        _patientRepoMock = new Mock<IGenericRepository<Patient>>();

        _unitOfWorkMock.Setup(u => u.Repository<Therapist>()).Returns(_therapistRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<Patient>()).Returns(_patientRepoMock.Object);

        SetupTherapistResolution();
        SetupPatientOwnership();

        _sut = new ExerciseService(
            _exerciseRepositoryMock.Object,
            _exerciseLogRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    private void SetupTherapistResolution()
    {
        _therapistRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Therapist { Id = _therapistId, UserId = _userId });
    }

    private void SetupPatientOwnership()
    {
        _patientRepoMock
            .Setup(r => r.GetByIdAsync(_patientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patient { Id = _patientId, TherapistId = _therapistId, FullName = "Test Patient" });
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsExerciseViewDto()
    {
        var dto = new ExerciseCreateDto
        {
            PatientId = _patientId,
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

        var result = await _sut.CreateAsync(dto, _userId);

        result.Should().NotBeNull();
        result.PatientId.Should().Be(dto.PatientId);
        result.Description.Should().Be(dto.Description);
        result.Frequency.Should().Be(dto.Frequency);
        _exerciseRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Exercise>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_OtherTherapistPatient_ThrowsUnauthorized()
    {
        var otherPatientId = Guid.NewGuid();
        _patientRepoMock
            .Setup(r => r.GetByIdAsync(otherPatientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patient { Id = otherPatientId, TherapistId = Guid.NewGuid(), FullName = "Other Patient" });

        var dto = new ExerciseCreateDto
        {
            PatientId = otherPatientId,
            Description = "Test",
            Frequency = "Daily"
        };

        var act = () => _sut.CreateAsync(dto, _userId);
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Patient does not belong to this therapist.");
    }

    [Fact]
    public async Task UpdateAsync_ExistingId_UpdatesDescription()
    {
        var exerciseId = Guid.NewGuid();
        var existingExercise = new Exercise
        {
            Id = exerciseId,
            PatientId = _patientId,
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

        var result = await _sut.UpdateAsync(dto, _userId);

        result.Should().NotBeNull();
        result.Description.Should().Be("New Description");
        result.Frequency.Should().Be("Weekly");
        _exerciseRepositoryMock.Verify(x => x.Update(It.IsAny<Exercise>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_OtherTherapistExercise_ThrowsUnauthorized()
    {
        var exerciseId = Guid.NewGuid();
        var otherPatientId = Guid.NewGuid();

        _exerciseRepositoryMock
            .Setup(x => x.GetByIdAsync(exerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Exercise { Id = exerciseId, PatientId = otherPatientId, Status = "Active" });

        _patientRepoMock
            .Setup(r => r.GetByIdAsync(otherPatientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patient { Id = otherPatientId, TherapistId = Guid.NewGuid(), FullName = "Other" });

        var dto = new ExerciseUpdateDto { Id = exerciseId, Description = "Hack" };

        var act = () => _sut.UpdateAsync(dto, _userId);
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_CallsSaveChanges()
    {
        var exerciseId = Guid.NewGuid();
        var existingExercise = new Exercise
        {
            Id = exerciseId,
            PatientId = _patientId,
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

        await _sut.DeleteAsync(exerciseId, _userId);

        _exerciseRepositoryMock.Verify(x => x.Remove(existingExercise), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_OtherTherapistExercise_ThrowsUnauthorized()
    {
        var exerciseId = Guid.NewGuid();
        var otherPatientId = Guid.NewGuid();

        _exerciseRepositoryMock
            .Setup(x => x.GetByIdAsync(exerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Exercise { Id = exerciseId, PatientId = otherPatientId, Status = "Active" });

        _patientRepoMock
            .Setup(r => r.GetByIdAsync(otherPatientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patient { Id = otherPatientId, TherapistId = Guid.NewGuid(), FullName = "Other" });

        var act = () => _sut.DeleteAsync(exerciseId, _userId);
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetByIdAsync_OwnExercise_ReturnsDto()
    {
        var exerciseId = Guid.NewGuid();
        var exercise = new Exercise
        {
            Id = exerciseId,
            PatientId = _patientId,
            Description = "My Exercise",
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _exerciseRepositoryMock
            .Setup(x => x.GetByIdAsync(exerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exercise);

        var result = await _sut.GetByIdAsync(exerciseId, _userId);

        result.Should().NotBeNull();
        result.Id.Should().Be(exerciseId);
    }

    [Fact]
    public async Task GetByIdAsync_OtherTherapistExercise_ThrowsUnauthorized()
    {
        var exerciseId = Guid.NewGuid();
        var otherPatientId = Guid.NewGuid();

        _exerciseRepositoryMock
            .Setup(x => x.GetByIdAsync(exerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Exercise { Id = exerciseId, PatientId = otherPatientId, Status = "Active" });

        _patientRepoMock
            .Setup(r => r.GetByIdAsync(otherPatientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patient { Id = otherPatientId, TherapistId = Guid.NewGuid(), FullName = "Other" });

        var act = () => _sut.GetByIdAsync(exerciseId, _userId);
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOnlyOwnPatientsExercises()
    {
        var otherPatientId = Guid.NewGuid();

        _patientRepoMock
            .Setup(r => r.FindAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Patient> { new Patient { Id = _patientId, TherapistId = _therapistId, FullName = "Own" } });

        var exercises = new List<Exercise>
        {
            new Exercise { Id = Guid.NewGuid(), PatientId = _patientId, Description = "Own Exercise", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Exercise { Id = Guid.NewGuid(), PatientId = otherPatientId, Description = "Other Exercise", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        _exerciseRepositoryMock
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(exercises);

        var result = (await _sut.GetAllAsync(_userId)).ToList();

        result.Should().HaveCount(1);
        result[0].PatientId.Should().Be(_patientId);
    }

    [Fact]
    public async Task GetByPatientIdAsync_WithOwnership_ReturnsExercises()
    {
        var exercises = new List<Exercise>
        {
            new Exercise { Id = Guid.NewGuid(), PatientId = _patientId, Description = "Ex 1", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Exercise { Id = Guid.NewGuid(), PatientId = _patientId, Description = "Ex 2", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        _exerciseRepositoryMock
            .Setup(x => x.GetByPatientIdAsync(_patientId))
            .ReturnsAsync(exercises);

        var result = await _sut.GetByPatientIdAsync(_patientId, _userId);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByPatientIdAsync_WithoutOwnership_ReturnsExercises()
    {
        var exercises = new List<Exercise>
        {
            new Exercise { Id = Guid.NewGuid(), PatientId = _patientId, Description = "Ex 1", Status = "Active", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        _exerciseRepositoryMock
            .Setup(x => x.GetByPatientIdAsync(_patientId))
            .ReturnsAsync(exercises);

        var result = await _sut.GetByPatientIdAsync(_patientId);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task LogCompletionAsync_WrongPatient_ThrowsUnauthorized()
    {
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

        var act = () => _sut.LogCompletionAsync(dto);
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Exercise does not belong to this patient.");
    }

    [Fact]
    public async Task ExtendDueDateAsync_OwnExercise_UpdatesDueDate()
    {
        var exerciseId = Guid.NewGuid();
        var newDueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14));
        var existingExercise = new Exercise
        {
            Id = exerciseId,
            PatientId = _patientId,
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

        await _sut.ExtendDueDateAsync(exerciseId, newDueDate, _userId);

        existingExercise.DueDate.Should().Be(newDueDate);
        _exerciseRepositoryMock.Verify(x => x.Update(existingExercise), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExtendDueDateAsync_OtherTherapistExercise_ThrowsUnauthorized()
    {
        var exerciseId = Guid.NewGuid();
        var otherPatientId = Guid.NewGuid();

        _exerciseRepositoryMock
            .Setup(x => x.GetByIdAsync(exerciseId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Exercise { Id = exerciseId, PatientId = otherPatientId, Status = "Active" });

        _patientRepoMock
            .Setup(r => r.GetByIdAsync(otherPatientId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Patient { Id = otherPatientId, TherapistId = Guid.NewGuid(), FullName = "Other" });

        var act = () => _sut.ExtendDueDateAsync(exerciseId, DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14)), _userId);
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task NoTherapistProfile_ThrowsUnauthorized()
    {
        _therapistRepoMock
            .Setup(r => r.FindSingleAsync(It.IsAny<Expression<Func<Therapist, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Therapist?)null);

        var unknownUserId = Guid.NewGuid();
        var act = () => _sut.GetAllAsync(unknownUserId);
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Therapist profile not found.");
    }
}
