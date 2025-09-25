using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Exceptions.Base;
using MagicECommerce_API.Exceptions.CategoryException;
using MagicECommerce_API.Exceptions.ProductException;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;
using System.Runtime.InteropServices;
using System.Xml;

namespace MagicECommerce_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IProductRepository repo, ICategoryService categoryService, ILogger<ProductService> logger)
        {
            _repo = repo;
            _categoryService = categoryService;
            _logger = logger;
        }

        #region Public Methods
        public async Task<ProductResponseDto> CreateAsync(ProductRequestDto dto)
        {
            if(dto == null)
            {
                throw new ValidationException("Invalid product data");
            }
            if(string.IsNullOrWhiteSpace(dto.Name)) {
                throw new ValidationException("Product name is required");
            }
            if(dto.Price <= 0)
            {
                throw new ValidationException("Product price must be greater than 0");
            }
            if(dto.StockQuantity < 0)
            {
                throw new ValidationException("Stock quantity cannot be negative");
            }
            if (dto.CategoryId == Guid.Empty) 
            {
                throw new ValidationException("Category ID is required");
            }
            var categoryDto = await _categoryService.GetCategoryByIdAsync(dto.CategoryId);
            if (categoryDto == null)
            {
                throw new CategoryNotFoundException(dto.CategoryId);
            }
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId,
                IsActive = true
            };
            var created = await _repo.CreateAsync(product);
            _logger.LogInformation("Product created successfully: {ProductName} with ID {ProductId}", created.Name, created.Id);

            return MapToResponseDto(created);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid product ID");
            }
            var product = await _repo.GetByIdAsync(id);
            if(product == null)
            {
                throw new ProductNotFoundException(id);
            }
            var result = await _repo.DeleteAsync(id);
            if(result)
            {
                _logger.LogInformation("Product deleted successfully: {ProductId}", id);
            }
            return result;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();
            return products.Select(MapToResponseDto);
        }

        public async Task<ProductResponseDto?> GetByIdAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid product ID");
            }
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
            {
                throw new ProductNotFoundException(id);
            }
            ;
            return MapToResponseDto(product);
        }

        public async Task<ProductResponseDto?> UpdateAsync(Guid id, ProductRequestDto dto)
        {
            if(dto == null)
            {
                throw new ValidationException("Invalid product data");
            }
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid product ID");
            }
            var product = await _repo.GetByIdAsync(id);
            if(product == null)
            {
                throw new ProductNotFoundException(id);
            }
            if (string.IsNullOrWhiteSpace(dto.Name)) 
            {
                throw new ValidationException("Product name is required");
            }
            if(dto.Price > 0)
            {
                product.Price = dto.Price;
            } else
            {
                throw new ValidationException("Price must be greater than 0");
            }
            if(dto.StockQuantity >= 0)
            {
                product.StockQuantity = dto.StockQuantity;
            } else
            {
                throw new ValidationException("Stock quanity must be greater than 0");
            }
            if (dto.CategoryId != Guid.Empty)
            {
                var categoryDto = await _categoryService.GetCategoryByIdAsync(dto.CategoryId);
                if (categoryDto == null)
                {
                    throw new CategoryNotFoundException(dto.CategoryId);
                }
                product.CategoryId = dto.CategoryId;
            } else
            {
                throw new ValidationException("Category ID is required");
            }
            product.Name = dto.Name;
            product.Description = dto.Description;
            var updated = await _repo.UpdateAsync(product);
            return MapToResponseDto(product);
        }
        #endregion

        #region Private Helpers
        private static ProductResponseDto MapToResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                IsActive = product.IsActive,
                CreatedAt = product.CreateAt,
                CategoryName = product.Category?.Name,

                ProductImages = product.ProductImages?.Select(pi => new ProductImageResponseDto
                {
                    Id = pi.Id,
                    Url = pi.Url,
                    AltText = pi.AltText,
                    CreatedAt = pi.CreatedAt,
                    UpdatedAt = pi.UpdatedAt
                }).ToList() ?? new List<ProductImageResponseDto>(),
                ProductVariants = product.ProductVariants?.Select(pv => new ProductVariantResponseDto
                {
                    Id = pv.Id,
                    VariantName = pv.VariantName,
                    VariantValue = pv.VariantValue,
                    StockQuantity = pv.StockQuantity,
                    CreatedAt = pv.CreatedAt,
                    UpdatedAt = pv.UpdatedAt
                }).ToList() ?? new List<ProductVariantResponseDto>()
            };
        }
        #endregion
    }
}
