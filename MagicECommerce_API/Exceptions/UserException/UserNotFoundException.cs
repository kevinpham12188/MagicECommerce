using MagicECommerce_API.Exceptions.Base;
using System.Net;

namespace MagicECommerce_API.Exceptions.UserException
{
    public class UserNotFoundException : BaseCustomException
    {
        public UserNotFoundException(Guid userId) : base($"User with ID {userId} was not found.", HttpStatusCode.NotFound)
        {
        }
        public UserNotFoundException(string email) : base($"User with Email {email} was not found.", HttpStatusCode.NotFound)
        {
        }
    }
}
