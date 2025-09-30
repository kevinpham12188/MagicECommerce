using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Exceptions.Base;
using MagicECommerce_API.Exceptions.RoleException;
using MagicECommerce_API.Exceptions.UserException;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MagicECommerce_API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _logger = logger;
        }

        #region Public Methods
        public async Task<UserResponseDto> CreateUserAsync(UserCreateRequestDto userDto)
        {
            //Validation
            if(userDto == null)
            {
                throw new ValidationException("Invalid user data.");
            }
            if(string.IsNullOrWhiteSpace(userDto.FirstName))
            {
                throw new ValidationException("First name is required");
            }
            if(userDto.FirstName.Length > 100)
            {
                throw new ValidationException("First name cannot exceed 100 characters");
            }
            if(string.IsNullOrWhiteSpace(userDto.LastName))
            {
                throw new ValidationException("Last name is required"); 
            }
            if(userDto.LastName.Length > 100)
            {
                throw new ValidationException("Last name cannot exceed 100 characters");
            }
            if(string.IsNullOrWhiteSpace(userDto.Email))
            {
                throw new ValidationException("Email is required");
            }
            if(userDto.Email.Length > 150)
            {
                throw new ValidationException("Email cannot exceed 150 characters");
            }
            if(!IsValidEmail(userDto.Email))
            {
                throw new ValidationException("Invalid email format");
            }
            if(string.IsNullOrWhiteSpace(userDto.Password))
            {
                throw new ValidationException("Password is required");
            }
            if(userDto.Password.Length < 6)
            {
                throw new ValidationException("Password must be at least 6 characters long");
            }
            if (!string.IsNullOrEmpty(userDto.Phone) && userDto.Phone.Length > 20) 
            {
                throw new ValidationException("Phone number cannot exceed 20 characters");
            }
            if(userDto.RoleId == Guid.Empty)
            {
                throw new ValidationException("Role ID is required");
            }
            // Check if role exists
            if (!await _roleRepository.RoleExistsAsync(userDto.RoleId))
            {
                throw new RoleNotFoundException(userDto.RoleId);
            }

            // Check for duplicate email
            if(await _userRepository.EmailExistsAsync(userDto.Email))
            {
                throw new UserAlreadyExistsException(userDto.Email.Trim());
            }

            var hashedPassword = HashPassword(userDto.Password);

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = hashedPassword,
                Phone = userDto.Phone?.Trim() ?? string.Empty,
                RoleId = userDto.RoleId
            };
            var createdUser = await _userRepository.CreateAsync(user);
            var userWithRole = await _userRepository.GetWithRoleAsync(createdUser.Id);

            _logger.LogInformation("Created user: {Email} with ID {UserId}", createdUser.Email, createdUser.Id);
            return MapToUserResponseDto(userWithRole);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            // Validation
            if(id  == Guid.Empty)
            {
                throw new ValidationException("Invalid ID");   
            }
            var existingUser = await _userRepository.GetByIdAsync(id);
            if(existingUser == null)
            {
                throw new UserNotFoundException(id);
            }
            var result = await _userRepository.DeleteAsync(id);
            if(result)
            {
                _logger.LogInformation("User deleted successfully: {Email} (ID: {UserId})", existingUser.Email, existingUser.Id);
            }
            return result;
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllWithRolesAsync();
            return users.Select(MapToUserResponseDto).ToList();
        }

        public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
        {
            if(string.IsNullOrEmpty(email))
            {
                throw new ValidationException("Email is required");
            }
            var user = await _userRepository.GetByEmailAsync(email);
            return user == null ? null : MapToUserResponseDto(user);
        }

        public async Task<UserResponseDto> GetUserByIdAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid user ID");
            }
            var user = await _userRepository.GetByIdAsync(id);
            if(user == null)
            {
                throw new UserNotFoundException(id);
            }
            return MapToUserResponseDto(user);
        }

        public async Task<List<UserResponseDto>> GetUsersByRoleAsync(Guid roleId)
        {
            if(roleId == Guid.Empty)
            {
                throw new ValidationException("Invalid role ID");
            }
            if(!await _roleRepository.RoleExistsAsync(roleId))
            {
                throw new RoleNotFoundException(roleId);
            }
            var users = await _userRepository.GetUsersByRoleIdAsync(roleId);
            return users.Select(MapToUserResponseDto).ToList();
        }

        public async Task<UserResponseDto> UpdateUserAsync(Guid id, UserUpdateRequestDto userDto)
        {
            // Validation
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid user ID");
            }
            if(userDto == null)
            {
                throw new ValidationException("Invalid user data");
            }
            if(string.IsNullOrWhiteSpace(userDto.FirstName))
            {
                throw new ValidationException("First name is required");
            }
            if(userDto.FirstName.Length > 100)
            {
                throw new ValidationException("First name cannot exceed 100 characters");
            }
            if(string.IsNullOrWhiteSpace(userDto.LastName))
            {
                throw new ValidationException("Last name is required");
            }
            if(userDto.LastName.Length > 100)
            {
                throw new ValidationException("Last name cannot exceed 100 characters");
            }
            if(string.IsNullOrWhiteSpace(userDto.Email))
            {
                throw new ValidationException("Email is required");
            }
            if(userDto.Email.Length > 150)
            {
                throw new ValidationException("Email cannot exceed 150 characters");
            }
            if(!IsValidEmail(userDto.Email))
            {
                throw new ValidationException("Invalid email format");
            }
            if (!string.IsNullOrEmpty(userDto.Phone) && userDto.Phone.Length > 20)
            {
                throw new ValidationException("Phone number cannot exceed 20 characters");
            }
            if(userDto.RoleId == Guid.Empty)
            {
                throw new ValidationException("Role ID is required");
            }
            // Check if user exists
            var existingUser = await _userRepository.GetByIdAsync(id);
            if(existingUser == null)
            {
                throw new UserNotFoundException(id);
            }

            // Check if role exists
            if(!await _roleRepository.RoleExistsAsync(userDto.RoleId))
            {
                throw new RoleNotFoundException(userDto.RoleId);
            }

            // Check for duplicate email
            if(await _userRepository.EmailExistsAsync(userDto.Email.Trim(), id))
            {
                throw new UserAlreadyExistsException(userDto.Email.Trim());
            }

            // Update user fields
            existingUser.FirstName = userDto.FirstName.Trim();
            existingUser.LastName = userDto.LastName.Trim();
            existingUser.Email = userDto.Email.Trim().ToLower();
            existingUser.Phone = userDto.Phone?.Trim() ?? string.Empty;
            existingUser.RoleId = userDto.RoleId;

            var updatedUser = await _userRepository.UpdateAsync(existingUser);
            var userWithRole = await _userRepository.GetWithRoleAsync(updatedUser.Id);
            _logger.LogInformation("Updated user: {Email} with ID {UserId}", updatedUser.Email, updatedUser.Id);
            return MapToUserResponseDto(userWithRole!);
        }

        #endregion

        #region Private Methods
        private UserResponseDto MapToUserResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name,
                Phone = user.Phone,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "MySuperSecret"));
            return Convert.ToBase64String(hashedBytes);
            //return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password + "salt"));
        }

        private static bool IsValidEmail(string email)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailRegex);
        }
        #endregion

    }
}
