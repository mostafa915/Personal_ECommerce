using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Reviews
{
    public record ReviewDetailsResponse(
        int ProductId,
        double AvarageRating,
        int TotalReviews,
        IEnumerable<UserReviewResponse>  Reviews
        );
}
