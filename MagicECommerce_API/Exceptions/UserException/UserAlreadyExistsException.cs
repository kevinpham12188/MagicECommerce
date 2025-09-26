using MagicECommerce_API.Exceptions.Base;
using System.Net;

namespace MagicECommerce_API.Exceptions.UserException
{
    public class UserAlreadyExistsException : BaseCustomException
    {
        public UserAlreadyExistsException(string email) : base($"User with Email {email} already exists.", HttpStatusCode.Conflict)
        {
        }
    }
}
