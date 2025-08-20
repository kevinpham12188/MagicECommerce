using System.Net;

namespace MagicECommerce_API.DTOS
{
    public class APIResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public T? Result { get; set; }
    }
}
