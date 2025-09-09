using MagicECommerce_API.Exceptions.Base;

namespace MagicECommerce_API.Exceptions.CouponException
{
    public class ExpiredCouponException : BaseCustomException
    {
        public ExpiredCouponException() :
          base($"Coupon has expired.", System.Net.HttpStatusCode.BadRequest)
        { }
    }
}
