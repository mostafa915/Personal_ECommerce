using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Products
{
    public record UpdateProductQuatityRequest(
        int QuantityAvailable
        );
}
