using System.Net;

namespace MagicECommerce_API.Exceptions.Base
{
    public abstract class BaseCustomException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        protected BaseCustomException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
