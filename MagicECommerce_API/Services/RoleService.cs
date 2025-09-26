using MagicECommerce_API.DTOS.Request;
using MagicECommerce_API.DTOS.Response;
using MagicECommerce_API.Exceptions.Base;
using MagicECommerce_API.Exceptions.RoleException;
using MagicECommerce_API.Models;
using MagicECommerce_API.Repositories.Interfaces;
using MagicECommerce_API.Services.Interfaces;

namespace MagicECommerce_API.Services
{
    public class RoleService : IRoleService
    { 
        private readonly IRoleRepository _roleRepo;
        private readonly ILogger<RoleService> _logger;
        public RoleService(IRoleRepository roleRepo, ILogger<RoleService> logger)
        {
            _roleRepo = roleRepo;
            _logger = logger;
        }
        #region Public Methods
        public async Task<RoleResponseDto> CreateRoleAsync(RoleRequestDto dto)
        {
            //Validation
            if(dto == null)
            {
                throw new ValidationException("Invalid role data");
            }

            if(string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ValidationException("Role name is required");
            }

            if(dto.Name.Length > 100)
            {
                throw new ValidationException("Role name is too long");
            }

            if(dto.Description.Length > 255)
            {
                throw new ValidationException("Role description is too long");
            }

            //Check for duplicate role name
            var existingRole = await _roleRepo.GetByNameAsync(dto.Name.Trim());
            if(existingRole != null)
            {
                throw new DuplicateRoleException(dto.Name.Trim());
            }
            var newRole = new Role
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Description = dto.Description.Trim() ?? string.Empty,
            };
            var createRole = await _roleRepo.CreateAsync(newRole);
            _logger.LogInformation("Role created successfully: {RoleName} with ID {RoleId}", createRole.Name, createRole.Id);
            return MapToResponseDto(createRole);
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            //Validation
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid role id");
            }

            var existingRole = await _roleRepo.GetByIdAsync(id);
            if(existingRole == null)
            {
                throw new RoleNotFoundException(id);
            }
            var deleted = await _roleRepo.DeleteAsync(id);
            if(deleted)
            {
                _logger.LogInformation("Role deleted successfully: {RoleName} (ID: {RoleId})", existingRole.Name, id);
            }
            return deleted;
        }

        public async Task<IEnumerable<RoleResponseDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepo.GetAllAsync();
            return roles.Select(MapToResponseDto).ToList();
        }

        public async Task<RoleResponseDto> GetRoleByIdAsync(Guid id)
        {
            //Validation
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid role id");
            }
            var role = await _roleRepo.GetByIdAsync(id);
            if(role == null)
            {
                throw new RoleNotFoundException(id);
            }
            return MapToResponseDto(role);
        }

        public async Task<RoleResponseDto> GetRoleByNameAsync(string name)
        {
            //Validation
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Role name is required");
            }
            var role = await _roleRepo.GetByNameAsync(name.Trim());
            if(role == null)
            {
                throw new RoleNotFoundException(name);
            }
            return MapToResponseDto(role);
        }

        public async Task<RoleResponseDto> UpdateRoleAsync(Guid id, RoleRequestDto dto)
        {
            //Validation
            if(id == Guid.Empty)
            {
                throw new ValidationException("Invalid role id");
            }
            if (dto == null)
            {
                throw new ValidationException("Invalid role data");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ValidationException("Role name is required");
            }

            if (dto.Name.Length > 100)
            {
                throw new ValidationException("Role name is too long");
            }

            if (dto.Description.Length > 255)
            {
                throw new ValidationException("Role description is too long");
            }



            //Check for exist role name
            var roleToUpdate = await _roleRepo.GetByIdAsync(id);
            if (roleToUpdate == null)
            {
                throw new RoleNotFoundException(id);
            }

            //Check for duplicate role name
            if(await _roleRepo.RoleNameExistsAsync(dto.Name.Trim(), id))
            {
                throw new DuplicateRoleException(dto.Name.Trim());
            }

            roleToUpdate.Name = dto.Name.Trim();
            roleToUpdate.Description = dto.Description.Trim() ?? string.Empty;
            var updatedRole = await _roleRepo.UpdateAsync(roleToUpdate);
            _logger.LogInformation("Role updated successfully: {RoleId}", id);
            return MapToResponseDto(updatedRole);
        }
        #endregion


        #region Private Methods
        private static RoleResponseDto MapToResponseDto(Role role)
        {
            return new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedAt = role.CreatedAt,
                UpdatedAt = role.UpdatedAt
            };
        }
        #endregion
    }
}
