using Japaninja.DomainModel.Models.Enums;
using Japaninja.Models.Product;

namespace Japaninja.Services.Product;

public interface IProductService
{
    Task AddNewProduct(DomainModel.Models.Product product);

    Task<IReadOnlyCollection<DomainModel.Models.Product>> GetProducts(ProductType? type);

    Task<bool> EditProduct(string id, DomainModel.Models.Product product);

    Task<bool> DeleteProduct(string id);
}