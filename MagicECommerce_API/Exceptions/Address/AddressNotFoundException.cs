using MagicECommerce_API.Exceptions.Base;
using System.Net;

namespace MagicECommerce_API.Exceptions.Address
{
    public class AddressNotFoundException : BaseCustomException
    {
        public AddressNotFoundException(Guid id) : base("Address not found.", HttpStatusCode.NotFound) { }
    }
}
