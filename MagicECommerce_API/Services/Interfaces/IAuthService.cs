using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;

namespace MagicECommerce_API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
        Task<UserResponseDto> RegisterAsync(UserCreateRequestDto dto);
        Task<bool> ChangePasswordAsyunc(Guid userId, ChangePasswordRequestDto dto);
        Task<UserResponseDto> GetCurrentUserAsync(Guid userId);
    }
}
