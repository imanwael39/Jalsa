using FluentValidation;
using Jalsa.Application.DTOs.Exercise;

namespace Jalsa.Application.Validators.Exercise;

public class ExerciseLogCreateDtoValidator : AbstractValidator<ExerciseLogCreateDto>
{
    public ExerciseLogCreateDtoValidator()
    {
        RuleFor(x => x.ExerciseId)
            .NotEmpty().WithMessage("ExerciseId is required.");

        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("PatientId is required.");

        RuleFor(x => x.CompletionStatus)
            .NotEmpty().WithMessage("CompletionStatus is required.")
            .Must(s => new[] { "Completed", "Partial", "Skipped" }.Contains(s))
            .WithMessage("CompletionStatus must be one of: Completed, Partial, Skipped.");

        RuleFor(x => x.ReflectionNote)
            .MaximumLength(2000).WithMessage("ReflectionNote must not exceed 2000 characters.")
            .When(x => x.ReflectionNote is not null);
    }
}
