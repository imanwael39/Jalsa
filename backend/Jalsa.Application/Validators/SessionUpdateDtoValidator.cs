using FluentValidation;
using Jalsa.Application.DTOs.Session;

namespace Jalsa.Application.Validators;

public class SessionUpdateDtoValidator : AbstractValidator<SessionUpdateDto>
{
    public SessionUpdateDtoValidator()
    {
        RuleFor(x => x.SessionDate)
            .NotEmpty().WithMessage("Session date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Session date cannot be in the future.");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).When(x => x.DurationMinutes.HasValue).WithMessage("Duration must be greater than 0.");

        RuleFor(x => x.SessionType)
            .MaximumLength(100).WithMessage("Session type cannot exceed 100 characters.");

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.")
            .Must(s => s == null || new[] { "Draft", "Final", "InProgress", "Cancelled" }.Contains(s))
            .WithMessage("Status must be one of: Draft, Final, InProgress, Cancelled.");

        RuleFor(x => x.SessionNote)
            .SetValidator(new SessionNoteUpdateDtoValidator()!).When(x => x.SessionNote is not null);
    }
}

public class SessionNoteUpdateDtoValidator : AbstractValidator<SessionNoteUpdateDto>
{
    public SessionNoteUpdateDtoValidator()
    {
        RuleFor(x => x.Observations)
            .MaximumLength(10000).WithMessage("Observations cannot exceed 10,000 characters.");

        RuleFor(x => x.Interventions)
            .MaximumLength(10000).WithMessage("Interventions cannot exceed 10,000 characters.");

        RuleFor(x => x.PatientResponse)
            .MaximumLength(10000).WithMessage("Patient response cannot exceed 10,000 characters.");

        RuleFor(x => x.HomeworkAssigned)
            .MaximumLength(10000).WithMessage("Homework assigned cannot exceed 10,000 characters.");

        RuleFor(x => x.NextGoals)
            .MaximumLength(10000).WithMessage("Next goals cannot exceed 10,000 characters.");
    }
}
