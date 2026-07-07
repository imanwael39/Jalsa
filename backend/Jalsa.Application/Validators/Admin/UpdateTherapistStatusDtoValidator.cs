using FluentValidation;
using Jalsa.Application.DTOs.Admin;
using Jalsa.Domain.Models.Clinic;

namespace Jalsa.Application.Validators.Admin;

public class UpdateTherapistStatusDtoValidator : AbstractValidator<UpdateTherapistStatusDto>
{
    public UpdateTherapistStatusDtoValidator()
    {
        RuleFor(x => x.NewStatus)
            .NotEmpty().WithMessage("NewStatus is required.")
            .Must(status => TherapistApprovalStatus.All.Contains(status))
            .WithMessage($"NewStatus must be one of: {string.Join(", ", TherapistApprovalStatus.All)}.");
    }
}
