using MagicECommerce_API.Exceptions.Base;
using System.Net;

namespace MagicECommerce_API.Exceptions.CategoryException
{
    public class DuplicateCategoryException : BaseCustomException
    {
        public DuplicateCategoryException(string name) : base($"Category '{name}' already exists", HttpStatusCode.Conflict) { }
    }
}
