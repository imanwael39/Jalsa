using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.DTOs.Intake;
using Jalsa.Application.Interfaces.Repositories;
using Jalsa.Application.Interfaces.Services;
using Jalsa.Domain.Models.Patient;

namespace Jalsa.API.Controllers;

[ApiController]
[Authorize(Roles = "Therapist")]
public class IntakeController : BaseController
{
    private readonly IOcrService _ocrService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIntakeService _intakeService;

    public IntakeController(IOcrService ocrService, IUnitOfWork unitOfWork, IIntakeService intakeService)
    {
        _ocrService = ocrService;
        _unitOfWork = unitOfWork;
        _intakeService = intakeService;
    }

    [HttpGet("api/patient/{patientId:guid}/intake")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var therapistId = GetCurrentUserId();
        var result = await _intakeService.GetByPatientIdAsync(patientId, therapistId);
        return Ok(result);
    }

    [HttpPost("api/patient/{patientId:guid}/intake")]
    public async Task<IActionResult> Save(Guid patientId, [FromBody] IntakeFormSaveDto dto)
    {
        var therapistId = GetCurrentUserId();
        var result = await _intakeService.SaveAsync(patientId, dto, therapistId);
        return Ok(result);
    }

    [HttpPost("api/patient/{patientId:guid}/intake/submit")]
    public async Task<IActionResult> Submit(Guid patientId)
    {
        var therapistId = GetCurrentUserId();
        var result = await _intakeService.SubmitAsync(patientId, therapistId);
        return Ok(result);
    }

    [HttpPost("api/intake/{intakeFormId:guid}/ocr")]
    public async Task<IActionResult> RunOcr(Guid intakeFormId, [FromBody] OcrRequest request)
    {
        var intakeFormRepo = _unitOfWork.Repository<IntakeForm>();
        var intakeForm = await intakeFormRepo.GetByIdAsync(intakeFormId);
        if (intakeForm == null)
            return NotFound(new { error = "Intake form not found" });

        var result = await _ocrService.ExtractFromImageAsync(request.ImageUrl);

        var extractionRepo = _unitOfWork.Repository<IntakeFormOcrExtraction>();
        await extractionRepo.AddAsync(new IntakeFormOcrExtraction
        {
            Id = Guid.NewGuid(),
            IntakeFormId = intakeFormId,
            ExtractedJson = result.ExtractedJson,
            RawOcrText = result.RawText,
            Confidence = result.Confidence,
            ModelUsed = result.ModelUsed,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        await _unitOfWork.SaveChangesAsync();

        return Ok(result);
    }
}
