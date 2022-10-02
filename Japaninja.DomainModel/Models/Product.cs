using Japaninja.DomainModel.Models.Enums;
using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class Product : IHasId
{
    public const int MaxNameLength = 50;
    public const int MaxDescriptionLength = 100;


    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Weight { get; set; }

    public float Price { get; set; }

    public ProductType ProductType { get; set; }

    public Spiciness Spiciness { get; set; }

    public IReadOnlyCollection<ProductsIngredients> ProductsIngredients { get; set; }
}