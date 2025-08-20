using MagicECommerce_API.DTOS;

namespace MagicECommerce_API.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto?> GetByIdAsync(Guid id);
        Task<ProductResponseDto> CreateAsync(ProductRequestDto dto);
        Task<ProductResponseDto?> UpdateAsync(Guid id, ProductRequestDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
