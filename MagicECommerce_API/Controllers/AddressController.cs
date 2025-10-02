using MagicECommerce_API.DTOS;
using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicECommerce_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressController : BaseController
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        /// <summary>
        /// Get all addresses for currrent user
        /// </summary>
        [HttpGet("my-addresses")]
        public async Task<IActionResult> GetMyAddresses()
        {
            var userId = GetCurrentUserId();
            var addresses = await _addressService.GetUserAddressesAsync(userId);
            return Ok(new APIResponse<List<AddressResponseDto>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = addresses
            });
        }

        /// <summary>
        /// Get default address for current user
        /// </summary>
        [HttpGet("my-default-address")]
        public async Task<IActionResult> GetMyDefaultAddress()
        {
            var userId = GetCurrentUserId();
            var address = await _addressService.GetDefaultAddressAsync(userId);
            return Ok(new APIResponse<AddressResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = address
            });
        }

        /// <summary>
        /// Get address by id for current user
        /// </summary>
        [HttpGet("my-address/{id}")]
        public async Task<IActionResult> GetMyAddressById(Guid id)
        {
            var userId = GetCurrentUserId();
            var address = await _addressService.GetAddressByIdAsync(id, userId);
            return Ok(new APIResponse<AddressResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = address
            });
        }

        /// <summary>
        /// Create a new address for current user
        /// </summary>
        [HttpPost("create-my-address")]
        public async Task<IActionResult> CreateMyAddress([FromBody] AddressRequestDto addressDto)
        {
            var userId = GetCurrentUserId();
            var address = await _addressService.CreateAddressAsync(userId, addressDto);
            return Ok(new APIResponse<AddressResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = address
            });
        }

        /// <summary>
        /// Update an existing address
        /// </summary>
        [HttpPut("update-my-address/{id}")]
        public async Task<IActionResult> UpdateMyAddress(Guid id, [FromBody] AddressRequestDto addressDto)
        {
            var userId = GetCurrentUserId();
            var address = await _addressService.UpdateAddressAsync(id, userId, addressDto);
            return Ok(new APIResponse<AddressResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = address
            });
        }

        /// <summary>
        /// Delete an address
        /// </summary>
        [HttpDelete("delete-my-address/{id}")]
        public async Task<IActionResult> DeleteMyAddress(Guid id)
        {
            var userId = GetCurrentUserId();
            var deleted = await _addressService.DeleteAddressAsync(id, userId);
            return Ok(new APIResponse<bool>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = deleted
            });
        }

        /// <summary>
        /// Set an address as default
        /// </summary>
        [HttpPut("set-my-default-address/{id}")]
        public async Task<IActionResult> SetMyDefaultAddress(Guid id)
        {
            var userId = GetCurrentUserId();
            var address = await _addressService.SetDefaultAddressAsync(id, userId);
            return Ok(new APIResponse<AddressResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = address
            });
        }
    }
}
