namespace MagicECommerce_API.DTOS.Response
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public UserResponseDto User { get; set; } = new();
    }
}
