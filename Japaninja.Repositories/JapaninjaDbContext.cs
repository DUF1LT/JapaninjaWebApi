using System.Reflection;
using Japaninja.DomainModel.Identity;
using Japaninja.DomainModel.Models;
using Japaninja.DomainModel.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories;

public class JapaninjaDbContext : IdentityDbContext
{
    public DbSet<ManagerUser> Managers { get; set; }

    public DbSet<CustomerUser> Customers { get; set; }

    public DbSet<CourierUser> Couriers { get; set; }


    public DbSet<Product> Products { get; set; }

    public DbSet<Ingredient> Ingredients { get; set; }

    public DbSet<ProductsIngredients> ProductsIngredients { get; set; }

    public DbSet<Cutlery> Cutlery { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public DbSet<CustomersOrders> CustomersOrders { get; set; }

    public DbSet<OrdersCutlery> OrdersCutlery { get; set; }

    public DbSet<OrdersProducts> OrdersProducts { get; set; }


    public JapaninjaDbContext(DbContextOptions<JapaninjaDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<OrderStatuses>(entityBuilder =>
        {
            entityBuilder.Property(e => e.Id).HasConversion<string>();
        });

        builder.Entity<ProductTypes>(entityBuilder =>
        {
            entityBuilder.Property(e => e.Id).HasConversion<string>();
        });

        builder.Entity<SpicinessTypes>(entityBuilder =>
        {
            entityBuilder.Property(e => e.Id).HasConversion<string>();
        });

        builder.Entity<IdentityUser>().ToTable("Users");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

        Seed(builder);
    }

    private void Seed(ModelBuilder builder)
    {
        builder.Entity<SpicinessTypes>().HasData(
            new SpicinessTypes { Id = Spiciness.Spicy},
            new SpicinessTypes { Id = Spiciness.NotSpicy}
        );

        builder.Entity<OrderStatuses>().HasData(
            new OrderStatuses { Id = OrderStatus.Processing},
            new OrderStatuses { Id = OrderStatus.Preparing},
            new OrderStatuses { Id = OrderStatus.Ready},
            new OrderStatuses { Id = OrderStatus.Shipping},
            new OrderStatuses { Id = OrderStatus.Delivered},
            new OrderStatuses { Id = OrderStatus.Closed}
        );

        builder.Entity<ProductTypes>().HasData(
            new ProductTypes { Id = ProductType.Sushi},
            new ProductTypes { Id = ProductType.Rolls},
            new ProductTypes { Id = ProductType.Sets},
            new ProductTypes { Id = ProductType.Soups},
            new ProductTypes { Id = ProductType.Noodles},
            new ProductTypes { Id = ProductType.Drinks},
            new ProductTypes { Id = ProductType.Garnish},
            new ProductTypes { Id = ProductType.Snacks}
        );
    }

    public sealed class OrderStatuses
    {
        public OrderStatus Id { get; set; }
    }

    public sealed class ProductTypes
    {
        public ProductType Id { get; set; }
    }

    public sealed class SpicinessTypes
    {
        public Spiciness Id { get; set; }
    }
}