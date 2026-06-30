using System.ComponentModel.DataAnnotations;

namespace Jalsa.Application.DTOs.Intake;

public class OcrRequest
{
    [Required]
    public string ImageUrl { get; set; } = string.Empty;
}
