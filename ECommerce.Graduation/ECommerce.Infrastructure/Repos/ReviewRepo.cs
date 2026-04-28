using ECommerce.Application.DTOs.Reviews;
using ECommerce.Application.IRepos;
using ECommerce.Domain.Models;
using ECommerce.Infrastructure.Configuration;
using ECommerce.Infrastructure.Seeding;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repos
{
    public class ReviewRepo(ApplicationDbContext context) : IReviewRepo
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<bool> IsUserBuyThisProuductBefore(string userId, int productId) =>
            await _context.OrderItems
            .AnyAsync(x => x.Order.CreatedById == userId && x.ProductId == productId && x.Order.OrderStatus == DeliveryTracking.Delivered);

        public async Task<bool> IsUserReviewedBefore(string userId, int productId) =>
            await _context.Reviews.AnyAsync(x => x.UserId == userId && x.ProductId == productId);


        public async Task<double> GetAvargaeRateAsync(int productId) =>
            await _context.Reviews.Where(x => x.ProductId == productId).AverageAsync(x => (double?)x.Rating) ?? 0;

        public async Task<int> GetCountReviewAsync(int productId) =>
            await _context.Reviews.CountAsync(x => x.ProductId == productId);


        public async Task<IEnumerable<UserReviewResponse>> GetReviwesAsync(int productId, CancellationToken cancellationToken) =>
            await _context.Reviews
            .Where(x => x.ProductId == productId)
            .ProjectToType<UserReviewResponse>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);


        public async Task<Review?> GetReviewAsync(int id, string userId, string role, CancellationToken cancellationToken)
        {
            if (role == DefaultRoles.Customer)
                return await _context.Reviews.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

            return await _context.Reviews.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Review?> GetReviewAsync(int id, string userId, CancellationToken cancellationToken) =>
            await _context.Reviews.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId, cancellationToken);

        public void Delete(Review review) =>
            _context.Reviews.Remove(review);

        public void Update(Review review) =>
            _context.Update(review);

        public async Task AddAsync(Review review) =>
            await _context.AddAsync(review);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken) => 
            await _context.SaveChangesAsync();
    }
}
