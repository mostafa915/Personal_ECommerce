using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Products
{
    public class UpdateProductQuatityRequestVaildator : AbstractValidator<UpdateProductQuatityRequest>
    {
        public UpdateProductQuatityRequestVaildator()
        {
            RuleFor(x => x.QuantityAvailable)
                .GreaterThanOrEqualTo(0);
        }
    }
}
