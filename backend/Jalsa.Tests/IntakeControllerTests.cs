using System.Security.Claims;
using FluentAssertions;
using Jalsa.API.Controllers;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.DTOs.Intake;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Patient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Jalsa.Tests;

public class IntakeControllerTests
{
    private readonly Mock<IOcrService> _ocrServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IIntakeService> _intakeServiceMock;
    private readonly Mock<IGenericRepository<IntakeForm>> _intakeFormRepoMock;
    private readonly Mock<IGenericRepository<IntakeFormOcrExtraction>> _ocrExtractionRepoMock;
    private readonly IntakeController _sut;
    private readonly Guid _therapistUserId;

    public IntakeControllerTests()
    {
        _ocrServiceMock = new Mock<IOcrService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _intakeServiceMock = new Mock<IIntakeService>();
        _intakeFormRepoMock = new Mock<IGenericRepository<IntakeForm>>();
        _ocrExtractionRepoMock = new Mock<IGenericRepository<IntakeFormOcrExtraction>>();

        _unitOfWorkMock.Setup(u => u.Repository<IntakeForm>()).Returns(_intakeFormRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Repository<IntakeFormOcrExtraction>()).Returns(_ocrExtractionRepoMock.Object);

        _sut = new IntakeController(_ocrServiceMock.Object, _unitOfWorkMock.Object, _intakeServiceMock.Object);

        _therapistUserId = Guid.NewGuid();
        var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, _therapistUserId.ToString()) };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [Fact]
    public async Task GetByPatientId_ExistingPatient_ReturnsOk()
    {
        var patientId = Guid.NewGuid();
        var dto = new IntakeFormViewDto
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            PresentingProblem = "قلق",
            Status = "Draft",
            CreatedAt = DateTime.UtcNow
        };

        _intakeServiceMock
            .Setup(x => x.GetByPatientIdAsync(patientId, _therapistUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var result = await _sut.GetByPatientId(patientId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task Save_ValidDto_ReturnsOk()
    {
        var patientId = Guid.NewGuid();
        var dto = new IntakeFormSaveDto
        {
            PresentingProblem = "اكتئاب",
            PsychiatricHistory = "لا يوجد",
            Medications = "لا يوجد"
        };
        var saved = new IntakeFormViewDto
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            PresentingProblem = "اكتئاب",
            Status = "Draft",
            CreatedAt = DateTime.UtcNow
        };

        _intakeServiceMock
            .Setup(x => x.SaveAsync(patientId, dto, _therapistUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(saved);

        var result = await _sut.Save(patientId, dto);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(saved);
    }

    [Fact]
    public async Task Submit_ExistingPatient_ReturnsOk()
    {
        var patientId = Guid.NewGuid();
        var submitted = new IntakeFormViewDto
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            Status = "Submitted",
            SubmittedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _intakeServiceMock
            .Setup(x => x.SubmitAsync(patientId, _therapistUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(submitted);

        var result = await _sut.Submit(patientId);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ((IntakeFormViewDto)ok.Value!).Status.Should().Be("Submitted");
    }

    [Fact]
    public async Task RunOcr_ExistingIntakeForm_ReturnsOkWithOcrResult()
    {
        var intakeFormId = Guid.NewGuid();
        var request = new OcrRequest { ImageUrl = "https://example.com/image.png" };
        var intakeForm = new IntakeForm { Id = intakeFormId };
        var ocrResult = new OcrResult
        {
            ExtractedJson = "{\"name\":\"test\"}",
            RawText = "extracted text",
            Confidence = 0.95m,
            ModelUsed = "gpt-4o"
        };

        _intakeFormRepoMock
            .Setup(x => x.GetByIdAsync(intakeFormId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(intakeForm);

        _ocrServiceMock
            .Setup(x => x.ExtractFromImageAsync(request.ImageUrl))
            .ReturnsAsync(ocrResult);

        _ocrExtractionRepoMock
            .Setup(x => x.AddAsync(It.IsAny<IntakeFormOcrExtraction>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.RunOcr(intakeFormId, request);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeEquivalentTo(ocrResult);
        _ocrExtractionRepoMock.Verify(x => x.AddAsync(It.IsAny<IntakeFormOcrExtraction>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
