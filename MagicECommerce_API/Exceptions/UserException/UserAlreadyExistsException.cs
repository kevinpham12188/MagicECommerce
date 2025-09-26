using MagicECommerce_API.Exceptions.Base;

namespace MagicECommerce_API.Exceptions.UserException
{
    public class UserAlreadyExistsException : BaseCustomException
    {
        public UserAlreadyExistsException(string email) : base($"User with Email {email} already exists.", System.Net.HttpStatusCode.Conflict)
        {
        }
    }
}
