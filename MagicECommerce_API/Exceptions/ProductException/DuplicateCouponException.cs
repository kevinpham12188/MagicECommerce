using MagicECommerce_API.Exceptions.Base;

namespace MagicECommerce_API.Exceptions.ProductException
{
    public class DuplicateCouponException : BaseCustomException
    {
        public DuplicateCouponException(string code) :
          base($"Coupon with code '{code}' already exists.", System.Net.HttpStatusCode.Conflict)
        { }
    }
}
