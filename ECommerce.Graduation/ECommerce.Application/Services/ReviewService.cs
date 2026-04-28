using ECommerce.Application.Abstractions;
using ECommerce.Application.DTOs.Reviews;
using ECommerce.Application.Errors;
using ECommerce.Application.IRepos;
using ECommerce.Domain.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class ReviewService(IReviewRepo reviewRepo) : IReviewService
    {
        private readonly IReviewRepo _reviewRepo = reviewRepo;

        public async Task<Result<ReviewResponse>> ReviewAsync(int productId, string userId, ReviewRequest request, CancellationToken cancellationToken = default)
        {
            var IsUserDeliverdProduct = await _reviewRepo.IsUserBuyThisProuductBefore(userId, productId);
            if (!IsUserDeliverdProduct)
                return Result.Faliuar<ReviewResponse>(ReviewsError.NotEligibleForReview);

            var IsUserReviewed = await _reviewRepo.IsUserReviewedBefore(userId, productId);
            if (IsUserReviewed)
                return Result.Faliuar<ReviewResponse>(ReviewsError.ReviewAlreadyExists);

            var review = request.Adapt<Review>();
            review.UserId = userId;
            review.ProductId = productId;
            await _reviewRepo.AddAsync(review);
            await _reviewRepo.SaveChangesAsync(cancellationToken);
            return Result.Success(review.Adapt<ReviewResponse>());
        }

        public async Task<Result<ReviewDetailsResponse>> GetReviewsProductAsync(int productId, CancellationToken cancellationToken = default)
        {
            var avg = await _reviewRepo.GetAvargaeRateAsync(productId);
            var total = await _reviewRepo.GetCountReviewAsync(productId);
            var usersReviews = await _reviewRepo.GetReviwesAsync(productId, cancellationToken);

            var response = new ReviewDetailsResponse(productId, avg, total, usersReviews);
            return Result.Success(response);

        }

        public async Task<Result> RemoveAsync(int id, string userId, string role, CancellationToken cancellationToken = default)
        {
            if (await _reviewRepo.GetReviewAsync(id, userId, role, cancellationToken) is not { } review)
                return Result.Faliuar(ReviewsError.ReviewNotFound);

            _reviewRepo.Delete(review);
            await _reviewRepo.SaveChangesAsync(cancellationToken);
            return Result.Success();    

        }

        public async Task<Result> UpdateAsync(int id, string userId, ReviewRequest request, CancellationToken cancellationToken = default)
        {
            if(await _reviewRepo.GetReviewAsync(id, userId, cancellationToken) is not { } review)
                return Result.Faliuar(ReviewsError.ReviewNotFound);

            review.Comment = request.Comment;
            review.Rating = request.Rating;
            review.UpdatedAt = DateTime.UtcNow;

            _reviewRepo.Update(review);
            await _reviewRepo.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }
    }
}
