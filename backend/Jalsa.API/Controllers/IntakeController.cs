using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.Interfaces.Repositores;
using Jalsa.Domain.Models.Patient;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/intake")]
[Authorize(Roles = "Therapist")]
public class IntakeController : ControllerBase
{
    private readonly IOcrService _ocrService;
    private readonly IUnitOfWork _unitOfWork;

    public IntakeController(IOcrService ocrService, IUnitOfWork unitOfWork)
    {
        _ocrService = ocrService;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("{intakeFormId}/ocr")]
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

public class OcrRequest
{
    public string ImageUrl { get; set; } = string.Empty;
}
