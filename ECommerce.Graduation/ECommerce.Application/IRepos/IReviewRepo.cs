using ECommerce.Application.DTOs.Reviews;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.IRepos
{
    public interface IReviewRepo
    {
        Task<bool> IsUserBuyThisProuductBefore(string userId, int productId);
        Task<bool> IsUserReviewedBefore(string userId, int productId);
        Task<double> GetAvargaeRateAsync(int productId);
        Task<int> GetCountReviewAsync(int productId);
        Task<IEnumerable<UserReviewResponse>> GetReviwesAsync(int productId, CancellationToken cancellationToken);
        Task<Review?> GetReviewAsync(int id, string userId, string role, CancellationToken cancellationToken);
        Task<Review?> GetReviewAsync(int id, string userId, CancellationToken cancellationToken);
        Task AddAsync(Review review);
        public void Delete(Review review);
        void Update(Review review);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
