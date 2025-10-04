using MagicECommerce_API.Data;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MagicECommerce_API.Repositories
{
    public class ProductReviewRepository : BaseRepository<ProductReview>, IProductReviewRepository
    {
        public ProductReviewRepository(ApplicationDBContext context) : base(context)
        {}

        public async Task<bool> AnonymizeUserReviewsAsync(Guid userId)
        {
            var reviews = await _context.ProductReviews.Where(r => r.UserId == userId && !r.IsAnonymous)
                .ToListAsync();
            if (!reviews.Any())
                return false;
            foreach(var review in reviews)
            {
                review.UserId = null;
                review.IsAnonymous = true;
                review.DisplayName = "Former Customer";
                review.UpdatedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductReview> CreateReviewAsync(ProductReview review)
        {
            review.UpdatedAt = DateTime.UtcNow;
            review.CreatedAt = DateTime.UtcNow;
            await _context.ProductReviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReviewAsync(Guid id)
        {
            var review = await _context.ProductReviews.FindAsync(id);
            if (review == null)
                return false;
            _context.ProductReviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductReview>> GetAllReviewsAsync()
        {
            return await _context.ProductReviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingForProductAsync(Guid productId)
        {
            var reviews = await _context.ProductReviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();
            return reviews.Any() ? reviews.Average(r => r.Rating) : 0.0;
        }

        public async Task<Dictionary<int, int>> GetRatingDistributionForProductAsync(Guid productId)
        {
            var reviews = await _context.ProductReviews
                .Where(r => r.ProductId == productId)
                .GroupBy(r => r.Rating)
                .Select(g => new {Rating = g.Key, Count = g.Count() })
                .ToListAsync();
            var distribution = new Dictionary<int, int>
            {
                {5, 0},
                {4, 0},
                {3, 0},
                {2, 0},
                {1, 0}
            };

            foreach(var item in reviews)
            {
                distribution[item.Rating] = item.Count;
            }
            return distribution;
        }

        public async Task<ProductReview?> GetReviewByIdAsync(Guid id)
        {
            return await _context.ProductReviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<int> GetReviewCountForProductAsync(Guid productId)
        {
            return await _context.ProductReviews
                .CountAsync(r => r.ProductId == productId);
        }

        public async Task<IEnumerable<ProductReview>> GetReviewsByProductIdAsync(Guid productId)
        {
            return await _context.ProductReviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductReview>> GetReviewsByUserIdAsync(Guid userId)
        {
            return await _context.ProductReviews
                .Where(r => r.UserId == userId)
                .Include(r => r.Product)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<ProductReview> UpdateReviewAsync(ProductReview review)
        {
            review.UpdatedAt = DateTime.UtcNow;
            _context.ProductReviews.Update(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> UserHasReviewedProductAsync(Guid userId, Guid productId)
        {
            return await _context.ProductReviews
                .AnyAsync(r => r.UserId == userId && r.ProductId == productId && !r.IsAnonymous);
        }
    }
}
