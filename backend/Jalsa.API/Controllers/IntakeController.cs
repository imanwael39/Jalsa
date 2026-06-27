using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/intake")]
[Authorize(Roles = "Therapist")]
public class IntakeController : ControllerBase
{
    private readonly IOcrService _ocrService;
    private readonly Galsa_DBDbContext _context;

    public IntakeController(IOcrService ocrService, Galsa_DBDbContext context)
    {
        _ocrService = ocrService;
        _context = context;
    }

    [HttpPost("{intakeFormId}/ocr")]
    public async Task<IActionResult> RunOcr(Guid intakeFormId, [FromBody] OcrRequest request)
    {
        var intakeForm = await _context.IntakeForms.FindAsync(intakeFormId);
        if (intakeForm == null)
            return NotFound(new { error = "Intake form not found" });

        var result = await _ocrService.ExtractFromImageAsync(request.ImageUrl);

        _context.IntakeFormOcrExtractions.Add(new Domain.Models.Patient.IntakeFormOcrExtraction
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

        await _context.SaveChangesAsync();

        return Ok(result);
    }
}

public class OcrRequest
{
    public string ImageUrl { get; set; } = string.Empty;
}
