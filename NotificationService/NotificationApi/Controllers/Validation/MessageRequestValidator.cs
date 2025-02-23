using FluentValidation;
using Notification.Api.Model;

namespace Notification.Api.Controllers.Validation
{
    public class MessageRequestValidator : AbstractValidator<SendMessageRequest>
    {
        public MessageRequestValidator()
        {
            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Body cannot be empty");
        }
    }
}
