using MagicECommerce_API.DTOS;
using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Models;
using MagicECommerce_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicECommerce_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var coupons = await _couponService.GetAllCouponsAsync();
            return Ok(new APIResponse<List<CouponResponseDto>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = coupons.ToList()
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var coupon = await _couponService.GetCouponByIdAsync(id);
            return Ok(new APIResponse<CouponResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = coupon
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CouponRequestDto dto)
        {
            var updatedCoupon = await _couponService.UpdateCouponAsync(id, dto);
            return Ok(new APIResponse<CouponResponseDto>
               {
                   StatusCode = HttpStatusCode.OK,
                   IsSuccess = true,
                   Result = updatedCoupon
               });
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var coupon = await _couponService.GetCouponByCodeAsync(code);
            return Ok(new APIResponse<CouponResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = coupon
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CouponRequestDto dto)
        {
            var createdCoupon = await _couponService.CreateCouponAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdCoupon.Id }, new APIResponse<CouponResponseDto>
            {
                StatusCode = HttpStatusCode.Created,
                IsSuccess = true,
                Result = createdCoupon
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _couponService.DeleteCouponAsync(id);
            return Ok(new APIResponse<string>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = "Coupon deleted successfully"
            });
        }

        [HttpPost("validate/{code}")]
        public async Task<IActionResult> Validate(string code)
        {
            var isValid = await _couponService.ValidateCouponAsync(code);
            return Ok(new APIResponse<bool>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = isValid
            });
        }

        [HttpPost("use/{code}")]
        public async Task<IActionResult> Use(string code)
        {
            var usedCoupon = await _couponService.UseCouponAsync(code);
            return Ok(new APIResponse<CouponResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = usedCoupon
            });
        }
    }
}
