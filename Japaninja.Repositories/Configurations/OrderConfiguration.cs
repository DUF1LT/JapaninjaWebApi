﻿using Japaninja.DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Japaninja.Repositories.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(p => p.Comment).HasMaxLength(Order.MaxCommentLength);
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.CustomerId).IsRequired();
        builder.Property(p => p.RestaurantId).IsRequired();

        builder.HasOne(p => p.Customer)
            .WithMany()
            .HasForeignKey(p => p.CustomerId);

        builder.HasOne(p => p.Courier)
            .WithMany()
            .HasForeignKey(p => p.CourierId);

        builder.HasOne(p => p.CustomerAddress)
            .WithMany()
            .HasForeignKey(p => p.CustomerAddressId);

        builder.HasOne(p => p.Restaurant)
            .WithMany()
            .HasForeignKey(p => p.RestaurantId);

        builder.HasOne<JapaninjaDbContext.OrderStatuses>()
            .WithMany()
            .HasForeignKey(p => p.Status)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}