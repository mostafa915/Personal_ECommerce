using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Carts
{
    public record CartResponse(
        int Id,
        IList<CartItemResponse> Items,
        decimal TotalPrice
        );
}
