using Japaninja.DomainModel.Models.Interfaces;

namespace Japaninja.DomainModel.Models;

public class ProductsIngredients : IHasId
{
    public string Id { get; set; }

    public string ProductId { get; set; }

    public Product Product { get; set; }

    public string IngredientId { get; set; }

    public Ingredient Ingredient { get; set; }
}