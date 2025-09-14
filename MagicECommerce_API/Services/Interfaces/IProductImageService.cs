using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;

namespace MagicECommerce_API.Services.Interfaces
{
    public interface IProductImageService
    {
        Task<IEnumerable<ProductImageResponseDto>> GetAllProductImagesAsync();
        Task<ProductImageResponseDto?> GetProductImageByIdAsync(Guid id);
        Task<IEnumerable<ProductImageResponseDto>> GetProductImagesByProductIdAsync(Guid productId);
        Task<ProductImageResponseDto> CreateProductImageAsync(ProductImageRequestDto dto);
        Task<ProductImageResponseDto> UpdateProductImageAsync(Guid id, ProductImageRequestDto dto);
        Task<bool> DeleteProductImageAsync(Guid id);
        Task<bool> DeleteProductImagesByProductIdAsync(Guid productId);
    }
}
