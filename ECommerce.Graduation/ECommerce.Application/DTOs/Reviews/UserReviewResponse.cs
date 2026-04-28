using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Reviews
{
    public record UserReviewResponse(
        string UserName,
        int Rating,
        string Comment,
        DateTime CreatedAt
        );
}
