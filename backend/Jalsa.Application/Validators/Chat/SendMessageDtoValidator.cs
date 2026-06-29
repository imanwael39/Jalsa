using FluentValidation;
using Jalsa.Application.DTOs.Chat;

namespace Jalsa.Application.Validators.Chat;

public class SendMessageDtoValidator : AbstractValidator<SendMessageDto>
{
    public SendMessageDtoValidator()
    {
        RuleFor(x => x.ConversationId)
            .NotEmpty().WithMessage("ConversationId is required.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(10000).WithMessage("Content must not exceed 10000 characters.");
    }
}
