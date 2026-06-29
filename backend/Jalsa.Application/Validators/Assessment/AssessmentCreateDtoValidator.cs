using FluentValidation;
using Jalsa.Application.DTOs.Assessment;

namespace Jalsa.Application.Validators.Assessment;

public class AssessmentCreateDtoValidator : AbstractValidator<AssessmentCreateDto>
{
    public AssessmentCreateDtoValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty().WithMessage("TemplateId is required.");

        RuleFor(x => x.Title)
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            .When(x => x.Title is not null);

        RuleFor(x => x.TotalScore)
            .InclusiveBetween(0, 1000).WithMessage("TotalScore must be between 0 and 1000.")
            .When(x => x.TotalScore.HasValue);
    }
}
