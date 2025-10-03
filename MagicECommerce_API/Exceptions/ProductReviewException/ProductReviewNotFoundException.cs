using MagicECommerce_API.Exceptions.Base;
using System.Net;

namespace MagicECommerce_API.Exceptions.ProductReviewException
{
    public class ProductReviewNotFoundException : BaseCustomException
    {
        public ProductReviewNotFoundException(Guid id) : base("Product review not found.", HttpStatusCode.NotFound)
        {
        }
    }
}
