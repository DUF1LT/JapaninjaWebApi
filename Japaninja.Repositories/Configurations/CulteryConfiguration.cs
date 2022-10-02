using Japaninja.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Japaninja.Repositories.Configurations;

public class CutleryConfiguration : IEntityTypeConfiguration<Cutlery>
{
    public void Configure(EntityTypeBuilder<Cutlery> builder)
    {
        builder.Property(p => p.Name).IsRequired().HasMaxLength(Cutlery.MaxNameLength);
    }
}