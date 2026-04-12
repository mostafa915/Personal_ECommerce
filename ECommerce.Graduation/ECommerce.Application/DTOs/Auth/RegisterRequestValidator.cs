using ECommerce.Application.DTOs.Auth.Regex;
using ECommerce.Application.Regex;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Auth
{
    public class RegisterRequestValidator : AbstractValidator<RegisterationRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(RegexPassword.Pass)
                .WithMessage("\"Invalid password. It must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character (e.g., !@#$)");
        }
    }
}
