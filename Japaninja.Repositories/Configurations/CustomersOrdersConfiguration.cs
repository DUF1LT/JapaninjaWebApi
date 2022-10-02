using Japaninja.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Japaninja.Repositories.Configurations;

public class CustomersOrdersConfiguration : IEntityTypeConfiguration<CustomersOrders>
{
    public void Configure(EntityTypeBuilder<CustomersOrders> builder)
    {
        builder.HasIndex(p => new { p.CustomerId, p.OrderId })
            .IsUnique();

        builder.HasOne(p => p.Order)
            .WithMany()
            .HasForeignKey(p => p.OrderId);

        builder.HasOne(p => p.Customer)
            .WithMany(p => p.Orders)
            .HasForeignKey(p => p.CustomerId);
    }
}