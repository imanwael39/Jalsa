using FluentValidation;
using Jalsa.Application.DTOs.Session;

namespace Jalsa.Application.Validators.Session;

public class SessionUpdateDtoValidator : AbstractValidator<SessionUpdateDto>
{
    public SessionUpdateDtoValidator()
    {
        RuleFor(x => x.DurationMinutes)
            .InclusiveBetween(1, 480).WithMessage("DurationMinutes must be between 1 and 480.")
            .When(x => x.DurationMinutes.HasValue);

        RuleFor(x => x.SessionType)
            .MaximumLength(100).WithMessage("SessionType must not exceed 100 characters.")
            .When(x => x.SessionType is not null);

        RuleFor(x => x.Status)
            .MaximumLength(50).WithMessage("Status must not exceed 50 characters.")
            .When(x => x.Status is not null);
    }
}
