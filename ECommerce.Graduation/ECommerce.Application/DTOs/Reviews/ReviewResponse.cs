using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Reviews
{
    public record ReviewResponse(
        int Id,
        int ProductId,
        int Rating,
        string Comment,
        DateTime CreatedAt
        );
}
