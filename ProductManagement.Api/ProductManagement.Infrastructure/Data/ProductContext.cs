using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Models;

namespace ProductManagement.Infrastructure.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the primary key
            modelBuilder.Entity<Product>().HasKey(p => p.ID);

            // Configure column properties
            modelBuilder.Entity<Product>().Property(p => p.ProductName).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Product>().Property(p => p.Description).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Product>().Property(p => p.UpdatedBy).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(p => p.UpdatedOn).IsRequired().HasMaxLength(100);

            // Relationship with Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // -------------------- CATEGORY CONFIGURATION --------------------
            modelBuilder.Entity<Category>().ToTable("Category"); // 👈 map to actual table name
            modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);

            modelBuilder.Entity<Category>().Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Category>().Property(c => c.Description)
                .HasMaxLength(300);


            base.OnModelCreating(modelBuilder);
        }
    }
}
