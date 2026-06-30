using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.Application.DTOs.Exercise;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static Jalsa.API.Controllers.ExerciseController;

namespace Jalsa.Tests;

public class ExerciseControllerTests
{
    private readonly Mock<IExerciseService> _exerciseServiceMock;
    private readonly ExerciseController _sut;
    private readonly Guid _userId = Guid.NewGuid();

    public ExerciseControllerTests()
    {
        _exerciseServiceMock = new Mock<IExerciseService>();
        _sut = new ExerciseController(_exerciseServiceMock.Object);

        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _userId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };
    }

    private static ExerciseViewDto MakeExerciseDto(Guid? id = null, Guid? patientId = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        PatientId = patientId ?? Guid.NewGuid(),
        Description = "تمرين اختبار",
        Frequency = "Daily",
        Status = "Active",
        CreatedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task GetAll_ReturnsOkWithList()
    {
        var exercises = new List<ExerciseViewDto> { MakeExerciseDto(), MakeExerciseDto() };
        _exerciseServiceMock.Setup(x => x.GetAllAsync(_userId)).ReturnsAsync(exercises);

        var result = await _sut.GetAll();

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = ok.Value.Should().BeAssignableTo<IEnumerable<ExerciseViewDto>>().Subject;
        value.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsOk()
    {
        var exerciseId = Guid.NewGuid();
        var dto = MakeExerciseDto(exerciseId);
        _exerciseServiceMock.Setup(x => x.GetByIdAsync(exerciseId, _userId)).ReturnsAsync(dto);

        var result = await _sut.GetById(exerciseId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetByPatientId_ReturnsOkWithList()
    {
        var patientId = Guid.NewGuid();
        var exercises = new List<ExerciseViewDto> { MakeExerciseDto(patientId: patientId) };
        _exerciseServiceMock.Setup(x => x.GetByPatientIdAsync(patientId, _userId)).ReturnsAsync(exercises);

        var result = await _sut.GetByPatientId(patientId);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Create_ValidDto_Returns201Created()
    {
        var dto = new ExerciseCreateDto { PatientId = Guid.NewGuid(), Description = "تمرين جديد", Frequency = "Daily" };
        var created = MakeExerciseDto(patientId: dto.PatientId);
        _exerciseServiceMock.Setup(x => x.CreateAsync(dto, _userId)).ReturnsAsync(created);

        var result = await _sut.Create(dto);

        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task Update_ValidDto_ReturnsOk()
    {
        var exerciseId = Guid.NewGuid();
        var dto = new ExerciseUpdateDto { Description = "محدث" };
        var updated = MakeExerciseDto(exerciseId);
        _exerciseServiceMock.Setup(x => x.UpdateAsync(It.Is<ExerciseUpdateDto>(d => d.Id == exerciseId), _userId)).ReturnsAsync(updated);

        var result = await _sut.Update(exerciseId, dto);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        var exerciseId = Guid.NewGuid();
        _exerciseServiceMock.Setup(x => x.DeleteAsync(exerciseId, _userId)).Returns(Task.CompletedTask);

        var result = await _sut.Delete(exerciseId);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task ExtendDueDate_ValidRequest_ReturnsNoContent()
    {
        var exerciseId = Guid.NewGuid();
        var newDueDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14));
        _exerciseServiceMock.Setup(x => x.ExtendDueDateAsync(exerciseId, newDueDate, _userId)).Returns(Task.CompletedTask);

        var result = await _sut.ExtendDueDate(exerciseId, new ExtendDueDateRequest(newDueDate));

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetMyExercises_PatientRole_ReturnsOk()
    {
        var exercises = new List<ExerciseViewDto> { MakeExerciseDto(patientId: _userId) };
        _exerciseServiceMock.Setup(x => x.GetByPatientIdAsync(_userId)).ReturnsAsync(exercises);

        var result = await _sut.GetMyExercises();

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task LogCompletion_ValidDto_Returns201Created()
    {
        var dto = new ExerciseLogCreateDto { ExerciseId = Guid.NewGuid(), CompletionStatus = "Completed" };
        var logView = new ExerciseLogViewDto { Id = Guid.NewGuid(), ExerciseId = dto.ExerciseId, PatientId = _userId, CompletionStatus = "Completed", CreatedAt = DateTime.UtcNow };
        _exerciseServiceMock.Setup(x => x.LogCompletionAsync(It.Is<ExerciseLogCreateDto>(d => d.PatientId == _userId))).ReturnsAsync(logView);

        var result = await _sut.LogCompletion(dto);

        result.Should().BeOfType<CreatedResult>();
    }

    [Fact]
    public async Task GetMyLogs_PatientRole_ReturnsOk()
    {
        var logs = new List<ExerciseLogViewDto> { new() { Id = Guid.NewGuid(), PatientId = _userId, CompletionStatus = "Completed", CreatedAt = DateTime.UtcNow } };
        _exerciseServiceMock.Setup(x => x.GetLogsByPatientIdAsync(_userId)).ReturnsAsync(logs);

        var result = await _sut.GetMyLogs();

        result.Should().BeOfType<OkObjectResult>();
    }
}
