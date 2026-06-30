using FluentValidation;
using Jalsa.Application.DTOs.Session;

namespace Jalsa.Application.Validators.Session;

public class SessionNoteDtoValidator : AbstractValidator<SessionNoteDto>
{
    public SessionNoteDtoValidator()
    {
        RuleFor(x => x.Observations)
            .MaximumLength(5000).WithMessage("Observations must not exceed 5000 characters.")
            .When(x => x.Observations is not null);

        RuleFor(x => x.Interventions)
            .MaximumLength(5000).WithMessage("Interventions must not exceed 5000 characters.")
            .When(x => x.Interventions is not null);

        RuleFor(x => x.PatientResponse)
            .MaximumLength(5000).WithMessage("PatientResponse must not exceed 5000 characters.")
            .When(x => x.PatientResponse is not null);

        RuleFor(x => x.HomeworkAssigned)
            .MaximumLength(2000).WithMessage("HomeworkAssigned must not exceed 2000 characters.")
            .When(x => x.HomeworkAssigned is not null);

        RuleFor(x => x.NextGoals)
            .MaximumLength(2000).WithMessage("NextGoals must not exceed 2000 characters.")
            .When(x => x.NextGoals is not null);
    }
}
