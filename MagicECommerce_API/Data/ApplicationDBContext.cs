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
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasOne(pi => pi.Product)
                    .WithMany(p => p.ProductImages)
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

            modelBuilder.Entity<Role>().HasIndex(r => r.Name).IsUnique();
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Label).IsRequired().HasMaxLength(50);
                entity.Property(a => a.Line1).IsRequired().HasMaxLength(256);
                entity.Property(a => a.Line2).HasMaxLength(256);
                entity.Property(a => a.City).IsRequired().HasMaxLength(256);
                entity.Property(a => a.State).IsRequired().HasMaxLength(256);
                entity.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
                entity.Property(a => a.Country).IsRequired().HasMaxLength(100);
                entity.Property(a => a.IsDefault).HasDefaultValue(false);
                entity.HasOne(a => a.User).WithMany().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(a => a.UserId);
                entity.HasIndex(a => new { a.UserId, a.IsDefault });
            });
        }
    }
}
