using MagicECommerce_API.Exceptions.Base;

namespace MagicECommerce_API.Exceptions.RoleException
{
    public class DuplicateRoleException : BaseCustomException
    {
        public DuplicateRoleException(string name) :
         base($"Role with name '{name}' already exists", System.Net.HttpStatusCode.Conflict)
        { }
    }
}
