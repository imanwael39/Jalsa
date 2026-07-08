using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.Application.DTOs.PatientAssessment;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/patient-assessments")]
[Authorize(Roles = "Patient")]
public class PatientAssessmentController : BaseController
{
    private readonly IPatientAssessmentService _patientAssessmentService;

    public PatientAssessmentController(IPatientAssessmentService patientAssessmentService)
    {
        _patientAssessmentService = patientAssessmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await _patientAssessmentService.GetListAsync(GetCurrentUserId());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDetail(Guid id)
    {
        var result = await _patientAssessmentService.GetDetailAsync(GetCurrentUserId(), id);
        return Ok(result);
    }

    [HttpPut("{id:guid}/responses/{questionId:guid}")]
    public async Task<IActionResult> SaveAnswer(Guid id, Guid questionId, [FromBody] SaveAnswerRequestDto dto)
    {
        await _patientAssessmentService.SaveAnswerAsync(GetCurrentUserId(), id, questionId, dto);
        return NoContent();
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<IActionResult> Submit(Guid id)
    {
        var result = await _patientAssessmentService.SubmitAsync(GetCurrentUserId(), id);
        return Ok(result);
    }
}
