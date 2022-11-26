using Japaninja.DomainModel.Models;
using Japaninja.Models.Product;

namespace Japaninja.Creators.ProductCreator;

public interface IProductCreator : ICreator
{
    Product CreateFrom(CreateProduct createProduct);
}