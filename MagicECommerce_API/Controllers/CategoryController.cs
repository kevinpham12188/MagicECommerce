using MagicECommerce_API.DTOS;
using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Models;
using MagicECommerce_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicECommerce_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(new APIResponse<List<CategoryResponseDto>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = categories.ToList()
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(new APIResponse<CategoryResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = category
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryRequestDto categoryRequest)
        {
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryRequest);
            return Ok(new APIResponse<CategoryResponseDto>
               {
                   StatusCode = HttpStatusCode.OK,
                   IsSuccess = true,
                   Result = updatedCategory
               });                       
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok(new APIResponse<string>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = "Category deleted successfully"
            });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByName([FromQuery] string name)
        {
            var categories = await _categoryService.GetCategoriesByNameAsync(name);
            return Ok(new APIResponse<List<CategoryResponseDto>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = categories.ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryRequestDto categoryRequest)
        {
                var createdCategory = await _categoryService.CreateCategoryAsync(categoryRequest);
                return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, new APIResponse<CategoryResponseDto>
                {
                    StatusCode = HttpStatusCode.Created,
                    IsSuccess = true,
                    Result = createdCategory
                });
        }
    }
}
