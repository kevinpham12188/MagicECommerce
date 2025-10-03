namespace MagicECommerce_API.DTOS.Response
{
    public class ProductReviewResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid? UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public bool IsAnonymous { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
