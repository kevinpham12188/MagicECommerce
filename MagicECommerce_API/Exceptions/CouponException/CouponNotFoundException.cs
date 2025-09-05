using MagicECommerce_API.Exceptions.Base;
using System.Net;

namespace MagicECommerce_API.Exceptions.CouponException
{
    public class CouponNotFoundException : BaseCustomException
    {
        public CouponNotFoundException(Guid id) :
          base($"Coupon not found", HttpStatusCode.NotFound)
        { }
    }
}
