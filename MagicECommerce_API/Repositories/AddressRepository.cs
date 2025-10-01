using MagicECommerce_API.Data;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MagicECommerce_API.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDBContext _context;
        public AddressRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Address> CreateAsync(Address address)
        {
            address.CreatedAt = DateTime.UtcNow;
            address.UpdatedAt = DateTime.UtcNow;
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address?> GetByIdAsync(Guid id)
        {
            return await _context.Addresses.FindAsync(id);
        }
        public async Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Addresses.Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
        public async Task<Address?> GetDefaultByUserIdAsync(Guid userId)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault);
        }
        public async Task<Address> UpdateAsync(Address address)
        {
            address.UpdatedAt = DateTime.UtcNow;
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var address = _context.Addresses.Find(id);
            if (address == null)
            {
                return false;
            }
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Addresses.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> UserOwnsAddressAsync(Guid userId, Guid addressId)
        {
            return await _context.Addresses.AnyAsync(a => a.Id == addressId && a.UserId == userId);
        }
        public async Task ClearDefaultForUserAsync(Guid userId)
        {
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId && a.IsDefault)
                .ToListAsync();
            foreach (var address in addresses)
            {
                address.IsDefault = false;
                address.UpdatedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }
    }
}
