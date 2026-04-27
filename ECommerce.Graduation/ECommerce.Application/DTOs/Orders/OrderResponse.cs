using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Orders
{
    public record OrderResponse(
        int OrderId,
        decimal TotalAmount,
        string Status,
        DateTime CreatedDate
        );
}
