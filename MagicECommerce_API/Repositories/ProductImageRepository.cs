using MagicECommerce_API.Data;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MagicECommerce_API.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductImageRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ProductImage> CreateProductImageAsync(ProductImage productImage)
        {
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage;
        }

        public async Task<bool> DeleteProductImageAsync(Guid id)
        {
            var productImage = _context.ProductImages.Find(id);
            if (productImage == null)
            {
                return false;
            }
            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductImagesByProductIdAsync(Guid productId)
        {
            var deletedCount = await _context.ProductImages
                .Where(pi => pi.ProductId == productId)
                .ExecuteDeleteAsync();
            return deletedCount > 0;
        }

        public async Task<IEnumerable<ProductImage>> GetAllProductImageAsync()
        {
            return await _context.ProductImages
                .OrderBy(pi => pi.CreatedAt)
                .ToListAsync();
        }

        public async Task<ProductImage?> GetProductImageByIdAsync(Guid id)
        {
            return await _context.ProductImages.FindAsync(id);
        }

        public async Task<IEnumerable<ProductImage>> GetProductImagesByProductIdAsync(Guid productId)
        {
            return await _context.ProductImages
                .Where(pi => pi.ProductId == productId)
                .OrderBy(pi => pi.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> ProductExistsAsync(Guid productId)
        {
            return await _context.Products.AnyAsync(p => p.Id == productId);
        }

        public async Task<ProductImage> UpdateProductImageAsync(ProductImage productImage)
        {
            productImage.UpdatedAt = DateTime.UtcNow;
            _context.ProductImages.Update(productImage);
            await _context.SaveChangesAsync();
            return productImage;
        }
    }
}
