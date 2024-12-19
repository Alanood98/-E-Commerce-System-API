using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IReviewRepo
    {
        void AddReview(Review review);
        Review GetReview(int reviewId);
        IEnumerable<Review> GetAllReviewsForProduct(int productId, int page, int pageSize);
        void UpdateReview(Review review);
        void DeleteReview(Review review);
        decimal RecalculateProductRating(int productId);
    }
}
