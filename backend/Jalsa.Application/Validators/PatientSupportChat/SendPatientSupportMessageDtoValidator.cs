using FluentValidation;
using Jalsa.Application.DTOs.PatientSupportChat;

namespace Jalsa.Application.Validators.PatientSupportChat;

public class SendPatientSupportMessageDtoValidator : AbstractValidator<SendPatientSupportMessageDto>
{
    public SendPatientSupportMessageDtoValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty().WithMessage("ConversationId is required.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(10000).WithMessage("Content must not exceed 10000 characters.");
    }
}
