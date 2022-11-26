using Japaninja.DomainModel.Models.Enums;

namespace Japaninja.Models.Product;

public class CreateProduct
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string Weight { get; set; }

    public float Price { get; set; }

    public ProductType ProductType { get; set; }

    public Spiciness Spiciness { get; set; }

    public string Ingredients { get; set; }

    public string Image { get; set; }
}