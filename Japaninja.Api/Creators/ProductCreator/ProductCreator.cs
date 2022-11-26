using Japaninja.DomainModel.Models;
using Japaninja.Models.Product;

namespace Japaninja.Creators.ProductCreator;

public class ProductCreator : IProductCreator
{
    public Product CreateFrom(CreateProduct createProduct)
    {
        return new Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = createProduct.Name,
            Description = createProduct.Description,
            Price = createProduct.Price,
            Ingredients = createProduct.Ingredients,
            Spiciness = createProduct.Spiciness,
            Weight = createProduct.Weight,
            ProductType = createProduct.ProductType,
            Image = createProduct.Image,
        };
    }

    public Product Populate(Product product, Product withProductValues)
    {
        product.Name = withProductValues.Name;
        product.Description = withProductValues.Description;
        product.Price = withProductValues.Price;
        product.Ingredients = withProductValues.Ingredients;
        product.Spiciness = withProductValues.Spiciness;
        product.Weight = withProductValues.Weight;
        product.ProductType = withProductValues.ProductType;
        product.Image = withProductValues.Image;

        return product;
    }
}