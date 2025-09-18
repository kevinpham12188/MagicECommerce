using System.ComponentModel.DataAnnotations;

namespace MagicECommerce_API.DTOS.Response
{
    public class ProductVariantResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string VariantName { get; set; }
        public string VariantValue { get; set; } 
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
