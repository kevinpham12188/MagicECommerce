using MagicECommerce_API.Models;
using System.Security.Claims;

namespace MagicECommerce_API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
