using E_CommerceSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace E_CommerceSystem.Repositories
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
        }

        public Review GetReview(int reviewId)
        {
            return _context.Reviews.FirstOrDefault(r => r.RId == reviewId);
        }

        public IEnumerable<Review> GetAllReviewsForProduct(int productId, int page, int pageSize)
        {
            return _context.Reviews
                .Where(r => r.ProductId == productId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public void UpdateReview(Review review)
        {
            _context.Reviews.Update(review);
            _context.SaveChanges();
        }

        public void DeleteReview(Review review)
        {
            _context.Reviews.Remove(review);
            _context.SaveChanges();
        }

        public decimal RecalculateProductRating(int productId)
        {
            var reviews = _context.Reviews.Where(r => r.ProductId == productId).ToList();
            if (reviews.Any())
            {
                var overallRating = (decimal)reviews.Average(r => (double)r.Rating); // Cast to decimal
                var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    product.OverallRating = overallRating; // Use the calculated rating
                    _context.Products.Update(product);
                    _context.SaveChanges();
                }
                return overallRating;
            }
            return 0;
        }
    }
}