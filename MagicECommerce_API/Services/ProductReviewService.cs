using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Exceptions.Base;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;

namespace MagicECommerce_API.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly ILogger<ProductReviewService> _logger;
        private readonly IProductReviewRepository _reviewRepo;
        public ProductReviewService(ILogger<ProductReviewService> logger, IProductReviewRepository reviewRepo)
        {
            _logger = logger;
            _reviewRepo = reviewRepo;
        }

        #region Public Methods

        public Task<bool> AnonymizeUserReviewsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductReviewResponseDto> CreateReviewAsync(Guid userId, ProductReviewRequestDto reviewDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteReviewAsync(Guid id, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductReviewResponseDto>> GetAllReviewsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductRatingStatsDto> GetProductRatinStatsAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductReviewResponseDto?> GetReviewByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductReviewResponseDto>> GetReviewsByProductIdAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductReviewResponseDto>> GetReviewsByUserIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductReviewResponseDto> UpdateReviewAsync(Guid id, Guid userId, ProductReviewRequestDto reviewDto)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods
        private ProductReviewResponseDto MapToResponseDto(ProductReview productReview)
        {
            string displayName;
            if(productReview.IsAnonymous || productReview.UserId == null)
            {
                displayName = productReview.DisplayName;
            }
            else if (productReview.User != null) 
            {
                displayName = $"{productReview.User.FirstName} {productReview.User.LastName}".Trim();
                if(displayName == null)
                {
                    displayName = "Customer";
                }
            }
            else
            {
                displayName = "Customer";
            }
            return new ProductReviewResponseDto
            {
                Id = productReview.Id,
                ProductId = productReview.ProductId,
                UserId = productReview.UserId,
                DisplayName = productReview.DisplayName,
                IsAnonymous = productReview.IsAnonymous,
                Rating = productReview.Rating,
                Comment = productReview.Comment,
                CreatedAt = productReview.CreatedAt,
                UpdatedAt = productReview.UpdatedAt
            };
        }
        private void ValidateReviewRequest(ProductReviewRequestDto reviewDto)
        {
            if(reviewDto.ProductId == Guid.Empty)
            {
                throw new ValidationException("Product ID is required");
            }
            if(reviewDto.Rating < 1 || reviewDto.Rating > 5)
            {
                throw new ValidationException("Rating must be between 1 and 5");
            }
            if(!string.IsNullOrWhiteSpace(reviewDto.Comment) && reviewDto.Comment.Length > 1000)
            {
                throw new ValidationException("Commen cannot exceed 1000 characters");
            }
        }
        #endregion
    }
}
