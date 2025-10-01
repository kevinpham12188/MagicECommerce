using MagicECommerce_API.Models;

namespace MagicECommerce_API.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Task<Address> CreateAsync(Address address);
        Task<Address?> GetByIdAsync(Guid id);
        Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId);
        Task<Address?> GetDefaultByUserIdAsync(Guid userId);
        Task<Address> UpdateAsync(Address address);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> UserOwnsAddressAsync(Guid userId, Guid addressId);
        Task ClearDefaultForUserAsync(Guid userId);
    }
}
