using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.DTOs.Session;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class SessionControllerTests
{
    private readonly Mock<ISessionService> _sessionServiceMock;
    private readonly Mock<ISttService> _sttServiceMock;
    private readonly Mock<ISummarizationService> _summarizationServiceMock;
    private readonly SessionController _sut;
    private readonly Guid _therapistUserId;

    public SessionControllerTests()
    {
        _sessionServiceMock = new Mock<ISessionService>();
        _sttServiceMock = new Mock<ISttService>();
        _summarizationServiceMock = new Mock<ISummarizationService>();

        _sut = new SessionController(
            _sessionServiceMock.Object,
            _sttServiceMock.Object,
            _summarizationServiceMock.Object);

        _therapistUserId = Guid.NewGuid();
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, _therapistUserId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    private static SessionViewDto MakeSessionDto(Guid? id = null, Guid? patientId = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        PatientId = patientId ?? Guid.NewGuid(),
        SessionNumber = 1,
        SessionDate = DateOnly.FromDateTime(DateTime.UtcNow),
        DurationMinutes = 50,
        SessionType = "Individual",
        Status = "Draft",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task Create_ValidDto_Returns201Created()
    {
        // Arrange
        var dto = new SessionCreateDto
        {
            PatientId = Guid.NewGuid(),
            SessionDate = DateOnly.FromDateTime(DateTime.UtcNow),
            DurationMinutes = 50
        };
        var created = MakeSessionDto(patientId: dto.PatientId);

        _sessionServiceMock
            .Setup(x => x.CreateAsync(dto, _therapistUserId))
            .ReturnsAsync(created);

        // Act
        var result = await _sut.Create(dto);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(created);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsOk()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var dto = MakeSessionDto(id: sessionId);

        _sessionServiceMock
            .Setup(x => x.GetByIdAsync(sessionId, _therapistUserId))
            .ReturnsAsync(dto);

        // Act
        var result = await _sut.GetById(sessionId);

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetByPatientId_ExistingPatient_ReturnsOkWithList()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var sessions = new List<SessionViewDto> { MakeSessionDto(patientId: patientId), MakeSessionDto(patientId: patientId) };

        _sessionServiceMock
            .Setup(x => x.GetByPatientIdAsync(patientId, _therapistUserId))
            .ReturnsAsync(sessions);

        // Act
        var result = await _sut.GetByPatientId(patientId);

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = ok.Value.Should().BeAssignableTo<IEnumerable<SessionViewDto>>().Subject;
        value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Update_ValidDto_ReturnsOkWithUpdated()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var dto = new SessionUpdateDto { Id = sessionId, Status = "Completed" };
        var updated = MakeSessionDto(id: sessionId);
        updated.Status = "Completed";

        _sessionServiceMock
            .Setup(x => x.UpdateAsync(It.Is<SessionUpdateDto>(d => d.Id == sessionId), _therapistUserId))
            .ReturnsAsync(updated);

        // Act
        var result = await _sut.Update(sessionId, dto);

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ((SessionViewDto)ok.Value!).Status.Should().Be("Completed");
    }

    [Fact]
    public async Task Delete_ExistingId_Returns204NoContent()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        _sessionServiceMock
            .Setup(x => x.DeleteAsync(sessionId, _therapistUserId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.Delete(sessionId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
        _sessionServiceMock.Verify(x => x.DeleteAsync(sessionId, _therapistUserId), Times.Once);
    }

    [Fact]
    public async Task SaveNote_ValidDto_ReturnsOkWithNote()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var dto = new SessionNoteDto { Observations = "لاحظت تحسناً" };
        var noteView = new SessionNoteViewDto
        {
            Id = Guid.NewGuid(),
            SessionId = sessionId,
            Observations = dto.Observations,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _sessionServiceMock
            .Setup(x => x.SaveNoteAsync(sessionId, dto, _therapistUserId))
            .ReturnsAsync(noteView);

        // Act
        var result = await _sut.SaveNote(sessionId, dto);

        // Assert
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(noteView);
    }

    [Fact]
    public async Task GetNote_NoteExists_ReturnsOk()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var note = new SessionNoteViewDto { Id = Guid.NewGuid(), SessionId = sessionId, Observations = "ملاحظة", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

        _sessionServiceMock
            .Setup(x => x.GetNoteAsync(sessionId, _therapistUserId))
            .ReturnsAsync(note);

        // Act
        var result = await _sut.GetNote(sessionId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetNote_NoNote_Returns404()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        _sessionServiceMock
            .Setup(x => x.GetNoteAsync(sessionId, _therapistUserId))
            .ReturnsAsync((SessionNoteViewDto?)null);

        // Act
        var result = await _sut.GetNote(sessionId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
