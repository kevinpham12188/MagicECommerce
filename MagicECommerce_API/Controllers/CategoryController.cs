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
            if (category == null)
            {
                return NotFound(new APIResponse<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Category not found" }
                });
            }
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
            if (categoryRequest == null)
            {
                return BadRequest(new APIResponse<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid category data" }
                });
            }
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryRequest);
            if (updatedCategory == null)
            {
                return NotFound(new APIResponse<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Category not found" }
                });
            }
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
            var isDeleted = await _categoryService.DeleteCategoryAsync(id);
            if (!isDeleted)
            {
                return NotFound(new APIResponse<string>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Category not found" }
                });
            }
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
            if (categoryRequest == null)
            {
                return BadRequest(new APIResponse<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid category data" }
                });
            }
            //try catch block can be added here for better error handling, catch put conflict if duplicate, show other display error message
            try
            {
                var createdCategory = await _categoryService.CreateCategoryAsync(categoryRequest);
                return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, new APIResponse<CategoryResponseDto>
                {
                    StatusCode = HttpStatusCode.Created,
                    IsSuccess = true,
                    Result = createdCategory
                });
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    var innerMessage = ex.InnerException.Message.ToLower();
                    if (innerMessage.Contains("unique") ||
                        innerMessage.Contains("duplicate") ||
                        innerMessage.Contains("constraint") ||
                        innerMessage.Contains(categoryRequest.Name.ToLower()))
                    {
                        return Conflict(new APIResponse<string>
                        {
                            StatusCode = HttpStatusCode.Conflict,
                            IsSuccess = false,
                            ErrorMessages = new List<string> { $"Category '{categoryRequest.Name}' already exist." }
                        });
                    }
                }
                return StatusCode(500, new APIResponse<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "A database error occurred while creating the category" }
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new APIResponse<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { ex.Message }
                });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new APIResponse<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "An error occurred while creating the category"}
                });
            }
        }
    }
}
