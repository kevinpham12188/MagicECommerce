using MagicECommerce_API.Exceptions.Base;

namespace MagicECommerce_API.Exceptions.ProductException
{
    public class CouponUsageLimitException : BaseCustomException
    {
        public CouponUsageLimitException() :
          base($"Coupon usage limit has been reached.", System.Net.HttpStatusCode.BadRequest)
        { }
    }
}
