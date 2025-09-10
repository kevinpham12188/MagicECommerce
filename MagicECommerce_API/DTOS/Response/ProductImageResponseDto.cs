namespace MagicECommerce_API.DTOS.Response
{
    public class ProductImageResponseDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? AltText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid ProductId { get; set; }
    }
}
