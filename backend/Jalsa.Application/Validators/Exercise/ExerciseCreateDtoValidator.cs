using FluentValidation;
using Jalsa.Application.DTOs.Exercise;

namespace Jalsa.Application.Validators.Exercise;

public class ExerciseCreateDtoValidator : AbstractValidator<ExerciseCreateDto>
{
    public ExerciseCreateDtoValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("PatientId is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.DueDate)
            .Must(d => !d.HasValue || d.Value > DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("DueDate must be greater than today.");
    }
}
