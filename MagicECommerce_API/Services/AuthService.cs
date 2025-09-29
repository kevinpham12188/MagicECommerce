using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Exceptions.Base;
using MagicECommerce_API.Exceptions.UserException;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MagicECommerce_API.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepo, IUserService userService, ITokenService tokenService, ILogger<AuthService> logger)
        {
            _userRepo = userRepo;
            _userService = userService;
            _tokenService = tokenService;
            _logger = logger;
        }

        #region Public Methods
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ValidationException("Email and password are required");
            }
            var user = await _userRepo.GetByEmailAsync(dto.Email.Trim());
            if (user == null)
            {
                throw new UserNotFoundException(dto.Email);
            }
                
            if(!VerifyPassword(dto.Password, user.Password))
            {
                throw new ValidationException("Invalid email or password");
            }
            var userWithRole = await _userRepo.GetWithRoleAsync(user.Id);
            var token = _tokenService.GenerateToken(userWithRole!);

            _logger.LogInformation("User {Email} logged in successfully", dto.Email);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(60),
                User = MapToUserResponseDto(userWithRole!)
            };
        }

        public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordRequestDto dto)
        {
            if(dto == null || string.IsNullOrWhiteSpace(dto.CurrentPassword) || string.IsNullOrWhiteSpace(dto.NewPassword))
            {
                throw new ValidationException("Current and new password are required");
            } 

            if(dto.NewPassword != dto.ConfirmNewPassword)
            {
                throw new ValidationException("Passwords do not match");
            }

            if(dto.NewPassword.Length < 6)
            {
                throw new ValidationException("Password must be at least 6 characters");
            }

            var user = await _userRepo.GetByIdAsync(userId);
            if(user == null)
            {
                throw new UserNotFoundException(userId);
            }

            if(!VerifyPassword(dto.CurrentPassword, user.Password))
            {
                throw new ValidationException("Current password is incorrect");
            }

            user.Password = HashPassword(dto.NewPassword);
            await _userRepo.UpdateAsync(user);

            _logger.LogInformation("Password changed for user {userId}", userId);
            return true;
        }

        public async Task<UserResponseDto> RegisterAsync(UserCreateRequestDto dto)
        {
            return await _userService.CreateUserAsync(dto);
        }

        public async Task<UserResponseDto> GetCurrentUserAsync(Guid userId)
        {
            return await _userService.GetUserByIdAsync(userId);
        }
        #endregion

        #region Private Methods
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "MySuperSecret"));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

        private static UserResponseDto MapToUserResponseDto(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name ?? string.Empty,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
        #endregion
    }
}
