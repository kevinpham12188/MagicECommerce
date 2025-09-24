using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Exceptions.Base;
using MagicECommerce_API.Exceptions.ProductException;
using MagicECommerce_API.Exceptions.ProductVariantException;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;

namespace MagicECommerce_API.Services
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductVariantRepository _variantRepo;
        private readonly ILogger<ProductVariantService> _logger;
        private readonly IProductRepository _productRepo;
        public ProductVariantService(IProductVariantRepository variantRepo, ILogger<ProductVariantService> logger, IProductRepository productRepo)
        {
            _variantRepo = variantRepo;
            _logger = logger;
            _productRepo = productRepo;
        }

        #region Public Methods
        public async Task<ProductVariantResponseDto> CreateVariantAsync(ProductVariantRequestDto dto)
        {
            //Validation
            if(dto == null)
            {
                throw new ValidationException("Invalid product variant data");
            }
            if(dto.ProductId == Guid.Empty)
            {
                throw new ValidationException("Product ID is required");
            }
            if(string.IsNullOrWhiteSpace(dto.VariantName))
            {
                throw new ValidationException("Variant name is required");
            }
            if(string.IsNullOrWhiteSpace(dto.VariantValue))
            {
                throw new ValidationException("Variant value is required");
            }
            if(dto.VariantName.Length > 50)
            {
                throw new ValidationException("Variant name cannot exceed 50 characters");
            }
            if(dto.VariantValue.Length > 50)
            {
                throw new ValidationException("Variant value cannot exceed 50 characters");
            }
            if(dto.StockQuantity < 0)
            {
                throw new ValidationException("Stock quantity cannot be negative");
            }
            //Check if product exists
            if(!await _variantRepo.ProductExistAsync(dto.ProductId))
            {
                throw new ProductNotFoundException(dto.ProductId);
            }
            //Check for duplicate variant
            var existingVariant = await _variantRepo.GetVariantByProductAndNameValueAsync(dto.ProductId, dto.VariantName.Trim(), dto.VariantValue.Trim());
            if(existingVariant != null)
            {
                throw new ValidationException($"Variant '{dto.VariantName}: {dto.VariantValue}' already exists for this product");
            }
            var variant = new ProductVariant
            {
                ProductId = dto.ProductId,
                VariantName = dto.VariantName.Trim(),
                VariantValue = dto.VariantValue.Trim(),
                StockQuantity = dto.StockQuantity
            };

            var createdVariant = await _variantRepo.CreateVariantAsync(variant);
            await UpdateProductTotalStockAsync(dto.ProductId);
            _logger.LogInformation("Created new variant with id {VariantId} for product {ProductId}", createdVariant.Id, createdVariant.ProductId);
            return MapToResponseDto(createdVariant);
        }

        public async Task<bool> DeleteVariantAsync(Guid id)
        {
            //Validate ID
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid variant ID");
            }
            var existingVariant = await _variantRepo.GetVariantByIdAsync(id);
            if(existingVariant == null)
            {
                throw new ProductVariantNotFoundException(id);
            }
            var result = await _variantRepo.DeleteVariantAsync(id);
            if(result)
            {
                await UpdateProductTotalStockAsync(existingVariant.ProductId);
                _logger.LogInformation("Deleted variant with id {VariantId}", id);
            }
            return result;
        }

        public async Task<bool> DeleteVariantsByProductIdAsync(Guid productId)
        {
            //Validate Product ID
            if(productId == Guid.Empty)
            {
                throw new ValidationException("Invalid product ID");
            }
            var productExists = await _variantRepo.ProductExistAsync(productId);
            if(!productExists)
            {
                throw new ProductNotFoundException(productId);
            }
            var existingVariants = await _variantRepo.GetVariantsByProductIdAsync(productId);
            if(existingVariants == null || !existingVariants.Any())
            {
                _logger.LogInformation("No variants found for product with id {ProductId}", productId);
                return true;
            }
            var result = await _variantRepo.DeleteVariantsByProductIdAsync(productId);
            if(result)
            {
                await UpdateProductTotalStockAsync(productId);
                _logger.LogInformation("Deleted all variants for product with id {ProductId}", productId);
            }
            return result;
        }

        public async Task<IEnumerable<ProductVariantResponseDto>> GetAllVariantsAsync()
        {
            var variants =  await _variantRepo.GetAllVariantAsync();
            return variants.Select(MapToResponseDto).ToList();
        }

        public async Task<ProductVariantResponseDto> GetVariantByIdAsync(Guid id)
        {
            //Validate ID
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid variant ID");
            }
            var variant = await _variantRepo.GetVariantByIdAsync(id);
            if(variant == null)
            {
                throw new ProductVariantNotFoundException(id);
            }
            return MapToResponseDto(variant);
        }

        public async Task<IEnumerable<ProductVariantResponseDto>> GetVariantsByProductIdAsync(Guid productId)
        {
            //Validate Product ID
            if(productId == Guid.Empty)
            {
                throw new ValidationException("Invalid product ID");
            }
            //Check if product exists
            var productExists = await _variantRepo.ProductExistAsync(productId);
            if(!productExists)
            {
                throw new ProductNotFoundException(productId);
            }
            var variants = await _variantRepo.GetVariantsByProductIdAsync(productId);
            if(variants == null || !variants.Any())
            {
                return new List<ProductVariantResponseDto>();
            }
            return variants.Select(MapToResponseDto).ToList();
        }

        public async Task<ProductVariantResponseDto> UpdateVariantAsync(Guid id, ProductVariantRequestDto dto)
        {
            //Validation
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid variant ID");
            }
            //Validate dto
            if (dto == null)
            {
                throw new ValidationException("Invalid product variant data");
            }
            if (dto.ProductId == Guid.Empty)
            {
                throw new ValidationException("Product ID is required");
            }
            if (string.IsNullOrWhiteSpace(dto.VariantName))
            {
                throw new ValidationException("Variant name is required");
            }
            if (string.IsNullOrWhiteSpace(dto.VariantValue))
            {
                throw new ValidationException("Variant value is required");
            }
            if (dto.StockQuantity < 0)
            {
                throw new ValidationException("Stock quantity cannot be negative");
            }
            //Check if product exists
            if (!await _variantRepo.ProductExistAsync(dto.ProductId))
            {
                throw new ProductNotFoundException(dto.ProductId);
            }
            // Check if variant exists
            var existingVariant = await _variantRepo.GetVariantByIdAsync(id);
            if (existingVariant == null)
            {
                throw new ProductVariantNotFoundException(id);
            }

            // Check if new product exists (if changing product)
            if (existingVariant.ProductId != dto.ProductId)
            {
                throw new ValidationException("Changing the product of a variant is not allowed");
            }

            // Check for duplicate variant (only if name/value changed)
            if (existingVariant.VariantName.Trim() != dto.VariantName.Trim() ||
                existingVariant.VariantValue.Trim() != dto.VariantValue.Trim())
            {
                var duplicateVariant = await _variantRepo.GetVariantByProductAndNameValueAsync(
                    dto.ProductId, dto.VariantName.Trim(), dto.VariantValue.Trim());
                if (duplicateVariant != null && duplicateVariant.Id != id)
                {
                    throw new ValidationException($"Variant '{dto.VariantName}: {dto.VariantValue}' already exists for this product");
                }
            }
            //existingVariant.ProductId = dto.ProductId;
            existingVariant.VariantName = dto.VariantName.Trim();
            existingVariant.VariantValue = dto.VariantValue.Trim();
            existingVariant.StockQuantity = dto.StockQuantity;
            var updatedVariant = await _variantRepo.UpdateVariantAsycn(existingVariant);
            return MapToResponseDto(updatedVariant);
        }

        #endregion

        #region Private Helpers
        private static ProductVariantResponseDto MapToResponseDto (ProductVariant productVariant)
        {
            return new ProductVariantResponseDto
            {
                Id = productVariant.Id,
                ProductId = productVariant.ProductId,
                VariantName = productVariant.VariantName,
                VariantValue = productVariant.VariantValue,
                StockQuantity = productVariant.StockQuantity,
                CreatedAt = productVariant.CreatedAt,
                UpdatedAt = productVariant.UpdatedAt
            };
        }

        private async Task UpdateProductTotalStockAsync(Guid productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product != null)
            {
                var variants = await _variantRepo.GetVariantsByProductIdAsync(productId);
                var totalVariantStock = variants.Sum(v => v.StockQuantity);

                // Update product total stock to sum of variant stocks
                product.StockQuantity = totalVariantStock;
                await _productRepo.UpdateAsync(product);

                _logger.LogInformation("Updated product {ProductId} total stock to {TotalStock}",
                    productId, totalVariantStock);
            }
        }
        #endregion
    }
}
