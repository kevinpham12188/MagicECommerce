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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(new APIResponse<List<UserResponseDto>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = users.ToList()
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(new APIResponse<UserResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = user
            });
        }
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(new APIResponse<UserResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = user
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequestDto dto)
        {
            var createdUser = await _userService.CreateUserAsync(dto);
            return Ok(new APIResponse<UserResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = createdUser
            });
        }

        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetUsersByRole(Guid roleId)
        {
            var users = await _userService.GetUsersByRoleAsync(roleId);
            return Ok(new APIResponse<List<UserResponseDto>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = users.ToList()
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateRequestDto dto)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, dto);
            return Ok(new APIResponse<UserResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = updatedUser
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            return Ok(new APIResponse<bool>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = deleted
            });
        }
    }
}
