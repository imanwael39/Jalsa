using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jalsa.API.Exceptions;
using Jalsa.API.Services.Interfaces.AI;
using Jalsa.Application.DTOs.Report;
using Jalsa.Application.Interfaces.Services;

namespace Jalsa.API.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = "Therapist")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IReportGenerationService _aiService;

    public ReportController(IReportService reportService, IReportGenerationService aiService)
    {
        _reportService = reportService;
        _aiService = aiService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] ReportGenerateDto dto)
    {
        var therapistId = GetCurrentUserId();
        var aiContent = await _aiService.GenerateDraftAsync(dto.PatientId, dto.TherapistInstructions, dto.Language);
        var result = await _reportService.CreateWithAiContentAsync(dto.PatientId, aiContent, therapistId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var therapistId = GetCurrentUserId();
        var result = await _reportService.GetByIdAsync(id, therapistId);
        return Ok(result);
    }

    [HttpGet("patient/{patientId:guid}")]
    public async Task<IActionResult> GetByPatientId(Guid patientId)
    {
        var therapistId = GetCurrentUserId();
        var result = await _reportService.GetByPatientIdAsync(patientId, therapistId);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ReportUpdateDto dto)
    {
        var therapistId = GetCurrentUserId();
        var result = await _reportService.UpdateAsync(id, dto, therapistId);
        return Ok(result);
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var therapistId = GetCurrentUserId();
        var result = await _reportService.ApproveAsync(id, therapistId);
        return Ok(result);
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id)
    {
        var therapistId = GetCurrentUserId();
        var result = await _reportService.RejectAsync(id, therapistId);
        return Ok(result);
    }

    [HttpGet("{id:guid}/export")]
    public async Task<IActionResult> Export(Guid id)
    {
        var therapistId = GetCurrentUserId();
        var report = await _reportService.GetByIdAsync(id, therapistId);

        var content = report.CurrentVersion?.Content ?? string.Empty;
        var html = BuildReportHtml(report.PatientId.ToString(), content, report.CreatedAt);
        var bytes = Encoding.UTF8.GetBytes(html);

        return File(bytes, "text/html; charset=utf-8", $"report-{id}.html");
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var therapistId = GetCurrentUserId();
        await _reportService.DeleteAsync(id, therapistId);
        return NoContent();
    }

    private static string BuildReportHtml(string patientId, string content, DateTime createdAt)
    {
        var date = createdAt.ToString("dd/MM/yyyy");
        var escaped = System.Net.WebUtility.HtmlEncode(content).Replace("\n", "<br/>");
        return "<!DOCTYPE html>" +
            "<html lang=\"ar\" dir=\"rtl\"><head><meta charset=\"utf-8\"/><title>تقرير طبي - جلسة</title><style>" +
            "@import url('https://fonts.googleapis.com/css2?family=Tajawal:wght@400;600;700&display=swap');" +
            "*,*::before,*::after{box-sizing:border-box;margin:0;padding:0}" +
            "body{font-family:'Tajawal',Arial,sans-serif;direction:rtl;background:#fff;color:#1a1a1a;padding:2rem;line-height:1.8}" +
            ".report-header{border-bottom:2px solid #0F6E56;padding-bottom:1rem;margin-bottom:2rem}" +
            ".clinic-name{font-size:1.5rem;font-weight:700;color:#0F6E56}" +
            ".report-meta{font-size:0.9rem;color:#555;margin-top:0.5rem}" +
            ".report-content{font-size:1rem;white-space:pre-wrap}" +
            ".report-footer{margin-top:3rem;border-top:1px solid #ccc;padding-top:1rem;font-size:0.85rem;color:#777}" +
            "@media print{body{padding:1cm}.report-header{page-break-after:avoid}}" +
            "</style></head><body>" +
            "<div class=\"report-header\">" +
            "<div class=\"clinic-name\">جلسة - نظام إدارة العيادة النفسية</div>" +
            $"<div class=\"report-meta\">تاريخ التقرير: {date} | رقم المريض: {patientId}</div>" +
            "</div>" +
            $"<div class=\"report-content\">{escaped}</div>" +
            "<div class=\"report-footer\">تم إنشاء هذا التقرير بواسطة نظام جلسة — للطباعة: Ctrl+P ← حفظ كـ PDF</div>" +
            "</body></html>";
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub");

        if (string.IsNullOrEmpty(claim) || !Guid.TryParse(claim, out var userId))
            throw new ApiException(401, "Invalid authentication token");

        return userId;
    }
}
