using Dotnet_Base_Backend.Common.Errors;
using Dotnet_Base_Backend.DTO;
using FluentValidation;
using FluentValidation.Validators;

namespace Dotnet_Base_Backend.API.Validators
{
    public class MessageDTOValidator: AbstractValidator<MessageDto>
    {
        public MessageDTOValidator(bool isIdRequired = false)
        {
            RuleFor(x => x.Message)
                .NotNull().WithErrorCode(((int)ErrorCode.REQUIRED).ToString())
                .NotEmpty().WithErrorCode(((int)ErrorCode.EMPTY).ToString())
                .MaximumLength(20).WithErrorCode(((int)ErrorCode.MAX_LENGTH).ToString());

            if (isIdRequired)
            {
                RuleFor(x => x.Id)
                    .NotNull().WithErrorCode(((int)ErrorCode.REQUIRED).ToString())
                    .GreaterThan(0).WithErrorCode(((int)ErrorCode.INVALID).ToString());
            }
        }
    }
}
