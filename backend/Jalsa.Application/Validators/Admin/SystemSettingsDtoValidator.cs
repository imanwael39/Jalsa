using FluentValidation;
using Jalsa.Application.DTOs.Admin;

namespace Jalsa.Application.Validators.Admin;

public class SystemSettingsDtoValidator : AbstractValidator<SystemSettingsDto>
{
    private static readonly string[] AllowedLanguages = ["ar", "en"];

    public SystemSettingsDtoValidator()
    {
        RuleFor(x => x.SiteName)
            .NotEmpty().WithMessage("SiteName is required.")
            .MaximumLength(100).WithMessage("SiteName must not exceed 100 characters.");

        RuleFor(x => x.DefaultLanguage)
            .Must(lang => AllowedLanguages.Contains(lang))
            .WithMessage($"DefaultLanguage must be one of: {string.Join(", ", AllowedLanguages)}.");

        RuleFor(x => x.PasswordMinLength)
            .InclusiveBetween(6, 32).WithMessage("PasswordMinLength must be between 6 and 32.");

        RuleFor(x => x.SessionTimeoutMinutes)
            .InclusiveBetween(5, 480).WithMessage("SessionTimeoutMinutes must be between 5 and 480.");
    }
}
