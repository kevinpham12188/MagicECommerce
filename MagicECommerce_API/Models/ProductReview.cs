using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicECommerce_API.Models
{
    public class ProductReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //Navigation properties
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
