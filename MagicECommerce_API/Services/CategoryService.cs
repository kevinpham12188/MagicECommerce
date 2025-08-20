using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;

namespace MagicECommerce_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetAllCategoriesAsync();
            return categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public async Task<CategoryResponseDto?> GetCategoryByIdAsync(Guid id)
        {
            var category = await _repo.GetCategoryByIdAsync(id);
            if (category == null)
                return null;
            return new CategoryResponseDto 
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<CategoryResponseDto> CreateCategoryAsync(CategoryRequestDto dto)
        {
            //var existingCategory = await _repo.GetAllCategoriesAsync();
            //bool categoryExists = existingCategory.Any(c => c.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase));
            //if(categoryExists )
            //{
            //    throw new Exception($"Category '{dto.Name}' already exists.");
            //}
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description
            };
            await _repo.CreateCategoryAsync(category);
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            return await _repo.DeleteCategoryAsync(id);
        }

        

        public async Task<IEnumerable<CategoryResponseDto>> GetCategoriesByNameAsync(string name)
        {
            var categories = await _repo.GetCategoriesByNameAsync(name);
            return categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }

        

        public async Task<CategoryResponseDto?> UpdateCategoryAsync(Guid id, CategoryRequestDto dto)
        {
            var category = await _repo.GetCategoryByIdAsync(id);
            if (category == null)
                return null;
            category.Name = dto.Name;
            category.Description = dto.Description;
            var updated = await _repo.UpdateCategoryAsync(category);
            return new CategoryResponseDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description
            };
        }
    }
}
