using MagicECommerce_API.DTOS;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;

namespace MagicECommerce_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ICategoryService _categoryService;
        public ProductService(IProductRepository repo, ICategoryService categoryService)
        {
            _repo = repo;
            _categoryService = categoryService;
        }
        public async Task<ProductResponseDto> CreateAsync(ProductRequestDto dto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId
            };
            var created = await _repo.CreateAsync(product);

            var categoryDto = await _categoryService.GetCategoryByIdAsync(created.CategoryId);
            created.Category = categoryDto == null ? null : new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            return new ProductResponseDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                Price = created.Price,
                StockQuantity = created.StockQuantity,
                CategoryId = created.CategoryId,
                CategoryName = created.Category?.Name
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();
            return products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.Name : null,
            });
        }

        public async Task<ProductResponseDto?> GetByIdAsync(Guid id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) return null;
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category != null ? product.Category.Name : null,
            };
        }

        public async Task<ProductResponseDto?> UpdateAsync(Guid id, ProductRequestDto dto)
        {
            var product = await _repo.GetByIdAsync(id);
            if(product == null) return null;
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Description = dto.Description;
            product.StockQuantity = dto.StockQuantity;
            product.CategoryId = dto.CategoryId;
            var updated = await _repo.UpdateAsync(product);
            return new ProductResponseDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Price = updated.Price,
                Description = updated.Description,
                StockQuantity = updated.StockQuantity,
                CategoryId = updated.CategoryId,
                CategoryName = updated.Category != null ? updated.Category.Name : null
            };
        }
    }
}
