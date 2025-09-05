using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;

namespace MagicECommerce_API.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _repo;
        private readonly ILogger<CouponService> _logger;
        public CouponService(ICouponRepository repo, ILogger<CouponService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public Task<CouponResponseDto> CreateCouponAsync(CouponRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCouponAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CouponResponseDto>> GetAllCouponAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CouponResponseDto?> GetCouponByCodeAsync(string code)
        {
            throw new NotImplementedException();
        }

        public Task<CouponResponseDto?> GetCouponByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<CouponResponseDto?> UpdateCouponAsync(Guid id, CouponRequestDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<CouponResponseDto?> UseCouponAsync(string code)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ValidateCouponAsync(string code)
        {
            throw new NotImplementedException();
        }
    }
}
