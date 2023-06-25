using AccountManager.Application.Extensions;
using AccountManager.Domain.Dto;
using FluentValidation;

namespace AccountManager.Application.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequestDTO>
    {
        public RegisterRequestValidator() 
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotNull().NotEmpty().Equal(x => x.ConfirmPassword).Password();
        }
    }
}
