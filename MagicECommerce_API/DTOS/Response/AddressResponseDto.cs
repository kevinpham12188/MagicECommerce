namespace MagicECommerce_API.DTOS.Response
{
    public class AddressResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Line1 { get; set; } = string.Empty;
        public string Line2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Computed property for full address
        public string FullAddress => $"{Line1}, {(!string.IsNullOrEmpty(Line2) ? Line2 + ", " : "")}{City}, {State} {ZipCode}, {Country}";
    }
}
