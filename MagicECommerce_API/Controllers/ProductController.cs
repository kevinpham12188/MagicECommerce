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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(new APIResponse<List<ProductResponseDto>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = products.ToList()
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(new APIResponse<ProductResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = product
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductRequestDto productRequest)
        {
            var createdProduct = await _productService.CreateAsync(productRequest);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, new APIResponse<ProductResponseDto>
            {
                StatusCode = HttpStatusCode.Created,
                IsSuccess = true,
                Result = createdProduct
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductRequestDto productRequest)
        {
            var updatedProduct = await _productService.UpdateAsync(id, productRequest);
            return Ok(new APIResponse<ProductResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = updatedProduct
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);
            return Ok(new APIResponse<string>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = "Product deleted successfully"
            });
        }

    }
}
