using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Orders
{
    public record OrderAllResonse(
        string CustomerName,
        int OrderId,
        string Status,
        decimal TotalAmount,
        IList<OrderItemResponse> Items
        );
}
