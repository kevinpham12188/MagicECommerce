using MagicECommerce_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicECommerce_API.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Coupon> Coupon { get; set; }
    }
}
