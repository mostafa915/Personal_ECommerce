using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public interface IReviewService
    {
        Task<Result<ReviewResponse>> ReviewAsync(int productId, string userId, ReviewRequest request, CancellationToken cancellationToken = default);
        Task<Result<ReviewDetailsResponse>> GetReviewsProductAsync(int productId, CancellationToken cancellationToken = default);
        Task<Result> RemoveAsync(int id, string userId, string role, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, string userId, ReviewRequest request, CancellationToken cancellationToken = default);
    }
}
