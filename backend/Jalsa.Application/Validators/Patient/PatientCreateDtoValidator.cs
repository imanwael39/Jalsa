using FluentValidation;
using Jalsa.Application.DTOs.Patient;

namespace Jalsa.Application.Validators.Patient;

public class PatientCreateDtoValidator : AbstractValidator<PatientCreateDto>
{
    public PatientCreateDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("FullName is required.")
            .MaximumLength(200).WithMessage("FullName must not exceed 200 characters.");

        RuleFor(x => x.DateOfBirth)
            .Must(d => !d.HasValue || d.Value <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("DateOfBirth cannot be in the future.");

        RuleFor(x => x.Gender)
            .MaximumLength(20).WithMessage("Gender must not exceed 20 characters.")
            .When(x => x.Gender is not null);

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Phone must not exceed 20 characters.")
            .When(x => x.Phone is not null);

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(200).WithMessage("Email must not exceed 200 characters.")
            .When(x => x.Email is not null);

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.")
            .When(x => x.Address is not null);

        RuleFor(x => x.ReferralSource)
            .MaximumLength(200).WithMessage("ReferralSource must not exceed 200 characters.")
            .When(x => x.ReferralSource is not null);

        RuleFor(x => x.ChiefComplaint)
            .MaximumLength(1000).WithMessage("ChiefComplaint must not exceed 1000 characters.")
            .When(x => x.ChiefComplaint is not null);

        RuleFor(x => x.EmergencyContactName)
            .MaximumLength(200).WithMessage("EmergencyContactName must not exceed 200 characters.")
            .When(x => x.EmergencyContactName is not null);

        RuleFor(x => x.EmergencyContactRelationship)
            .MaximumLength(100).WithMessage("EmergencyContactRelationship must not exceed 100 characters.")
            .When(x => x.EmergencyContactRelationship is not null);

        RuleFor(x => x.EmergencyContactPhone)
            .MaximumLength(30).WithMessage("EmergencyContactPhone must not exceed 30 characters.")
            .When(x => x.EmergencyContactPhone is not null);

        RuleFor(x => x.TreatmentStartDate)
            .Must(d => !d.HasValue || d.Value <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("TreatmentStartDate cannot be in the future.");
    }
}
