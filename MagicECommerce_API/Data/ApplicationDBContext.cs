using MagicECommerce_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicECommerce_API.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasOne(pi => pi.Product)
                    .WithMany(p => p.productImages)
                    .HasForeignKey(pi => pi.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(pi => pi.Url)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(pi => pi.AltText)
                    .HasMaxLength(255);
            });
                
        }
    }
}
