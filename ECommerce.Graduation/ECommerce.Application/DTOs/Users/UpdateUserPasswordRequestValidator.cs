using ECommerce.Application.DTOs.Auth.Regex;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Users
{
    public class UpdateUserPasswordRequestValidator : AbstractValidator<UpdateUserPasswordRequest>
    {
        public UpdateUserPasswordRequestValidator() 
        {
            RuleFor(x => x.OldPass)
                .NotEmpty();

            RuleFor(x => x.NewPass)
                .NotEmpty()
                .Matches(RegexPassword.Pass)
                .WithMessage("\"Invalid password. It must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character (e.g., !@#$)");

        }
    }
}
