using Japaninja.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Japaninja.Repositories.Configurations;

public class OrdersProductsConfiguration : IEntityTypeConfiguration<OrdersProducts>
{
    public void Configure(EntityTypeBuilder<OrdersProducts> builder)
    {
        builder.HasIndex(p => new { p.OrderId, p.ProductId })
            .IsUnique();

        builder.HasOne(p => p.Order)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.OrderId);

        builder.HasOne(p => p.Product)
            .WithMany()
            .HasForeignKey(p => p.ProductId);
    }
}