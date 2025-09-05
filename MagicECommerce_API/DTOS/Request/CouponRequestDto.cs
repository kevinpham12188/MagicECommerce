using System.ComponentModel.DataAnnotations;

namespace MagicECommerce_API.DTOS.Request
{
    public class CouponRequestDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public string DiscountType {  get; set; } = string.Empty;

        [Required]
        public decimal DiscountValue { get; set; }

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        [Required]
        public int UsageLimit { get; set; }
    }
}
