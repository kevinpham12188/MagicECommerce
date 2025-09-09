using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Exceptions.Base;
using MagicECommerce_API.Exceptions.CategoryException;
using MagicECommerce_API.Exceptions.CouponException;
using MagicECommerce_API.Exceptions.ProductException;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;
using System.Runtime.InteropServices;

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

        #region Public Methods

        public async Task<CouponResponseDto> CreateCouponAsync(CouponRequestDto dto)
        {
            // Validation
            if(dto == null)
            {
                throw new ValidationException("Invalid coupon data");
            }
            if(string.IsNullOrWhiteSpace(dto.Code))
            {
                throw new ValidationException("Coupon code is required");
            }
            if(string.IsNullOrWhiteSpace(dto.DiscountType))
            {
                throw new ValidationException("Discount type is required");
            }

            //Validation discount type
            var discountType = dto.DiscountType.ToUpper();
            if(discountType != "PERCENTAGE" && discountType != "FIXED")
            {
                throw new ValidationException("Discount type must be PERCENTAGE or FIXED");
            }

            //Validate discount value
            if(dto.DiscountValue <= 0)
            {
                throw new ValidationException("Discount value must be greater than 0");
            }

            //Validate percentage range
            if(discountType == "PERCENTAGE" && dto.DiscountValue > 100)
            {
                throw new ValidationException("Percentage discount cannot exceed 100%");
            }

            //Validate dates
            if(dto.ValidFrom >= dto.ValidTo)
            {
                throw new ValidationException("Valid from date must be before valid to date");
            }

            //Validate usage limit
            if(dto.UsageLimit <= 0)
            {
                throw new ValidationException("Usage limit must be greater than 0");
            }

            //Check for duplicate code
            var code = dto.Code.Trim().ToUpper();
            var existingCode = await _repo.GetCouponByCodeAsync(code);
            if(existingCode != null)
            {
                throw new DuplicateCouponException(code);
            }

            var coupon = new Coupon
            {
                Id = Guid.NewGuid(),
                Code = code,
                DiscountType = discountType,
                DiscountValue = dto.DiscountValue,
                ValidFrom = dto.ValidFrom,
                ValidTo = dto.ValidTo,
                UsageLimit = dto.UsageLimit,
                UsageCount = 0
            };
            await _repo.CreateCouponAsync(coupon);
            _logger.LogInformation("Coupon created successfully: {CouponCode}", coupon.Code);
            return MapToDto(coupon);
        }

        public async Task<bool> DeleteCouponAsync(Guid id)
        {
            //Validation
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid coupon ID");
            }
            //Check if exist before attempting to delete
            var existingCoupon = await _repo.GetCouponByIdAsync(id);
            if(existingCoupon == null)
            {
                throw new CouponNotFoundException(id);
            }
            var result = await _repo.DeleteCouponAsync(id);
            if(result)
            {
                _logger.LogInformation("Coupon deleted successfully: {CouponId}", id);
            }
            return result;
        }

        public async Task<IEnumerable<CouponResponseDto>> GetAllCouponsAsync()
        {
            var coupons = await _repo.GetAllCouponsAsync();
            return coupons.Select(MapToDto);
        }

        public async Task<CouponResponseDto?> GetCouponByCodeAsync(string code)
        {
            //Validation
            if(string.IsNullOrWhiteSpace(code))
            {
                throw new ValidationException("Coupon code is required");
            }
            var coupon = await _repo.GetCouponByCodeAsync(code.Trim().ToUpper());
            if(coupon == null)
            {
                throw new CouponNotFoundException(Guid.Empty);
            }
            return MapToDto(coupon);
        }

        public async Task<CouponResponseDto?> GetCouponByIdAsync(Guid id)
        {
            //Validation
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid coupon ID");
            }
            var coupon = await _repo.GetCouponByIdAsync(id);
            if(coupon == null)
            {
                throw new CouponNotFoundException(id);
            }
            return MapToDto(coupon);
        }

        public async Task<CouponResponseDto?> UpdateCouponAsync(Guid id, CouponRequestDto dto)
        {
            //Validation
            if (dto == null)
            {
                throw new ValidationException("Invalid coupon data");
            }
            if (id == Guid.Empty)
            {
                throw new ValidationException("Invalid coupon ID");
            }

            // Validate if coupon exists
            var coupon = await _repo.GetCouponByIdAsync(id);
            if (coupon == null)
            {
                throw new CouponNotFoundException(id);
            }

            // Required fields
            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                throw new ValidationException("Coupon code is required");
            }
            if (string.IsNullOrWhiteSpace(dto.DiscountType))
            {
                throw new ValidationException("Discount type is required");
            }

            // Validate discount type
            var discountType = dto.DiscountType.ToUpper();
            if (discountType != "PERCENTAGE" && discountType != "FIXED")
            {
                throw new ValidationException("Invalid discount type. Must be either 'PERCENTAGE' or 'FIXED'");
            }

            // Validate discount value
            if (dto.DiscountValue <= 0)
            {
                throw new ValidationException("Discount value must be greater than zero");
            }

            // Validate percentage discount value
            if (discountType == "PERCENTAGE" && dto.DiscountValue > 100)
            {
                throw new ValidationException("Percentage discount value cannot exceed 100%");
            }

            // Validate date range
            if (dto.ValidFrom >= dto.ValidTo)
            {
                throw new ValidationException("Invalid date range: 'Valid From' must be earlier than 'Valid To'");
            }

            // Validate usage limit
            if (dto.UsageLimit <= 0)
            {
                throw new ValidationException("Usage limit must be greater than zero");
            }
            
            // Uniqueness check
            var code = dto.Code.Trim().ToUpper();
            if(code != coupon.Code)
            {
                var existingCoupon = await _repo.GetCouponByCodeAsync(code);
                if (existingCoupon != null)
                {
                    throw new DuplicateCouponException(code);
                }
            }
            
            coupon.Code = code;
            coupon.DiscountType = discountType;
            coupon.DiscountValue = dto.DiscountValue;
            coupon.ValidFrom = dto.ValidFrom;
            coupon.ValidTo = dto.ValidTo;
            coupon.UsageLimit = dto.UsageLimit;

            var updated = await _repo.UpdateCouponAsync(id, coupon);
            _logger.LogInformation("Coupon updated successfully: {CouponId}", id);
            
            return MapToDto(updated);
        }

        public async Task<CouponResponseDto?> UseCouponAsync(string code)
        {
            if(string.IsNullOrWhiteSpace(code))
            {
                throw new ValidationException("Coupon code is required");
            }
            var coupon = await _repo.GetCouponByCodeAsync(code.Trim().ToUpper());
            if(coupon == null)
            {
                throw new CouponNotFoundException(Guid.Empty);
            }

            var now = DateTime.UtcNow;
            // Check date validity
            if (now < coupon.ValidFrom || now > coupon.ValidTo)
            {
                throw new ExpiredCouponException();
            }

            // Check usage limit
            if(coupon.UsageCount >= coupon.UsageLimit)
            {
                throw new CouponUsageLimitException();
            }

            coupon.UsageCount++;
            var updated = await _repo.UpdateCouponAsync(coupon.Id, coupon);
            _logger.LogInformation("Coupon used successfully: {CouponCode}, Usage: {UsageCount}/{UsageLimit}", coupon.Code, coupon.UsageCount, coupon.UsageLimit);
            return MapToDto(updated);
            }
        

        public async Task<bool> ValidateCouponAsync(string code)
        {
            if(string.IsNullOrWhiteSpace(code))
            {
                return false;
            }
            var coupon = await _repo.GetCouponByCodeAsync(code.Trim().ToUpper());
            if(coupon == null)
            {
                return false;
            }
            var now = DateTime.UtcNow;

            // Check date validity
            if (now < coupon.ValidFrom || now > coupon.ValidTo)
            {
                return false;
            }

            // Check usage limit
            if (coupon.UsageCount >= coupon.UsageLimit)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Private Methods
        private static CouponResponseDto MapToDto(Coupon coupon)
        {
            return new CouponResponseDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                DiscountType = coupon.DiscountType,
                DiscountValue = coupon.DiscountValue,
                ValidFrom = coupon.ValidFrom,
                ValidTo = coupon.ValidTo,
                UsageLimit = coupon.UsageLimit,
                UsageCount = coupon.UsageCount,
                IsActive = coupon.IsActive,
                CreatedAt = coupon.CreatedAt,
                UpdatedAt = coupon.UpdatedAt
            };
        }
        #endregion
    }
}
