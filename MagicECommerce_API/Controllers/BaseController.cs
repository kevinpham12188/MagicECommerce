using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MagicECommerce_API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        protected string GetCurrentUserEmail()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }

        protected string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        protected bool IsInRole(params string[] roles)
        {
            var userRole = GetCurrentUserRole();
            return roles.Contains(userRole);
        }
    }
}
