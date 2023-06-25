using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Application.Extensions
{
    public static class RuleBuilderExtension
    {
        public static void Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minLength = 8, int maxLength = 16)
        {
            ruleBuilder
                .MinimumLength(minLength).WithMessage($"Minimum length of the password is {minLength}")
                .MaximumLength(maxLength).WithMessage($"Maximum length of the password is {maxLength}")
                .Matches("[a-z]").WithMessage("Password need lower case letter a-z")
                .Matches("[A-Z]").WithMessage("Password need upper case letter A-Z")
                .Matches("[0-9]").WithMessage("Password need at least one number 0-9")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password need at least one special character");
        }
    }
}
