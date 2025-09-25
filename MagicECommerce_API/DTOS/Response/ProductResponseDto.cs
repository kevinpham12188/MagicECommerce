namespace MagicECommerce_API.DTOS.Response
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProductImageResponseDto> ProductImages { get; set; } = new List<ProductImageResponseDto>();
        public List<ProductVariantResponseDto> ProductVariants { get; set; } = new List<ProductVariantResponseDto>();
    }
}
