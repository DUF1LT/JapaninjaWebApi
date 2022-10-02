using Japaninja.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Japaninja.Repositories.Configurations;

public class ProductsIngredientsConfiguration : IEntityTypeConfiguration<ProductsIngredients>
{
    public void Configure(EntityTypeBuilder<ProductsIngredients> builder)
    {
        builder.HasIndex(p => new { p.ProductId, p.IngredientId })
            .IsUnique();

        builder.HasOne(p => p.Product)
            .WithMany(p => p.ProductsIngredients)
            .HasForeignKey(p => p.ProductId);

        builder.HasOne(p => p.Ingredient)
            .WithMany()
            .HasForeignKey(p => p.IngredientId);
    }
}