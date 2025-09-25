using MagicECommerce_API.Models;

namespace MagicECommerce_API.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> CreateAsync(Role role);
        Task<Role> GetByIdAsync(Guid id);
        Task<Role> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role> UpdateAsync(Role role);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> RoleExistsAsync(Guid id);
        Task<bool> RoleNameExistsAsync(string name);
        Task<bool> RoleNameExistsAsync(string name, Guid excludeId);
    }
}
