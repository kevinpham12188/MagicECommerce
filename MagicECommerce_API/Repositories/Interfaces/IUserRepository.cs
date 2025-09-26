using MagicECommerce_API.Models;

namespace MagicECommerce_API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetWithRoleAsync(Guid Id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetAllWithRolesAsync();
        Task<IEnumerable<User>> GetUsersByRoleIdAsync(Guid roleId);
        Task<User?> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UserExistsAsync(Guid id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> EmailExistsAsync(string email, Guid excludeUserId);
    }
}
