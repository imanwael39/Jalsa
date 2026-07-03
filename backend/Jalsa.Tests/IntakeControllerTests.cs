using System.Security.Claims;
using System.Text;
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
        var patientId = Guid.NewGuid();
        var intakeFormId = Guid.NewGuid();
        var intakeFormDto = new IntakeFormViewDto { Id = intakeFormId, PatientId = patientId };
        var ocrResult = new OcrResult
        {
            ExtractedJson = "{\"name\":\"test\"}",
            RawText = "extracted text",
            Confidence = 0.95m,
            ModelUsed = "gpt-4o"
        };

        var fileBytes = Encoding.UTF8.GetBytes("test-image-content");
        var stream = new MemoryStream(fileBytes);
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
        fileMock.Setup(_ => _.FileName).Returns("test.jpg");
        fileMock.Setup(_ => _.Length).Returns(stream.Length);
        fileMock.Setup(_ => _.ContentType).Returns("image/jpeg");

        _intakeServiceMock
            .Setup(x => x.GetByPatientIdAsync(patientId, _therapistUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(intakeFormDto);

        _ocrServiceMock
            .Setup(x => x.ExtractFromImageAsync(It.IsAny<string>()))
            .ReturnsAsync(ocrResult);

        _ocrExtractionRepoMock
            .Setup(x => x.AddAsync(It.IsAny<IntakeFormOcrExtraction>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _sut.RunOcr(patientId, intakeFormId, fileMock.Object);

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = ok.Value!;
        var responseType = response.GetType();
        var imageUrl = responseType.GetProperty("imageUrl")!.GetValue(response) as string;
        var extractedData = responseType.GetProperty("extractedData")!.GetValue(response) as Dictionary<string, string>;
        imageUrl.Should().StartWith("data:image/jpeg;base64,");
        extractedData.Should().ContainKey("name");
        extractedData!["name"].Should().Be("test");
        _ocrExtractionRepoMock.Verify(x => x.AddAsync(It.IsAny<IntakeFormOcrExtraction>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RunOcr_IntakeFormIdBelongsToDifferentForm_ReturnsNotFound()
    {
        // Guards against IDOR: a therapist must not be able to run OCR against an
        // intakeFormId that doesn't match the patient's actual intake form.
        var patientId = Guid.NewGuid();
        var requestedIntakeFormId = Guid.NewGuid();
        var actualIntakeFormDto = new IntakeFormViewDto { Id = Guid.NewGuid(), PatientId = patientId };

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.Length).Returns(100);
        fileMock.Setup(_ => _.ContentType).Returns("image/jpeg");

        _intakeServiceMock
            .Setup(x => x.GetByPatientIdAsync(patientId, _therapistUserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(actualIntakeFormDto);

        var result = await _sut.RunOcr(patientId, requestedIntakeFormId, fileMock.Object);

        result.Should().BeOfType<NotFoundObjectResult>();
        _ocrServiceMock.Verify(x => x.ExtractFromImageAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task RunOcr_NoFile_ReturnsBadRequest()
    {
        var patientId = Guid.NewGuid();
        var intakeFormId = Guid.NewGuid();

        var result = await _sut.RunOcr(patientId, intakeFormId, null!);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task RunOcr_FileExceedsSizeLimit_ReturnsBadRequest()
    {
        var patientId = Guid.NewGuid();
        var intakeFormId = Guid.NewGuid();

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.Length).Returns(11 * 1024 * 1024); // 11 MB, over the 10 MB limit
        fileMock.Setup(_ => _.ContentType).Returns("image/jpeg");

        var result = await _sut.RunOcr(patientId, intakeFormId, fileMock.Object);

        result.Should().BeOfType<BadRequestObjectResult>();
        _intakeServiceMock.Verify(
            x => x.GetByPatientIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task RunOcr_UnsupportedContentType_ReturnsBadRequest()
    {
        var patientId = Guid.NewGuid();
        var intakeFormId = Guid.NewGuid();

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.Length).Returns(100);
        fileMock.Setup(_ => _.ContentType).Returns("application/x-msdownload");

        var result = await _sut.RunOcr(patientId, intakeFormId, fileMock.Object);

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
