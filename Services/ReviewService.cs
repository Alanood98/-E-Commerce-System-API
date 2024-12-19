using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace E_CommerceSystem.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepo _reviewRepo;
        private readonly IOrderRepo _orderRepo;

        public ReviewService(IReviewRepo reviewRepo, IOrderRepo orderRepo)
        {
            _reviewRepo = reviewRepo;
            _orderRepo = orderRepo;
        }

        public void AddReview(Review review, int userId)
        {
            var orders = _orderRepo.GetAllOrdersForUser(userId);
            if (!orders.Any(o => o.OrderProducts.Any(op => op.ProductId == review.ProductId)))
            {
                throw new InvalidOperationException("User cannot review a product they have not purchased.");
            }

            var existingReview = _reviewRepo.GetAllReviewsForProduct(review.ProductId, 1, int.MaxValue)
                .FirstOrDefault(r => r.UId == userId);

            if (existingReview != null)
            {
                throw new InvalidOperationException("User cannot review the same product more than once.");
            }

            if (review.Rating < 1 || review.Rating > 5)
            {
                throw new InvalidOperationException("Rating must be between 1 and 5.");
            }

            _reviewRepo.AddReview(review);
            _reviewRepo.RecalculateProductRating(review.ProductId);
        }

        public IEnumerable<Review> GetAllReviewsForProduct(int productId, int page, int pageSize)
        {
            return _reviewRepo.GetAllReviewsForProduct(productId, page, pageSize);
        }

        public void UpdateReview(Review review, int userId)
        {
            var existingReview = _reviewRepo.GetReview(review.RId);
            if (existingReview == null || existingReview.UId != userId)
            {
                throw new InvalidOperationException("User can only update their own reviews.");
            }

            if (review.Rating < 1 || review.Rating > 5)
            {
                throw new InvalidOperationException("Rating must be between 1 and 5.");
            }

            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;
            _reviewRepo.UpdateReview(existingReview);
            _reviewRepo.RecalculateProductRating(review.ProductId);
        }

        public void DeleteReview(int reviewId, int userId, string role)
        {
            if (role != "User")
            {
                throw new InvalidOperationException("Only users can delete their own reviews.");
            }

            var review = _reviewRepo.GetReview(reviewId);
            if (review == null || review.UId != userId)
            {
                throw new InvalidOperationException("User can only delete their own reviews.");
            }

            _reviewRepo.DeleteReview(review);
            _reviewRepo.RecalculateProductRating(review.ProductId);
        }
    }
}
