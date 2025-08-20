using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;

namespace MagicECommerce_API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
        Task<CategoryResponseDto?> GetCategoryByIdAsync(Guid id);
        Task<CategoryResponseDto> CreateCategoryAsync(CategoryRequestDto dto);
        Task<CategoryResponseDto?> UpdateCategoryAsync(Guid id, CategoryRequestDto dto);
        Task<bool> DeleteCategoryAsync(Guid id);
        Task<IEnumerable<CategoryResponseDto>> GetCategoriesByNameAsync(string name);
    }
}
