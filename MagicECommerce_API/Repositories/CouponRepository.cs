using MagicECommerce_API.Data;
using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MagicECommerce_API.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDBContext _context;
        public CouponRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Coupon> CreateCouponAsync(Coupon coupon)
        {
            coupon.CreatedAt = DateTime.Now;
            coupon.UpdatedAt = DateTime.Now;
            _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync();
            return coupon;
        }

        public async Task<bool> DeleteCouponAsync(Guid id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null)
                return false;
            coupon.IsActive = false;
            coupon.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Coupon>> GetAllCouponsAsync()
        {
            return await _context.Coupons.ToListAsync();
        }

        public async Task<Coupon?> GetCouponByCodeAsync(string code)
        {
            return await _context.Coupons.FirstOrDefaultAsync(c => c.Code.ToLower() == code.ToLower() && c.IsActive);
        }

        public async Task<Coupon?> GetCouponByIdAsync(Guid id)
        {
            return await _context.Coupons.FindAsync(id);
        }

        public async Task<Coupon?> UpdateCouponAsync(Guid id, Coupon coupon)
        {
            coupon.UpdatedAt = DateTime.Now;
            _context.Coupons.Update(coupon);
            await _context.SaveChangesAsync();
            return coupon;
        }
    }
}
