using System.Net;

namespace MagicECommerce_API.Exceptions.Base
{
    public class EntityNotFoundException : BaseCustomException
    {
        public EntityNotFoundException(string entityName, Guid id) 
            : base($"{entityName} with ID '{id}' not found", HttpStatusCode.NotFound) { }

        public EntityNotFoundException(string entityName, string identifier, object value) 
            : base($"{entityName} with {identifier} '{value}' not found", HttpStatusCode.NotFound) { }
    }
}
