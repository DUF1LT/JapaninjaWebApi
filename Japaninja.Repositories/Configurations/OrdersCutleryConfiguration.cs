using Japaninja.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Japaninja.Repositories.Configurations;

public class OrdersCutleryConfiguration : IEntityTypeConfiguration<OrdersCutlery>
{
    public void Configure(EntityTypeBuilder<OrdersCutlery> builder)
    {
        builder.HasIndex(p => new { p.OrderId, p.CutleryId })
            .IsUnique();

        builder.HasOne(p => p.Order)
            .WithMany(p => p.Cutlery)
            .HasForeignKey(p => p.OrderId);

        builder.HasOne(p => p.Cutlery)
            .WithMany()
            .HasForeignKey(p => p.CutleryId);
    }
}