using System.ComponentModel.DataAnnotations;

namespace MagicECommerce_API.DTOS.Request
{
    public class ProductVariantRequestDto
    {
        public Guid ProductId { get; set; }
        public string VariantName { get; set; } = string.Empty;
        public string VariantValue { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
    }
}
