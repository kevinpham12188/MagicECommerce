using MagicECommerce_API.Exceptions.Base;
using System.Net;

namespace MagicECommerce_API.Exceptions.ProductImageException
{
    public class ProductImageNotFoundException : BaseCustomException
    {
        public ProductImageNotFoundException(Guid id) :
         base($"Product not found", HttpStatusCode.NotFound)
        { }
    }
}
