using Japaninja.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Japaninja.Repositories.Configurations;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> builder)
    {
        builder.Property(p => p.Address).IsRequired().HasMaxLength(Restaurant.MaxAddressLength);

        builder.HasOne(p => p.City)
            .WithMany(p => p.Restaurants)
            .HasForeignKey(p => p.CityId);
    }
}