using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;

namespace MagicECommerce_API.Services.Interfaces
{
    public interface IProductVariantService
    {
        Task<IEnumerable<ProductVariantResponseDto>> GetAllVariantsAsync();
        Task<IEnumerable<ProductVariantResponseDto>> GetVariantsByProductIdAsync(Guid productId);
        Task<ProductVariantResponseDto?> GetVariantByIdAsync(Guid id);
        Task<ProductVariantResponseDto?> CreateVariantAsycn(ProductVariantRequestDto dto);
        Task<ProductVariantResponseDto?> UpdateVariantAsync(Guid id, ProductVariantRequestDto dto);
        Task<bool> DeleteVariantAsync(Guid id);
        Task<bool> DeleteVariantsByProductIdAsync(Guid productId);
        Task<bool> VariantExistsAsync(Guid id);
        Task<bool> ProductExistAsync(Guid productId);
        Task<ProductVariantResponseDto?> GetVariantByProductAndNameValueAsync(Guid productId, string name, string value);

    }
}
