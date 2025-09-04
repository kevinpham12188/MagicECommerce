using MagicECommerce_API.Exceptions.Base;
using System.Net;

namespace MagicECommerce_API.Exceptions.ProductException
{
    public class ProductNotFoundException : BaseCustomException
    {
        public ProductNotFoundException(Guid id) :
          base($"Product not found", HttpStatusCode.NotFound)
        { }
    }
}
