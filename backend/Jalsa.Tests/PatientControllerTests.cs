using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.Application.DTOs.Patient;
using Jalsa.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class PatientControllerTests
{
    private readonly Mock<IPatientService> _patientServiceMock;
    private readonly PatientController _sut;
    private readonly Guid _therapistUserId;

    public PatientControllerTests()
    {
        _patientServiceMock = new Mock<IPatientService>();

        _sut = new PatientController(_patientServiceMock.Object);

        _therapistUserId = Guid.NewGuid();
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, _therapistUserId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    private static PatientViewDto MakePatientDto(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        FullName = "مريض تجريبي",
        Gender = "Male",
        Phone = "01000000000",
        Email = "patient@test.com",
        Status = "Active",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    [Fact]
    public async Task GetById_ExistingId_ReturnsOk()
    {
        var patientId = Guid.NewGuid();
        var dto = MakePatientDto(patientId);

        _patientServiceMock
            .Setup(x => x.GetByIdAsync(patientId, _therapistUserId))
            .ReturnsAsync(dto);

        var result = await _sut.GetById(patientId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithList()
    {
        var patients = new List<PatientViewDto> { MakePatientDto(), MakePatientDto() };

        _patientServiceMock
            .Setup(x => x.GetAllAsync(_therapistUserId, null))
            .ReturnsAsync(patients);

        var result = await _sut.GetAll(null);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = ok.Value.Should().BeAssignableTo<IEnumerable<PatientViewDto>>().Subject;
        value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Create_ValidDto_Returns201Created()
    {
        var dto = new PatientCreateDto { FullName = "مريض جديد", Gender = "Female", Phone = "01111111111" };
        var created = MakePatientDto();

        _patientServiceMock
            .Setup(x => x.CreateAsync(dto, _therapistUserId))
            .ReturnsAsync(created);

        var result = await _sut.Create(dto);

        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
        createdResult.Value.Should().BeEquivalentTo(created);
    }

    [Fact]
    public async Task Update_ValidDto_ReturnsOk()
    {
        var patientId = Guid.NewGuid();
        var dto = new PatientUpdateDto { FullName = "اسم محدث" };
        var updated = MakePatientDto(patientId);
        updated.FullName = "اسم محدث";

        _patientServiceMock
            .Setup(x => x.UpdateAsync(patientId, dto, _therapistUserId))
            .ReturnsAsync(updated);

        var result = await _sut.Update(patientId, dto);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ((PatientViewDto)ok.Value!).FullName.Should().Be("اسم محدث");
    }

    [Fact]
    public async Task Archive_ExistingId_ReturnsNoContent()
    {
        var patientId = Guid.NewGuid();
        _patientServiceMock
            .Setup(x => x.ArchiveAsync(patientId, _therapistUserId))
            .Returns(Task.CompletedTask);

        var result = await _sut.Archive(patientId);

        result.Should().BeOfType<NoContentResult>();
        _patientServiceMock.Verify(x => x.ArchiveAsync(patientId, _therapistUserId), Times.Once);
    }

    [Fact]
    public async Task Restore_ExistingId_ReturnsNoContent()
    {
        var patientId = Guid.NewGuid();
        _patientServiceMock
            .Setup(x => x.RestoreAsync(patientId, _therapistUserId))
            .Returns(Task.CompletedTask);

        var result = await _sut.Restore(patientId);

        result.Should().BeOfType<NoContentResult>();
        _patientServiceMock.Verify(x => x.RestoreAsync(patientId, _therapistUserId), Times.Once);
    }

    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        var patientId = Guid.NewGuid();
        _patientServiceMock
            .Setup(x => x.DeleteAsync(patientId, _therapistUserId))
            .Returns(Task.CompletedTask);

        var result = await _sut.Delete(patientId);

        result.Should().BeOfType<NoContentResult>();
        _patientServiceMock.Verify(x => x.DeleteAsync(patientId, _therapistUserId), Times.Once);
    }
}
