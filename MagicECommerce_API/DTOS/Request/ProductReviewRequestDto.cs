namespace MagicECommerce_API.DTOS.Request
{
    public class ProductReviewRequestDto
    {
        public Guid ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
