using MagicECommerce_API.Models;

namespace MagicECommerce_API.Repositories.Interfaces
{
    public interface IProductVariantRepository
    {
        Task<IEnumerable<ProductVariant>> GetAllVariantAsync();
        Task<IEnumerable<ProductVariant>> GetVariantsByProductIdAsync(Guid productId);
        Task<ProductVariant?> GetVariantByIdAsync(Guid id);
        Task<ProductVariant?> CreateVariantAsync(ProductVariant variant);
        Task<ProductVariant?> UpdateVariantAsycn(ProductVariant variant);
        Task<bool> DeleteVariantAsync(Guid id);
        Task<bool> DeleteVariantsByProductIdAsync(Guid productId);
        Task<bool> VariantExistsAsync(Guid id);
        Task<bool> ProductExistAsync(Guid productId);
        Task<ProductVariant?> GetVariantByProductAndNameValueAsync(Guid productId, string name, string value);

    }
}
