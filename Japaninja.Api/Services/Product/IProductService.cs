using Japaninja.DomainModel.Models.Enums;
using Japaninja.Models.Product;
using Japaninja.Models.Sorting;

namespace Japaninja.Services.Product;

public interface IProductService
{
    Task AddNewProduct(DomainModel.Models.Product product);

    Task<IReadOnlyCollection<DomainModel.Models.Product>> GetProducts(ProductType? type, SortByField? sortField, SortByDirection? sortByDirection);

    Task<bool> EditProduct(string id, DomainModel.Models.Product product);

    Task<bool> DeleteProduct(string id);
}