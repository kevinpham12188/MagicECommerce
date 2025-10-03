using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;

namespace MagicECommerce_API.Services.Interfaces
{
    public interface IProductReviewService
    {
        Task<IEnumerable<ProductReviewResponseDto>> GetAllReviewsAsync();
        Task<IEnumerable<ProductReviewResponseDto>> GetReviewsByProductIdAsync(Guid productId);
        Task<IEnumerable<ProductReviewResponseDto>> GetReviewsByUserIdAsync(Guid userId);
        Task<ProductReviewResponseDto?> GetReviewByIdAsync(Guid id);
        Task<ProductReviewResponseDto> CreateReviewAsync(Guid userId, ProductReviewRequestDto reviewDto);
        Task<ProductReviewResponseDto> UpdateReviewAsync(Guid id, Guid userId, ProductReviewRequestDto reviewDto);
        Task<bool> DeleteReviewAsync(Guid id, Guid userId);
        Task<bool> AnonymizeUserReviewsAsync(Guid userId);
        Task<ProductRatingStatsDto> GetProductRatinStatsAsync(Guid productId);
    }
}
