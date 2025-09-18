using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Exceptions.Base;
using MagicECommerce_API.Exceptions.CategoryException;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;

namespace MagicECommerce_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(ICategoryRepository repo, ILogger<CategoryService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        #region Public Methods
        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetAllCategoriesAsync();
            return categories.Select(MapToResponseDto);
        }

        public async Task<CategoryResponseDto?> GetCategoryByIdAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid category ID");
            }
            var category = await _repo.GetCategoryByIdAsync(id);
            if (category == null)
                throw new CategoryNotFoundException(id);
            return MapToResponseDto(category);
        }

        public async Task<CategoryResponseDto> CreateCategoryAsync(CategoryRequestDto dto)
        {
            //Validation
            if(dto == null)
            {
                throw new ValidationException("Invalid category data");
            }
            if(string.IsNullOrEmpty(dto.Name))
            {
                throw new ValidationException("Category name is required");
            }
            
            // Check for existing category
            var existingCategories = await _repo.GetAllCategoriesAsync();
            bool categoryExists = existingCategories.Any(c => c.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));
            if (categoryExists)
            {
                throw new DuplicateCategoryException(dto.Name);
            }
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description
            };
            await _repo.CreateCategoryAsync(category);
            _logger.LogInformation("Category created successfully: {CategoryName}", category.Name);
            return MapToResponseDto(category);
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ValidationException("Invalid category Id");
            }
            // Check if category exists before attempting to delete
            var category = await _repo.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new CategoryNotFoundException(id);
            }
            var result = await _repo.DeleteCategoryAsync(id);
            if(result)
            {
                _logger.LogInformation("Category deleted successfully: {CategoryId}", id);
            }
            return result;
        }       

        public async Task<IEnumerable<CategoryResponseDto>> GetCategoriesByNameAsync(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ValidationException("Search name is required");
            }
            var categories = await _repo.GetCategoriesByNameAsync(name.Trim());
            return categories.Select(MapToResponseDto);
        }

        
        public async Task<CategoryResponseDto?> UpdateCategoryAsync(Guid id, CategoryRequestDto dto)
        {
            // Validation
            if(dto == null)
            {
                throw new ValidationException("Invalid category data");
            }
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid category ID");
            }
            if(string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ValidationException("Category name is required");
            }
            var category = await _repo.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new CategoryNotFoundException(id);
            }
            var existingCategories = await _repo.GetAllCategoriesAsync();
            bool duplicateExists = existingCategories.Any(c => c.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase) && c.Id != id);
            if (duplicateExists)
            {
                throw new DuplicateCategoryException(dto.Name);
            }
            category.Name = dto.Name;
            category.Description = dto.Description;
            var updated = await _repo.UpdateCategoryAsync(category);
            _logger.LogInformation("Category updated successfully: {CategoryId}", id);
            return MapToResponseDto(updated);
        }
        #endregion

        #region Private Methods
        private static CategoryResponseDto MapToResponseDto(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }
        #endregion

    }
}
