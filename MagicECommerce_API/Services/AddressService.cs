using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Exceptions.Address;
using MagicECommerce_API.Exceptions.Base;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;


namespace MagicECommerce_API.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepo;
        private readonly ILogger<AddressService> _logger;
        public AddressService(IAddressRepository addressRepo, ILogger<AddressService> logger)
        {
            _addressRepo = addressRepo;
            _logger = logger;
        }

        #region Public Methods
        public async Task<AddressResponseDto> CreateAddressAsync(Guid userId, AddressRequestDto addressDto)
        {
            ValidateAddressDto(addressDto);

            // If this is set as default, clear existing defaults
            if(addressDto.IsDefault)
            {
               await _addressRepo.ClearDefaultForUserAsync(userId);
            }

            var address = new Address
            {
                UserId = userId,
                Label = addressDto.Label.Trim(),
                Line1 = addressDto.Line1.Trim(),
                Line2 = addressDto.Line2.Trim() ?? string.Empty,
                City = addressDto.City.Trim(),
                State = addressDto.State.Trim(),
                ZipCode = addressDto.ZipCode.Trim(),
                Country = addressDto.Country.Trim(),
                IsDefault = addressDto.IsDefault
            };

            var createdAddress = await _addressRepo.CreateAsync(address);
            _logger.LogInformation("Created new address with ID {AddressId} for user {UserId}", createdAddress.Id, userId);
            return MapToResponseDto(createdAddress);
        }

        public async Task<bool> DeleteAddressAsync(Guid id, Guid requestingUserId)
        {
            var address = await _addressRepo.GetByIdAsync(id);
            if(address == null)
            {
                throw new AddressNotFoundException(id);
            }
            //Verify ownership
            if(address.UserId != requestingUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this address");
            }
            var deleted = await _addressRepo.DeleteAsync(id);
            _logger.LogInformation("Deleted address with ID {AddressId} for user {UserId}", id, requestingUserId);
            return deleted;
        }

        public async Task<AddressResponseDto> GetAddressByIdAsync(Guid id, Guid requestingUserId)
        {
            var address = await _addressRepo.GetByIdAsync(id);
            if(address == null)
            {
                throw new AddressNotFoundException(id);
            }
            if(address.UserId != requestingUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this address");
            }
            return MapToResponseDto(address);
        }

        public async Task<AddressResponseDto> GetDefaultAddressAsync(Guid userId)
        {
            var address = await _addressRepo.GetDefaultByUserIdAsync(userId);
            return address != null ? MapToResponseDto(address) : null;
        }

        public async Task<List<AddressResponseDto>> GetUserAddressesAsync(Guid userId)
        {
            var addresses = await _addressRepo.GetByUserIdAsync(userId);
            return addresses.Select(MapToResponseDto).ToList();
        }

        public async Task<AddressResponseDto> SetDefaultAddressAsync(Guid id, Guid requestingUserId)
        {
           var address = await _addressRepo.GetByIdAsync(id);
              if (address == null)
              {
                throw new AddressNotFoundException(id);
            }
            //Verify ownership
            if (address.UserId != requestingUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this address");
            }
            await _addressRepo.ClearDefaultForUserAsync(requestingUserId);
            address.IsDefault = true;
            var updated = await _addressRepo.UpdateAsync(address);
            _logger.LogInformation("Set address with ID {AddressId} as default for user {UserId}", updated.Id, requestingUserId);
            return MapToResponseDto(updated);
        }

        public async Task<AddressResponseDto> UpdateAddressAsync(Guid id, Guid requestingUserId, AddressRequestDto addressDto)
        {
            var address = await _addressRepo.GetByIdAsync(id);
            if (address == null)
            {
                throw new AddressNotFoundException(id);
            }
            //Verify ownership
            if (address.UserId != requestingUserId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this address");
            }
            ValidateAddressDto(addressDto);
            // If this is set as default, clear existing defaults
            if (addressDto.IsDefault)
            {
                await _addressRepo.ClearDefaultForUserAsync(requestingUserId);
            }

            address.Label = addressDto.Label.Trim();
            address.Line1 = addressDto.Line1.Trim();
            address.Line2 = addressDto.Line2.Trim() ?? string.Empty;
            address.City = addressDto.City.Trim();
            address.State = addressDto.State.Trim();
            address.ZipCode = addressDto.ZipCode.Trim();
            address.Country = addressDto.Country.Trim();
            address.IsDefault = addressDto.IsDefault;

            var updated = await _addressRepo.UpdateAsync(address);
            _logger.LogInformation("Updated address with ID {AddressId} for user {UserId}", updated.Id, requestingUserId);
            return MapToResponseDto(updated);
        }

        #endregion

        #region Private Methods
        private static void ValidateAddressDto(AddressRequestDto dto)
        {
            if(string.IsNullOrWhiteSpace(dto.Label))
            {
                throw new ValidationException("Label is required");
            }
            if(dto.Label.Length > 50)
            {
                throw new ValidationException("Label must be 50 characters or less");
            }
            if(string.IsNullOrWhiteSpace(dto.Line1))
            {
                throw new ValidationException("Address Line 1 is required");
            }
            if(dto.Line1.Length > 256)
            {
                throw new ValidationException("Address Line 1 must be 256 characters or less");
            }
            if(dto.Line2.Length > 256)
            {
                throw new ValidationException("Address Line 2 must be 256 characters or less");
            }
            if(string.IsNullOrWhiteSpace(dto.City))
            {
                throw new ValidationException("City is required");
            }
            if(dto.City.Length > 256)
            {
                throw new ValidationException("City must be 256 characters or less");
            }
            if (string.IsNullOrWhiteSpace(dto.State))
            {
                throw new ValidationException("State is required");
            }
            if (dto.City.Length > 256)
            {
                throw new ValidationException("State must be 256 characters or less");
            }
            if (string.IsNullOrWhiteSpace(dto.ZipCode))
            {
                throw new ValidationException("Zip Code is required");
            }
            if (dto.City.Length > 20)
            {
                throw new ValidationException("Zip code must be 20 characters or less");
            }
            if (string.IsNullOrWhiteSpace(dto.Country))
            {
                throw new ValidationException("Country is required");
            }
            if (dto.City.Length > 100)
            {
                throw new ValidationException("Country must be 20 characters or less");
            }
        }
        private static AddressResponseDto MapToResponseDto(Address address)
        {
            return new AddressResponseDto
            {
                Id = address.Id,
                UserId = address.UserId,
                Label = address.Label,
                Line1 = address.Line1,
                Line2 = address.Line2,
                City = address.City,
                State = address.State,
                ZipCode = address.ZipCode,
                Country = address.Country,
                IsDefault = address.IsDefault,
                CreatedAt = address.CreatedAt,
                UpdatedAt = address.UpdatedAt
            };
        }
        #endregion
    }
}
