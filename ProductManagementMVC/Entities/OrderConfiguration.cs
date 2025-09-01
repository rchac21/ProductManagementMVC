using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagementMVC.Entities;
using System.Reflection.Emit;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(c => c.Id);

        builder.HasOne(o => o.User)            // Order-ს ჰყავს ერთი User
            .WithMany()                     // User-ს ბევრი Order
            .HasForeignKey(o => o.UserId)   // FK
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(o => o.DrinkNavigation)
        .WithMany()
        .HasForeignKey(o => o.Drink)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.FoodNavigation)
        .WithMany()
        .HasForeignKey(o => o.Food)
        .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.SweetNavigation)
        .WithMany()
        .HasForeignKey(o => o.Sweet)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
