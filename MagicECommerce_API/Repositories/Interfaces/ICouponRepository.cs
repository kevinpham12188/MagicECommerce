using MagicECommerce_API.Models;

namespace MagicECommerce_API.Repositories.Interfaces
{
    public interface ICouponRepository
    {
        Task<IEnumerable<Coupon>> GetAllCouponsAsync();
        Task<Coupon?> GetCouponByIdAsync(Guid id);
        Task<Coupon?> GetCouponByCodeAsync(string code);
        Task<Coupon> CreateCouponAsync(Coupon coupon);
        Task<Coupon?> UpdateCouponAsync(Guid id, Coupon coupon);
        Task<bool> DeleteCouponAsync(Guid id);
    }
}
