using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;

namespace MagicECommerce_API.Services.Interfaces
{
    public interface IRoleService
    {
        Task<RoleResponseDto> CreateRoleAsync(RoleRequestDto dto);
        Task<RoleResponseDto> GetRoleByIdAsync(Guid id);
        Task<RoleResponseDto> GetRoleByNameAsync(string name);
        Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync();
        Task<RoleResponseDto> UpdateRoleAsync(Guid id, RoleRequestDto dto);
        Task<RoleResponseDto> DeleteRoleAsync(Guid id);

    }
}
