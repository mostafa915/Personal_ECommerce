using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Products
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(3, 250);

            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(3, 1000);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            
        }
    }
}
