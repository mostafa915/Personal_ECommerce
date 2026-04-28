using ECommerce.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Errors
{
    public static class ReviewsError
    {
        public static readonly Error NotEligibleForReview = new("Reviews.NotEligibleForReview", "You can only review products you have purchased and received", StatusCodes.Status403Forbidden);
        public static readonly Error ReviewAlreadyExists = new("Reviews.ReviewAlreadyExists", "You have already reviewed this product", StatusCodes.Status409Conflict);
        public static readonly Error ReviewNotFound = new("Reviews.NotFound", "Review is Not Found", StatusCodes.Status404NotFound);
    }
}
