using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;

namespace MagicECommerce_API.Services.Interfaces
{
    public interface IProductVariantService
    {
        Task<IEnumerable<ProductVariantResponseDto>> GetAllVariantsAsync();
        Task<IEnumerable<ProductVariantResponseDto>> GetVariantsByProductIdAsync(Guid productId);
        Task<ProductVariantResponseDto> GetVariantByIdAsync(Guid id);
        Task<ProductVariantResponseDto> CreateVariantAsync(ProductVariantRequestDto dto);
        Task<ProductVariantResponseDto> UpdateVariantAsync(Guid id, ProductVariantRequestDto dto);
        Task<bool> DeleteVariantAsync(Guid id);
        Task<bool> DeleteVariantsByProductIdAsync(Guid productId);
    }
}
