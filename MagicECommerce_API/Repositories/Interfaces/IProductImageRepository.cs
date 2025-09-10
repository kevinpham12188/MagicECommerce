using MagicECommerce_API.Models;

namespace MagicECommerce_API.Repositories.Interfaces
{
    public interface IProductImageRepository
    {
        Task<IEnumerable<ProductImage>> GetAllProductImageAsync();
        Task<IEnumerable<ProductImage>> GetProductImagesByProductIdAsync(Guid productId);
        Task<ProductImage?> GetProductImageByIdAsync(Guid id);
        Task<ProductImage> CreateProductImageAsync(ProductImage productImage);
        Task<ProductImage> UpdateProductImageAsync(ProductImage productImage);
        Task<bool> DeleteProductImageAsync(Guid id);
        Task<bool> DeleteProductImagesByProductIdAsync(Guid productId);
        Task<bool> ProductExistsAsync(Guid productId);
    }
}
