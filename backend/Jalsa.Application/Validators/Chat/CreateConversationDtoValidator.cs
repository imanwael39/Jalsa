using FluentValidation;
using Jalsa.Application.DTOs.Chat;

namespace Jalsa.Application.Validators.Chat;

public class CreateConversationDtoValidator : AbstractValidator<CreateConversationDto>
{
    public CreateConversationDtoValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty().WithMessage("PatientId is required.");
    }
}
