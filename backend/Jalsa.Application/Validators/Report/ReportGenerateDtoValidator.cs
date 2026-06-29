using FluentValidation;
using Jalsa.Application.DTOs.Report;

namespace Jalsa.Application.Validators.Report;

public class ReportGenerateDtoValidator : AbstractValidator<ReportGenerateDto>
{
    public ReportGenerateDtoValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("PatientId is required.");

        RuleFor(x => x.TherapistInstructions)
            .MaximumLength(2000).WithMessage("TherapistInstructions must not exceed 2000 characters.")
            .When(x => x.TherapistInstructions is not null);

        RuleFor(x => x.Language)
            .NotEmpty().WithMessage("Language is required.")
            .MaximumLength(10).WithMessage("Language must not exceed 10 characters.");
    }
}
