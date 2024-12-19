using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IReviewService
    {
        void AddReview(Review review, int userId);
        IEnumerable<Review> GetAllReviewsForProduct(int productId, int page, int pageSize);
        void UpdateReview(Review review, int userId);
        void DeleteReview(int reviewId, int userId, string role);
    }
}
