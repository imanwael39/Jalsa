using FluentValidation;
using Jalsa.Application.DTOs.Intake;

namespace Jalsa.Application.Validators.Intake;

public class IntakeFormSaveDtoValidator : AbstractValidator<IntakeFormSaveDto>
{
    public IntakeFormSaveDtoValidator()
    {
        RuleFor(x => x.PresentingProblem)
            .MaximumLength(5000).WithMessage("PresentingProblem must not exceed 5000 characters.")
            .When(x => x.PresentingProblem is not null);

        RuleFor(x => x.PsychiatricHistory)
            .MaximumLength(5000).WithMessage("PsychiatricHistory must not exceed 5000 characters.")
            .When(x => x.PsychiatricHistory is not null);

        RuleFor(x => x.FamilyHistory)
            .MaximumLength(5000).WithMessage("FamilyHistory must not exceed 5000 characters.")
            .When(x => x.FamilyHistory is not null);

        RuleFor(x => x.Medications)
            .MaximumLength(2000).WithMessage("Medications must not exceed 2000 characters.")
            .When(x => x.Medications is not null);

        RuleFor(x => x.SocialHistory)
            .MaximumLength(5000).WithMessage("SocialHistory must not exceed 5000 characters.")
            .When(x => x.SocialHistory is not null);
    }
}
