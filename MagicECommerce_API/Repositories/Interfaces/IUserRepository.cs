using MagicECommerce_API.Models;

namespace MagicECommerce_API.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> CreateAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetWithRoleAsync(Guid Id);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> EmailExistsAsync(string email, Guid excludeUserId);
        Task<IEnumerable<User>> GetAllWithRolesAsync();
        Task<IEnumerable<User>> GetUsersByRoleIdAsync(Guid roleId);
        //Task<bool> UserExistsAsync(Guid id);
    }
}
