using MagicECommerce_API.Models;

namespace MagicECommerce_API.Repositories.Interfaces
{
    public interface IProductReviewRepository
    {
        Task<IEnumerable<ProductReview>> GetAllReviewsAsync();
        Task<IEnumerable<ProductReview>> GetReviewsByProductIdAsync(Guid productId);
        Task<IEnumerable<ProductReview>> GetReviewsByUserIdAsync(Guid userId);
        Task<ProductReview?> GetReviewByIdAsync(Guid id);
        Task<ProductReview> CreateReviewAsync(ProductReview review);
        Task<ProductReview> UpdateReviewAsync(ProductReview review);
        Task<bool> DeleteReviewAsync(Guid id);
        //Task<bool> ProductExistsAsync(Guid productId);
        //Task<bool> UserExistsAsync(Guid userId);
        Task<bool> UserHasReviewedProductAsync(Guid userId, Guid productId);
        Task<double> GetAverageRatingForProductAsync(Guid productId);
        Task<int> GetReviewCountForProductAsync(Guid productId);
        Task<bool> AnonymizeUserReviewsAsync(Guid userId);
        Task<Dictionary<int, int>> GetRatingDistributionForProductAsync(Guid productId);
    }
}
