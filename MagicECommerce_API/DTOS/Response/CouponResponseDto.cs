
namespace MagicECommerce_API.DTOS.Response
{
    public class CouponResponseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string DiscountType {  get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int UsageLimit { get; set; }
        public int UsageCount { get; set; }
        public bool isActive { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        
    }
}
