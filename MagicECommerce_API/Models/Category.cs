using System.ComponentModel.DataAnnotations;

namespace MagicECommerce_API.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(256)]
        public string? Description { get; set; }

    }
}
