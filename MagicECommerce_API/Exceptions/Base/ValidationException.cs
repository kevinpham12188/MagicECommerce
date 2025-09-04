using System.Net;

namespace MagicECommerce_API.Exceptions.Base
{
    public class ValidationException : BaseCustomException
    {
        public ValidationException(string message) : base(message, HttpStatusCode.BadRequest) { }
    }
}
