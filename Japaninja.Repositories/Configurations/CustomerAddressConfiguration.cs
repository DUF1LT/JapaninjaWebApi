using Japaninja.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Japaninja.Repositories.Configurations;

public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.HasOne(p => p.Customer)
            .WithMany(p => p.Addresses)
            .HasForeignKey(p => p.CustomerId);

        builder.Property(p => p.Street).IsRequired().HasMaxLength(300);
        builder.Property(p => p.HouseNumber).IsRequired().HasMaxLength(300);
        builder.Property(p => p.FlatNumber).HasMaxLength(300);
        builder.Property(p => p.Entrance).HasMaxLength(300);
        builder.Property(p => p.Floor).HasMaxLength(300);
    }
}