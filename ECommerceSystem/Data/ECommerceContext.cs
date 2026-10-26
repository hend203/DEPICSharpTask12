using ECommerceSystem.Entities;
using Microsoft.EntityFrameworkCore;
using ECommerceSystem.Entities;
namespace ECommerceSystem.Data;

public class ECommerceContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;Database=ECommerceDb;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            e.Property(c => c.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.Property(c => c.Name).IsRequired().HasMaxLength(100);
            e.Property(c => c.Email).IsRequired().HasMaxLength(200);
            e.HasIndex(c => c.Email).IsUnique();
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.Property(p => p.Name).IsRequired().HasMaxLength(150);
            e.Property(p => p.Price).HasColumnType("decimal(18,2)");

            // One-to-Many: Category -> Product
            e.HasOne(p => p.Category)
             .WithMany(c => c.Products)
             .HasForeignKey(p => p.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // One-to-Many: Customer -> Order
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Many-to-Many: Order <-> Product through OrderDetail
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Products)
            .WithMany(p => p.Orders)
            .UsingEntity<OrderDetail>(
                right => right.HasOne(od => od.Product)
                              .WithMany(p => p.OrderDetails)
                              .HasForeignKey(od => od.ProductId),
                left => left.HasOne(od => od.Order)
                            .WithMany(o => o.OrderDetails)
                            .HasForeignKey(od => od.OrderId),
                join =>
                {
                    join.HasKey(od => new { od.OrderId, od.ProductId });   // composite PK
                    join.Property(od => od.Quantity).HasDefaultValue(1);
                    join.ToTable("OrderDetails");
                });
    }
}
