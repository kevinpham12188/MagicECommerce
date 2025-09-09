using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Models;

namespace MagicECommerce_API.Services.Interfaces
{
    public interface ICouponService
    {
        Task<IEnumerable<CouponResponseDto>> GetAllCouponsAsync();
        Task<CouponResponseDto?> GetCouponByIdAsync(Guid id);
        Task<CouponResponseDto?> GetCouponByCodeAsync(string code);
        Task<CouponResponseDto> CreateCouponAsync(CouponRequestDto dto);
        Task<CouponResponseDto?> UpdateCouponAsync(Guid id, CouponRequestDto dto);
        Task<bool> DeleteCouponAsync(Guid id);
        Task<bool> ValidateCouponAsync(string code);
        Task<CouponResponseDto?> UseCouponAsync(string code);
    }
}
