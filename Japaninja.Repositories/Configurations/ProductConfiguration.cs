using Japaninja.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Japaninja.Repositories.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(Product.MaxNameLength);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(Product.MaxDescriptionLength);
        builder.Property(p => p.Price).IsRequired();
        builder.Property(p => p.Weight).IsRequired();
        builder.Property(p => p.Spiciness).IsRequired();
        builder.Property(p => p.ProductType).IsRequired();

        builder.HasOne<JapaninjaDbContext.SpicinessTypes>()
            .WithMany()
            .HasForeignKey(p => p.Spiciness)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<JapaninjaDbContext.ProductTypes>()
            .WithMany()
            .HasForeignKey(p => p.ProductType)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}