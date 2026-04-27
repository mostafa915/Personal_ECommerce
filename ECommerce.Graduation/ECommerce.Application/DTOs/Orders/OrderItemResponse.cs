using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Orders
{
    public record OrderItemResponse(
        int ProductId,
        string ProductName,
        decimal Price,
        int Quantity
        );
}
