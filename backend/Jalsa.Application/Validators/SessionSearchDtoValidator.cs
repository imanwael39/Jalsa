using FluentValidation;
using Jalsa.Application.DTOs.Session;

namespace Jalsa.Application.Validators;

public class SessionSearchDtoValidator : AbstractValidator<SessionSearchDto>
{
    public SessionSearchDtoValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.DateTo)
            .GreaterThanOrEqualTo(x => x.DateFrom).When(x => x.DateFrom.HasValue && x.DateTo.HasValue)
            .WithMessage("Date To must be greater than or equal to Date From.");

        RuleFor(x => x.Status)
            .Must(s => s == null || new[] { "Draft", "Final", "InProgress", "Cancelled" }.Contains(s))
            .WithMessage("Status must be one of: Draft, Final, InProgress, Cancelled.");
    }
}
