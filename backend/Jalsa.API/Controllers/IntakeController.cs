using System.Text.Json;
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

    private static readonly HashSet<string> AllowedOcrContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg", "image/png", "application/pdf"
    };
    private const long MaxOcrFileSizeBytes = 10 * 1024 * 1024; // 10 MB, matches the frontend's declared limit

    [HttpPost("api/patient/{patientId:guid}/intake/{intakeFormId:guid}/ocr")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> RunOcr(Guid patientId, Guid intakeFormId, IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "لم يتم إرفاق ملف" });

        if (file.Length > MaxOcrFileSizeBytes)
            return BadRequest(new { error = "حجم الملف يتجاوز الحد الأقصى المسموح به (10 ميجابايت)" });

        if (string.IsNullOrWhiteSpace(file.ContentType) || !AllowedOcrContentTypes.Contains(file.ContentType))
            return BadRequest(new { error = "نوع الملف غير مدعوم" });

        var therapistId = GetCurrentUserId();
        var intake = await _intakeService.GetByPatientIdAsync(patientId, therapistId);
        if (intake.Id != intakeFormId)
            return NotFound(new { error = "Intake form not found" });

        using var ms = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(ms);
        var bytes = ms.ToArray();
        var base64 = Convert.ToBase64String(bytes);
        var contentType = file.ContentType ?? "image/jpeg";
        var dataUri = $"data:{contentType};base64,{base64}";

        var result = await _ocrService.ExtractFromImageAsync(dataUri);

        var extractedData = new Dictionary<string, string>();
        if (!string.IsNullOrWhiteSpace(result.ExtractedJson))
        {
            try
            {
                extractedData = JsonSerializer.Deserialize<Dictionary<string, string>>(result.ExtractedJson)
                    ?? new Dictionary<string, string>();
            }
            catch { /* fallback to empty dict */ }
        }

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

        return Ok(new
        {
            imageUrl = dataUri,
            extractedData
        });
    }
}
