using MagicECommerce_API.DTOS;
using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicECommerce_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;
        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productImages = await _productImageService.GetAllProductImagesAsync();
            return Ok(new APIResponse<IEnumerable<ProductImageResponseDto>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = productImages
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var productImage = await _productImageService.GetProductImageByIdAsync(id);
            return Ok(new APIResponse<ProductImageResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = productImage
            });
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(Guid productId)
        {
            var productImages = await _productImageService.GetProductImagesByProductIdAsync(productId);
            return Ok(new APIResponse<IEnumerable<ProductImageResponseDto>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = productImages
            });
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductImageRequestDto dto)
        {
            var createdProductImage = await _productImageService.CreateProductImageAsync(dto);
            return Ok(new APIResponse<ProductImageResponseDto>
            {
                StatusCode = HttpStatusCode.Created,
                IsSuccess = true,
                Result = createdProductImage
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductImageRequestDto dto)
        {
            var updatedProductImage = await _productImageService.UpdateProductImageAsync(id, dto);
            return Ok(new APIResponse<ProductImageResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = updatedProductImage
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productImageService.DeleteProductImageAsync(id);
            return Ok(new APIResponse<string>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = result,
                Result = "Product image deleted successfully"
            });
        }

        [HttpDelete("product/{productId}")]
        public async Task<IActionResult> DeleteByProductId(Guid productId)
        {
            var result = await _productImageService.DeleteProductImagesByProductIdAsync(productId);
            return Ok(new APIResponse<string>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = result,
                Result = "Product image deleted successfully"
            });
        }
    }
}
