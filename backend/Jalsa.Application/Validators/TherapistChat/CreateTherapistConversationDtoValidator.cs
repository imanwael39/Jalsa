using FluentValidation;
using Jalsa.Application.DTOs.TherapistChat;

namespace Jalsa.Application.Validators.TherapistChat;

public class CreateTherapistConversationDtoValidator : AbstractValidator<CreateTherapistConversationDto>
{
    public CreateTherapistConversationDtoValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("PatientId is required.");
    }
}
