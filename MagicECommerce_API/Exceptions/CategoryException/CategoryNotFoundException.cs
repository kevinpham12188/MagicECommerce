using MagicECommerce_API.Exceptions.Base;
using System.Net;

namespace MagicECommerce_API.Exceptions.CategoryException
{
    public class CategoryNotFoundException : BaseCustomException
    {
        public CategoryNotFoundException(Guid id) : 
            base($"Category not found", HttpStatusCode.NotFound) { }
    }
}
