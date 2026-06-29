using FluentValidation;
using Jalsa.Application.DTOs.Session;

namespace Jalsa.Application.Validators.Session;

public class SessionCreateDtoValidator : AbstractValidator<SessionCreateDto>
{
    public SessionCreateDtoValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("PatientId is required.");

        RuleFor(x => x.DurationMinutes)
            .InclusiveBetween(1, 480).WithMessage("DurationMinutes must be between 1 and 480.")
            .When(x => x.DurationMinutes.HasValue);

        RuleFor(x => x.SessionType)
            .MaximumLength(100).WithMessage("SessionType must not exceed 100 characters.")
            .When(x => x.SessionType is not null);
    }
}
