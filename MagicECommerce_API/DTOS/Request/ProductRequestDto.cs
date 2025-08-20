using System.ComponentModel.DataAnnotations;

namespace MagicECommerce_API.DTOS.Request
{
    public class ProductRequestDto
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}
