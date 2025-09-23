using MagicECommerce_API.Data;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MagicECommerce_API.Repositories
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly ApplicationDBContext _context;
        public ProductVariantRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<ProductVariant?> CreateVariantAsync(ProductVariant variant)
        {
            _context.ProductVariants.Add(variant);
            await _context.SaveChangesAsync();
            return variant;
        }

        public async Task<bool> DeleteVariantAsync(Guid id)
        {
            var variant = await _context.ProductVariants.FindAsync(id);
            if(variant == null)
            {
                return false;
            }
            _context.ProductVariants.Remove(variant);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteVariantsByProductIdAsync(Guid productId)
        {
            var deletedCount = await _context.ProductVariants
                .Where(pv => pv.ProductId == productId)
                .ExecuteDeleteAsync(); 
            return deletedCount > 0;
        }

        public async Task<IEnumerable<ProductVariant>> GetAllVariantAsync()
        {
            return await _context.ProductVariants
                //.Include(pv => pv.Product)
                .OrderBy(pv => pv.VariantName)
                .ThenBy(pv => pv.VariantValue)
                .ToListAsync();
        }

        public async Task<ProductVariant?> GetVariantByIdAsync(Guid id)
        {
            return await _context.ProductVariants
                //.Include(pv => pv.Product)
                .FirstOrDefaultAsync(pv => pv.Id == id);
        }

        public async Task<ProductVariant?> GetVariantByProductAndNameValueAsync(Guid productId, string name, string value)
        {
            return await _context.ProductVariants
                .FirstOrDefaultAsync(pv => pv.ProductId == productId && pv.VariantName == name && pv.VariantValue == value);
        }

        public async Task<IEnumerable<ProductVariant>> GetVariantsByProductIdAsync(Guid productId)
        {
            return await _context.ProductVariants
                .Where(pv => pv.ProductId == productId)
                .OrderBy(pv => pv.VariantName)
                .ThenBy(pv => pv.VariantValue)
                .ToListAsync();
        }

        public async Task<bool> ProductExistAsync(Guid productId)
        {
            return await _context.Products.AnyAsync(p => p.Id == productId);
        }

        public async Task<ProductVariant?> UpdateVariantAsycn(ProductVariant variant)
        {
            variant.UpdatedAt = DateTime.UtcNow;
            _context.ProductVariants.Update(variant);
            await _context.SaveChangesAsync();
            return variant;
        }

        public async Task<bool> VariantExistsAsync(Guid id)
        {
            return await _context.ProductVariants.AnyAsync(pv => pv.Id == id);
        }
    }
}
