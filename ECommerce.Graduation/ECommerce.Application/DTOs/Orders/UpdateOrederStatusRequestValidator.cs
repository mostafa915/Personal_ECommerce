using ECommerce.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Orders
{
    public class UpdateOrederStatusRequestValidator : AbstractValidator<UpdateOrederStatusRequest>
    {
        public UpdateOrederStatusRequestValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty();
        }
    }
}
