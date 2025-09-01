using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagementMVC.Areas.Identity.Data;
using ProductManagementMVC.Entities;
using System.Reflection.Emit;

namespace ProductManagementMVC.Data;

public class ProductManagementMVCContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }

    public ProductManagementMVCContext(DbContextOptions<ProductManagementMVCContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        // ერთი Category -> ბევრი Product
        //builder.Entity<Product>()
        //    .HasOne(p => p.Category)
        //    .WithMany(c => c.Products)
        //    .HasForeignKey(p => p.CategoryId);

        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new OrderConfiguration());
    }
}
