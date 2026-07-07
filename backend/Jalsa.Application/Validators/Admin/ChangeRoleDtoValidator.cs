using FluentValidation;
using Jalsa.Application.DTOs.Admin;

namespace Jalsa.Application.Validators.Admin;

public class ChangeRoleDtoValidator : AbstractValidator<ChangeRoleDto>
{
    private static readonly string[] AllowedRoles = ["Admin", "Therapist", "Patient"];

    public ChangeRoleDtoValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("RoleName is required.")
            .Must(role => AllowedRoles.Contains(role)).WithMessage($"RoleName must be one of: {string.Join(", ", AllowedRoles)}.");
    }
}
