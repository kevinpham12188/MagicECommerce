using MagicECommerce_API.Data;
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

        public Task<Coupon> CreateCouponAsync(Coupon coupon)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCouponAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Coupon>> GetAllCouponAsync()
        {
            return await _context.Coupons.ToListAsync();
        }

        public Task<IEnumerable<Coupon>> GetAllCouponsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Coupon?> GetCouponByCodeAsync(string code)
        {
            throw new NotImplementedException();
        }

        public Task<Coupon?> GetCouponByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Coupon?> UpdateCouponAsync(Guid id, Coupon coupon)
        {
            throw new NotImplementedException();
        }
    }
}
