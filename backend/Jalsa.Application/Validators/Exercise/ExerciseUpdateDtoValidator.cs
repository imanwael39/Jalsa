using FluentValidation;
using Jalsa.Application.DTOs.Exercise;

namespace Jalsa.Application.Validators.Exercise;

public class ExerciseUpdateDtoValidator : AbstractValidator<ExerciseUpdateDto>
{
    public ExerciseUpdateDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.")
            .When(x => x.Description is not null);

        RuleFor(x => x.DueDate)
            .Must(d => !d.HasValue || d.Value > DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("DueDate must be greater than today.");
    }
}
