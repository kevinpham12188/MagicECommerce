using MagicECommerce_API.Data;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MagicECommerce_API.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDBContext _context;
        public RoleRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Role> CreateAsync(Role role)
        {
            role.UpdatedAt = DateTime.UtcNow;
            role.CreatedAt = DateTime.UtcNow;
            _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return false;
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> RoleExistsAsync(Guid id)
        {
            return await _context.Roles.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> RoleNameExistsAsync(string name)
        {
            return await _context.Roles.AnyAsync(r => r.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> RoleNameExistsAsync(string name, Guid excludeId)
        {
            return await _context.Roles.AnyAsync(r => r.Name.ToLower() == name.ToLower() && r.Id != excludeId);
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            role.UpdatedAt = DateTime.UtcNow;
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
            return role;
        }
    }
}
