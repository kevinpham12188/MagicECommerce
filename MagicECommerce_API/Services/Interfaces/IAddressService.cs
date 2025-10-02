using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;

namespace MagicECommerce_API.Services.Interfaces
{
    public interface IAddressService
    {
        Task<AddressResponseDto> CreateAddressAsync(Guid userId, AddressRequestDto addressDto);
        Task<AddressResponseDto> GetAddressByIdAsync(Guid id, Guid requestingUserId);
        Task<List<AddressResponseDto>> GetUserAddressesAsync(Guid userId);
        Task<AddressResponseDto> GetDefaultAddressAsync(Guid userId);
        Task<AddressResponseDto> UpdateAddressAsync(Guid id, Guid requestingUserId, AddressRequestDto addressDto);
        Task<bool> DeleteAddressAsync(Guid id, Guid requestingUserId);
        Task<AddressResponseDto> SetDefaultAddressAsync(Guid id, Guid requestingUserId);
    }
}
