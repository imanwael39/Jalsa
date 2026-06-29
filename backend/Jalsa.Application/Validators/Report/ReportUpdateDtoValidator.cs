using FluentValidation;
using Jalsa.Application.DTOs.Report;

namespace Jalsa.Application.Validators.Report;

public class ReportUpdateDtoValidator : AbstractValidator<ReportUpdateDto>
{
    public ReportUpdateDtoValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(50000).WithMessage("Content must not exceed 50000 characters.");

        RuleFor(x => x.ChangeNote)
            .MaximumLength(500).WithMessage("ChangeNote must not exceed 500 characters.")
            .When(x => x.ChangeNote is not null);
    }
}
