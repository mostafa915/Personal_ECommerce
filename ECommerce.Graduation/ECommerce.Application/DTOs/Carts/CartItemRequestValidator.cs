using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Carts
{
    public class CartItemRequestValidator : AbstractValidator<CartItemRequest>
    {
        public CartItemRequestValidator()
        {
            
            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}
