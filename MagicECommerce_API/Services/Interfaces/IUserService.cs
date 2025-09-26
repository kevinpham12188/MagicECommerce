using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;

namespace MagicECommerce_API.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> CreateUserAsync(UserCreateRequestDto userDto);
        Task<UserResponseDto> GetUserByIdAsync(Guid id);
        Task<UserResponseDto?> GetUserByEmailAsync(string email);
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<List<UserResponseDto>> GetUsersByRoleAsync(Guid roleId);
        Task<UserResponseDto> UpdateUserAsync(Guid id, UserUpdateRequestDto userDto);
        Task<bool> DeleteUserAsync(Guid id);

    }
}
