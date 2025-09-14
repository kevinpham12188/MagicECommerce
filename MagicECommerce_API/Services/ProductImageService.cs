using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Exceptions.Base;
using MagicECommerce_API.Exceptions.ProductException;
using MagicECommerce_API.Exceptions.ProductImageException;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace MagicECommerce_API.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly ILogger<ProductImageService> _logger;
        public ProductImageService(IProductImageRepository productImageRepository, ILogger<ProductImageService> logger)
        {
            _productImageRepository = productImageRepository;
            _logger = logger;
        }

        #region Public Methods
        public async Task<ProductImageResponseDto> CreateProductImageAsync(ProductImageRequestDto dto)
        {
            //Validation
            if(dto == null)
            {
                throw new ValidationException("Invalid product image data");
            }

            if(dto.ProductId == Guid.Empty)
            {
                throw new ValidationException("Product ID is required");
            }

            if(string.IsNullOrWhiteSpace(dto.Url))
            {
                throw new ValidationException("Image URL is required");
            }

            //Process and validate Url
            var url = dto.Url.Trim();
            if(url.Length > 500)
            {
                throw new ValidationException("Image URL cannot exceed 500 characters");
            }

            if(!Uri.TryCreate(url, UriKind.Absolute, out var valiUri) ||
                (valiUri.Scheme != Uri.UriSchemeHttp && valiUri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ValidationException("Image URL must be a valid HTTP or HTTPS URL");
            }

            //Validate AltText
            var altText = dto.AltText?.Trim() ?? string.Empty;
            if(altText.Length > 255)
            {
                throw new ValidationException("Alt text cannot exceed 255 characters");
            }

            //Validate product exists
            if(!await _productImageRepository.ProductExistsAsync(dto.ProductId))
            {
                _logger.LogWarning("Product with id {ProductId} does not exist", dto.ProductId);
                throw new ProductNotFoundException(dto.ProductId);
            }
            var productImage = new ProductImage
            {
                Url = dto.Url,
                AltText = dto.AltText,
                ProductId = dto.ProductId,
            };
            var createdProductImage = await _productImageRepository.CreateProductImageAsync(productImage);
            _logger.LogInformation("Product image with id {ProductImageId} created successfully", createdProductImage.Id);
            return MapToResponseDto(createdProductImage);
        }


        public async Task<bool> DeleteProductImageAsync(Guid id)
        {
            //Validate ID
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid product image ID");
            }

            var existingProductImage = await _productImageRepository.GetProductImageByIdAsync(id);
            if(existingProductImage == null)
            {
                throw new ProductImageNotFoundException(id);
            }
            var result = await _productImageRepository.DeleteProductImageAsync(id);
            if(result)
            {
                _logger.LogInformation("Product Image deleted successfully: {ProductImageId}", id);
            }
            return result;
        }

        public async Task<bool> DeleteProductImagesByProductIdAsync(Guid productId)
        {
            //Validate ID
            if (productId == Guid.Empty) 
            {
                throw new ValidationException("Invalid product ID");
            }

            //Check if product exists
            if (!await _productImageRepository.ProductExistsAsync(productId))
            {
                throw new ProductNotFoundException(productId);
            }
            var result = await _productImageRepository.DeleteProductImagesByProductIdAsync(productId);
            if(result)
            {
                _logger.LogInformation("Product image deleted successfully for product {ProductId}", productId);
            }
            return result;
        }

        public async Task<IEnumerable<ProductImageResponseDto>> GetAllProductImagesAsync()
        {
            var productImages = await _productImageRepository.GetAllProductImageAsync();
            return productImages.Select(MapToResponseDto);
        }

        public async Task<ProductImageResponseDto> GetProductImageByIdAsync(Guid id)
        {
            //Validate ID
            if (id == Guid.Empty)
            {
                throw new ValidationException("Invalid product image ID");
            }
            var productImage = await _productImageRepository.GetProductImageByIdAsync(id);
            if(productImage == null)
            {
                throw new ProductImageNotFoundException(id);
            }
            return MapToResponseDto(productImage);
        }

        public async Task<IEnumerable<ProductImageResponseDto>> GetProductImagesByProductIdAsync(Guid productId)
        {
            //Validate ID
            if (productId == Guid.Empty)
            {
                throw new ValidationException("Invalid product ID");
            }

            //Check if product exists
            if (!await _productImageRepository.ProductExistsAsync(productId))
            {
                throw new ProductNotFoundException(productId);
            }

            var productImages = await _productImageRepository.GetProductImagesByProductIdAsync(productId);
            return productImages.Select(MapToResponseDto);
        }

        public async Task<ProductImageResponseDto> UpdateProductImageAsync(Guid id, ProductImageRequestDto dto)
        {
            //Validation
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid product image ID");
            }
            if(dto == null)
            {
                throw new ValidationException("Invalid product image data");
            }

            //Validate if product image exists
            var existingProductImage = await _productImageRepository.GetProductImageByIdAsync(id);
            if(existingProductImage == null)
            {
                throw new ProductImageNotFoundException(id);
            }

            //Required fields
            if(dto.ProductId == Guid.Empty)
            {
                throw new ValidationException("Product ID is required");
            }
            if(string.IsNullOrWhiteSpace(dto.Url))
            {
                throw new ValidationException("Image URL is required");
            }

            //Validate Url
            var url = dto.Url.Trim();
            if (url.Length > 500)
            {
                throw new ValidationException("Image URL cannot exceed 500 characters");
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var valiUri) ||
                (valiUri.Scheme != Uri.UriSchemeHttp && valiUri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ValidationException("Image URL must be a valid HTTP or HTTPS URL");
            }

            //Validate AltText
            var altText = dto.AltText?.Trim() ?? string.Empty;
            if (altText.Length > 255)
            {
                throw new ValidationException("Alt text cannot exceed 255 characters");
            }

            existingProductImage.Url = url;
            existingProductImage.AltText = altText;
            existingProductImage.UpdatedAt = DateTime.UtcNow;

            var updatedProductImage = await _productImageRepository.UpdateProductImageAsync(existingProductImage);
            _logger.LogInformation("Product image with id {ProductImageId} updated successfully", updatedProductImage.Id);
            return MapToResponseDto(updatedProductImage);
        }

        #endregion

        #region Private Helpers
        private static ProductImageResponseDto MapToResponseDto(ProductImage productImage)
        {
            return new ProductImageResponseDto
            {
                Id = productImage.Id,
                Url = productImage.Url,
                AltText = productImage.AltText,
                CreatedAt = productImage.CreatedAt,
                UpdatedAt = productImage.UpdatedAt,
                ProductId = productImage.ProductId
            };
        }
        #endregion
    }
}
