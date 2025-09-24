using MagicECommerce_API.Exceptions.Base;
using System.Net;

namespace MagicECommerce_API.Exceptions.ProductVariantException
{
    public class ProductVariantNotFoundException : BaseCustomException
    {
        public ProductVariantNotFoundException(Guid id) :
         base($"Product variant not found", HttpStatusCode.NotFound)
        { }
    }
}
