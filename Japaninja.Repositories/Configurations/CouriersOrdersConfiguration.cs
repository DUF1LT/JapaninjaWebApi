using Japaninja.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Japaninja.Repositories.Configurations;

public class CouriersOrdersConfiguration : IEntityTypeConfiguration<CouriersOrders>
{
    public void Configure(EntityTypeBuilder<CouriersOrders> builder)
    {
        builder.HasIndex(p => new { p.CourierId, p.OrderId })
            .IsUnique();

        builder.HasOne(p => p.Order)
            .WithMany()
            .HasForeignKey(p => p.OrderId);

        builder.HasOne(p => p.Courier)
            .WithMany(p => p.Orders)
            .HasForeignKey(p => p.CourierId);
    }
}