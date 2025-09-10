using System.ComponentModel.DataAnnotations;

namespace MagicECommerce_API.DTOS.Request
{
    public class ProductImageRequestDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Url { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? AltText { get; set; }

    }
}
