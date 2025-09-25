using MagicECommerce_API.Exceptions.Base;
using System.Net;

namespace MagicECommerce_API.Exceptions.RoleException
{
    public class RoleNotFoundException : BaseCustomException
    {
        public RoleNotFoundException(Guid id) :
         base($"Role not found", HttpStatusCode.NotFound)
        { }

        public RoleNotFoundException(string name) :
         base($"Role with name '{name}' not found", HttpStatusCode.NotFound)
        { }
    }
}
