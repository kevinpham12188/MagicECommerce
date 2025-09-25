using MagicECommerce_API.DTOS;
using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Services;
using MagicECommerce_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicECommerce_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductVariantController : ControllerBase
    {
        private readonly IProductVariantService _variantService;
        public ProductVariantController(IProductVariantService variantService)
        {
            _variantService = variantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productVariants = await _variantService.GetAllVariantsAsync();
            return Ok(new APIResponse<IEnumerable<ProductVariantResponseDto>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = productVariants
            });
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(Guid productId)
        {
            var productVariants = await _variantService.GetVariantsByProductIdAsync(productId);
            return Ok(new APIResponse<IEnumerable<ProductVariantResponseDto>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = productVariants
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var productVariant = await _variantService.GetVariantByIdAsync(id);
            return Ok(new APIResponse<ProductVariantResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = productVariant
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductVariantRequestDto dto)
        {
            var createdVariant = await _variantService.CreateVariantAsync(dto);
            return Ok(new APIResponse<ProductVariantResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = createdVariant
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductVariantRequestDto dto)
        {
            var updatedVariant = await _variantService.UpdateVariantAsync(id, dto);
            return Ok(new APIResponse<ProductVariantResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = updatedVariant
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _variantService.DeleteVariantAsync(id);
            return Ok(new APIResponse<bool>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = success,
                Result = success
            });
        }

        [HttpDelete("product/{productId}")]
        public async Task<IActionResult> DeleteByProductId(Guid productId)
        {
            var success = await _variantService.DeleteVariantsByProductIdAsync(productId);
            return Ok(new APIResponse<bool>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = success,
                Result = success
            });
        }
    }
}
