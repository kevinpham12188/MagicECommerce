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
        public DbSet<ProductVariant> ProductVariants { get; set; }

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
            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.HasOne(pv => pv.Product)
                    .WithMany(p => p.ProductVariants)
                    .HasForeignKey(pv => pv.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(pv => pv.VariantName)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(pv => pv.VariantValue)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(pv => pv.StockQuantity)
                    .IsRequired();
                entity.HasIndex(pv => new { pv.ProductId, pv.VariantName, pv.VariantValue })
                    .IsUnique()
                    .HasDatabaseName("IX_ProductVariants_Unique");
            });
        }
    }
}
